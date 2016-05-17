using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllIsFair.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GraphicName { get; set; }
        public int Counter { get; set; }
        public bool DoesCount { get; set; }
        public bool IsWeapon { get; set; }
        public int WeaponRange { get; set; }
        public int SurvivalBonus { get; set; }
        public int ThreatBonus { get; set; }

        public Combatant Combatant { get; set; }
        //Issues
        //public ICollection<Event> Event { get; set; }
    }
}