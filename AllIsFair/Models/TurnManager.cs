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

        public List<int> DieResult { get; set; } = new List<int>();
        public List<string> DieResultGraphics { get; set; } = new List<string>();
        public List<int> DieResultEnemy { get; set; } = new List<int>(); // if empty event was drawn else attack
        public List<string> DieResultEnemyGraphics { get; set; } = new List<string>();
        public Event Event { get; set; }
        public int Healthloss { get; set; }
    }
}