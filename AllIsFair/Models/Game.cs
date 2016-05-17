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
    }

    public class Result
    {
        public Result()
        {
            
        }
        public Result(int currentTurnNumber, int currentTurnOrder)
        {
            TurnOrder = currentTurnOrder;
            TurnNumber = currentTurnNumber;
        }

        public int Id { get; set; }
        public int TurnNumber { get; set; }
        public int TurnOrder { get; set; }
        public int Healthloss { get; set; }
        public int StatReward { get; set; }

        public bool IsAttack { get; set; }

        public virtual string Rolls { get; set; }
        public virtual Event Event { get; set; }
        public virtual Combatant Combatant { get; set; }
    }
}