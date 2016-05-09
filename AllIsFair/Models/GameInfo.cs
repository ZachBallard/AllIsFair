using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
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