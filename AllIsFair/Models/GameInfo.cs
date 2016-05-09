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
        public PlayerVM Player  { get; set; } = new PlayerVM();
    }

    public class PlayerVM
    {
        public string Name { get; set; }
        public string Health { get; set; }
        public string Strength { get; set; }
        public string Speed { get; set; }
        public string Sanity { get; set; }
        public string Perception { get; set; }
        public string Threat { get; set; }
        public string Survivability { get; set; }
    }

    public class TileVM
    {
        public bool IsPossibleMove { get; set; }
        public bool HasCombatant { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public EventType Type { get; set; }
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

        public bool PlayerDone { get; set; }
        public bool AskWeapon { get; set; }
        public bool AskItem { get; set; }
        public bool ShowResult { get; set; }
        public bool IsAttack { get; set; }

        public List<int> DieResult { get; set; }
        public List<int> DieResultEnemy { get; set; }
        public Event Event { get; set; }
        public List<Tile> PossibleMoves { get; set; }
    }
}