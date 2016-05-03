using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public class GameInfo
    {
        public int Id { get; set; }
        public List<Combatant> Player { get; set; }

        public ICollection<Tile> Tiles { get; set; } = new List<Tile>();

        public ICollection<Combatant> Combatants { get; set; } = new List<Combatant>();

        public int AliveCombatants { get; set; }
    }
}