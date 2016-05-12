using System.ComponentModel.DataAnnotations;

namespace AllIsFair.Models
{
    public class Tile
    {
        public int Id { get; set; }
        public string GraphicName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public EventType Type { get; set; }

        [Required]
        public virtual Game Game { get; set; }

        public virtual Combatant Combatant { get; set; }
    }
}