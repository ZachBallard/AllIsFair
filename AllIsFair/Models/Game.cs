using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
    }

    public class Result
    {
        public int Id { get; set; }
        public int TurnNumber { get; set; }
        public int Healthloss { get; set; }
        public int StatReward { get; set; }

        [Required]
        public virtual Game Game { get; set; }

        public virtual ICollection<int> Rolls { get; set; } = new List<int>();
        public virtual Event Event { get; set; }
        public virtual Combatant Combatant { get; set; }
    }
}