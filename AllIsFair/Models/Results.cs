using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public class Results
    {
        public bool AskWeapon { get; set; }
        public bool didAttack { get; set; }
        public bool didFail { get; set; }
        public List<int> DieResult { get; set; } 
        
        public Event Event { get; set; }
    }
}