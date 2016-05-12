using System.Collections.Generic;

namespace AllIsFair.Models
{
    public class Game
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int CurrentTurnNumber { get; set; }
        public int CurrentTurnOrder { get; set; }
        public Event Event { get; set; }

        public virtual ICollection<GameAction> GameActions { get; set; } = new List<GameAction>();
        public virtual ICollection<Combatant> Combatants { get; set; } = new List<Combatant>();
        public virtual ICollection<Tile> Tiles { get; set; } = new List<Tile>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }

    
}