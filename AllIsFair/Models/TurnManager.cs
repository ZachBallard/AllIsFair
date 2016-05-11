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

    }
}