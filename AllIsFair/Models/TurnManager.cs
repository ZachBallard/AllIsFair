using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public class TurnManager
    {
        public int Id { get; set; }

        public List<Combatant> TurnOrder { get; set; }

        public Combatant CurrentTurn => TurnOrder.First();

        public bool isMoving { get; set; }
        public bool isAttacking { get; set; }

        public List<int> DieResult { get; set; } = new List<int>();
        public List<int> DieResultEnemy { get; set; } = new List<int>();
        public Event Event { get; set; }
    }
}