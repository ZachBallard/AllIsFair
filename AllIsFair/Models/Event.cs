using System.ComponentModel.DataAnnotations;

namespace AllIsFair.Models
{
    public enum Stat
    {
        Strength,
        Speed,
        Sanity,
        Perception,
        Threat,
        Survivability
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GraphicName { get; set; } //use for finding in folder
        public Stat RequiredStat { get; set; } //st sp sa pe th su
        public int TargetNumber { get; set; }
        public EventType Type { get; set; } //which tile can draw
        public int StatReward { get; set; }
        public Item ItemReward { get; set; }
        public string Description { get; set; }

    }
}