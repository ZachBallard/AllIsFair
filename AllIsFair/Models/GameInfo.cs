using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public class GameVM
    {
        public int NumOfAlive { get; set; }
        public int NumOfDead { get; set; }
        public IEnumerable<TileVM> Tiles { get; set; } = new List<TileVM>();
        public PlayerVM Player { get; set; } = new PlayerVM();
        public IEnumerable<Combatant> Combatants { get; set; } = new List<Combatant>();
        public List<Tile> PossibleMoves { get; set; } = new List<Tile>();
        public List<GameAction> GameActions { get; set; } = new List<GameAction>();
    }

    public class PlayerVM
    {
        public string Name { get; set; }
        public string GraphicName { get; set; }
        public string Health { get; set; }
        public string Strength { get; set; }
        public string Speed { get; set; }
        public string Sanity { get; set; }
        public string Perception { get; set; }
        public string Threat { get; set; }
        public string Survivability { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }

    public class TileVM
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public EventType Type { get; set; }
        public Combatant Combatant { get; set; }
        public string GraphicName { get; set; }
        public string CombatantGraphicName { get; set; }
    }

    public class GameInfo
    {
        public int Id { get; set; }

        public ICollection<Tile> Tiles { get; set; } = new List<Tile>();

        public ICollection<Combatant> Combatants { get; set; } = new List<Combatant>();

        public int AliveCombatants => Combatants.Count(x => x.Killer == null);

        public Combatant Player => Combatants.FirstOrDefault(x => x.IsPlayer);

    }
}