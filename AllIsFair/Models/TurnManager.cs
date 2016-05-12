using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public static class TurnManager
    {
        public int Id { get; set; }

        public List<Combatant> TurnOrder { get; set; } = new List<Combatant>();

        public Combatant CurrentTurn => TurnOrder.First();

        public bool IsPlayerAction { get; set; } // determine who is on defending if attack

  
    }
}