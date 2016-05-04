using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
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

        public virtual Game Game { get; set; } 
    }

    public class Game
    {
        public int Id { get; set; }
        public virtual ICollection<Combatant> Combatants { get; set; } = new List<Combatant>();
        public virtual ICollection<Tile> Tiles { get; set; } = new List<Tile>();
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public virtual  ApplicationUser User { get; set; } //but that makes it a one to one issue?
        public virtual int CombatantTurn { get; set; }
    }

    public class Combatant
    {
        public int TurnNumber { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsPlayer { get; set; }

        //original main stats without bonus
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Sanity { get; set; }
        public int Perception { get; set; }
        
        //constant
        [NotMapped]
        public int MaxEquip { get; } = 5;

        [NotMapped]
        public int Threat
        {
            get
            {
                var totalThreat = Strength + Perception;

                foreach (var item in Items)
                {
                    totalThreat = item.ThreatBonus;
                }

                return totalThreat;
            }
        }

        [NotMapped]
        public int Survivability
        {
            get
            {
                var totalSurvival = Sanity + Speed;

                foreach (var item in Items)
                {
                    totalSurvival = item.SurvivalBonus;
                }

                return totalSurvival;
            }
        }

        [NotMapped]
        public int CurrentEquip => Items.Count;

        [Required]
        public virtual Game Game { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public virtual Combatant Killer { get; set; }
        public string GraphicName { get; set; }
    }

    public class Tile
    {
        public int Id { get; set; }
        public string GraphicName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Type { get; set; }

        [Required]
        public virtual Game Game { get; set; }
        public virtual Combatant Combatant { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GraphicName { get; set; } //use for finding in folder
        public int Counter { get; set; }
        public bool DoesCount { get; set; }
        public bool IsWeapon { get; set; }
        public int WeaponRange { get; set; }
        public int SurvivalBonus { get; set; }
        public int ThreatBonus { get; set; }
        public Combatant Combatant { get; set; }
        [Required]
        public virtual Game Game { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GraphicName { get; set; } //use for finding in folder
        public int RequiredStat { get; set; } //st sp sa pe th su
        public int Type { get; set; } //which tile can draw
        public int StatReward { get; set; }
        public Item ItemReward { get; set; }

        [Required]
        public virtual Game Game { get; set; }
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasOptional(u => u.Game).WithRequired(g => g.User);
        }

        public DbSet<Game> Games { get; set; }
    }
}