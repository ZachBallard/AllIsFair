using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AllIsFair.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }

    public class Game
    {
        public int Id { get; set; }
        public ICollection<Combatant> Combatants { get; set; } = new List<Combatant>();
        public ICollection<Tile> Tiles { get; set; } = new List<Tile>();
        public ICollection<Card> Items { get; set; } = new List<Card>();
        public ICollection<Card> Purchases { get; set; } = new List<Card>();
        public ICollection<Card> Events { get; set; } = new List<Card>();
        public ICollection<Effect> Effects { get; set; } = new List<Effect>();

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
        [NotMapped]
        public int MaxEquip { get; } = 3;
        [NotMapped]
        public int MaxHolding { get; } = 2;

        //calculated from cards
        [NotMapped]
        public int Threat { get; set; } = 0;
        [NotMapped]
        public int Survivability { get; set; } = 0;
        [NotMapped]
        public int Holding { get; set; } = 0;
        [NotMapped]
        public int CurrentEquip { get; set; } = 0;

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
        public ICollection<Card> Cards { get; set; } = new List<Card>();
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

        public ICollection<Effect> Effects { get; set; } = new List<Effect>();
    }

    public class Effect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}