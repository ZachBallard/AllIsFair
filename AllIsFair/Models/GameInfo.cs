using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public class GameVM
    {
        public int NumOfAlive { get; set; }
        public int NumOfDead { get; set; }
        public IEnumerable<TileVM> Tiles { get; set; } = new List<TileVM>();
        public PlayerVM Player { get; set; } = new PlayerVM();

        public bool IsPlayerAction { get; set; }
        public List<int> DieResult { get; set; } = new List<int>();
        public List<int> DieResultEnemy { get; set; } = new List<int>();
        public List<string> DieResultGraphics { get; set; } = new List<string>();
        public List<string> DieResultEnemyGraphics { get; set; } = new List<string>();
        public List<GameActionVM> GameActions { get; set; } = new List<GameActionVM>();
        public EventVM Event { get; set; }
        public ItemVM Reward { get; set; }
    }

    public class GameActionVM
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
    }

    public class EventVM
    {
        public string Name { get; set; }
        public string GraphicName { get; set; } //use for finding in folder
        public Stat RequiredStat { get; set; } //st sp sa pe th su
        public int TargetNumber { get; set; }
        public EventType Type { get; set; } //which tile can draw
        public int StatReward { get; set; }
        public string Description { get; set; }
    }

    public class PlayerVM
    {
        public string Name { get; set; }
        public string GraphicName { get; set; }
        public string Health { get; set; }
        public string Strength { get; set; }
        public string Speed { get; set; }
        public string Sanity { get; set; }
        public string Perception { get; set; }
        public string Threat { get; set; }
        public string Survivability { get; set; }
        public List<ItemVM> Weapons { get; set; } = new List<ItemVM>();
        public List<ItemVM> Items { get; set; } = new List<ItemVM>();
    }

    public class ItemVM
    {

        public ItemVM(Item x)
        {
            Id = x.Id;
            Name = x.Name;
            GraphicName = "/Graphics/" + x.GraphicName;
            Counter = x.Counter;
            DoesCount = x.DoesCount;
            IsWeapon = x.IsWeapon;
            WeaponRange = x.WeaponRange;
            SurvivalBonus = x.SurvivalBonus;
            ThreatBonus = x.ThreatBonus;

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string GraphicName { get; set; } //use for finding in folder
        public int Counter { get; set; }
        public bool DoesCount { get; set; }
        public bool IsWeapon { get; set; }
        public int WeaponRange { get; set; }
        public int SurvivalBonus { get; set; }
        public int ThreatBonus { get; set; }
    }

    public class TileVM
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public EventType Type { get; set; }
        public string GraphicName { get; set; }
        public string CombatantGraphicName { get; set; }
        public bool IsPossibleMove { get; set; }
    }
}