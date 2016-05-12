using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllIsFair.Models
{
    public class TurnManager
    {
        private ApplicationDbContext _db;

        public TurnManager(ApplicationDbContext db)
        {
            this._db = db;
        }

        public List<Combatant> TurnOrder { get; set; }

        public Combatant CurrentTurn => TurnOrder.First();

        public bool IsPlayerAction { get; set; } // determine who is on defending if attack

        public List<int> DieResult { get; set; } = new List<int>();
        public List<string> DieResultGraphics { get; set; } = new List<string>();
        public List<int> DieResultEnemy { get; set; } = new List<int>(); // if empty event was drawn else attack
        public List<string> DieResultEnemyGraphics { get; set; } = new List<string>();
        public Event Event { get; set; }
        public int Healthloss { get; set; }

        public void RemoveResults()
        {
            IsPlayerAction = false;
            DieResult = new List<int>();
            DieResultEnemy = new List<int>();
            Event = new Event();
            Healthloss = 0;
        }

    }
}
