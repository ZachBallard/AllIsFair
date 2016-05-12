using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AllIsFair.Models
{
    public class Combatant
    {
        public Combatant()
        {
        }

        public Combatant(string name, bool isPlayer)
        {
            Name = name;
            IsPlayer = isPlayer;
            GraphicName = IsPlayer ? "Player.png" : "Enemy.png";
        }

        public int Id { get; set; }
        public int TurnOrder { get; set; }
        public string Name { get; set; }
        public Tile Tile { get; set; }

        [Required]
        public virtual Game Game { get; set; }

        public bool IsPlayer { get; set; }

        //original main stats without bonus
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Sanity { get; set; }
        public int Perception { get; set; }
        public int Health { get; set; } = 10;

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<GameAction> GameActions { get; set; } = new List<GameAction>();
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();

        public virtual Combatant Killer { get; set; }
        public string GraphicName { get; set; }


        [NotMapped]
        public int MaxEquip { get; } = 5;

        [NotMapped]
        public int Threat
        {
            get { return Strength + Perception + Items.Sum(item => item.ThreatBonus); }
        }

        [NotMapped]
        public int Survivability
        {
            get { return Sanity + Speed + Items.Sum(item => item.SurvivalBonus); }
        }

        [NotMapped]
        public int ItemsEquippedCount => Items.Count;
    }
}