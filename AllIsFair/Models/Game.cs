using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllIsFair.Models
{
    public class Game
    {
        public ICollection<Combatant> Combatants { get; set; } 
        public ICollection<Tile> Tiles { get; set; } 
        public ICollection<Card> Items { get; set; }
        public ICollection<Card> Purchases { get; set; }
        public ICollection<Card> Events { get; set; }
        public ICollection<Effect> Effects { get; set; }
        
    }

    public class Combatant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; } = 0;
        public int Energy { get; set; } = 10;
        public int X { get; set; }
        public int Y { get; set; }

        //constant
        public int MaxEquip { get; } = 3;
        public int MaxHolding { get; } = 2;
       
        //calculated from cards
        public int Threat { get; set; } = 0;
        public int Survivability { get; set; } = 0;
        public int Holding { get; set; } = 2;
        public int CurrentEquip { get; set; }

        //Main stats (any reach 0 and you lose
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Sanity { get; set; }

        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Effect> Effects { get; set; } = new List<Effect>();
        public Combatant Killer { get; set; }
    }

    public class Tile
    {
        public int Id { get; set; }
        public string GraphicName { get; set; }
        public ICollection<Card> Cards { get; set; } 
        public int X { get; set; }
        public int Y { get; set; }
        
    }

    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GraphicName { get; set; } //use for finding in folder
        public int Counter { get; set; }
        public bool DoesCount { get; set; }
        public bool IsOnce { get; set; }
        public int Type { get; set; }

        public ICollection<Effect> Effects { get; set; } 
    }

    public class Effect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}