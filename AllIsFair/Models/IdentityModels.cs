using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
        public virtual ICollection<GameAction> GameActions { get; set; } = new List<GameAction>();
        public virtual ICollection<Combatant> Combatants { get; set; } = new List<Combatant>();
        public virtual ICollection<Tile> Tiles { get; set; } = new List<Tile>();
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public virtual ApplicationUser User { get; set; }
        public int CombatantTurn { get; set; }

        public Event Event { get; set; }

        public TurnManager TurnManager { get; set; }
    }

    public class Combatant
    {

        public Combatant()
        {

        }
        public Combatant(string name, bool isPlayer)
        {
            this.Name = name;
            this.IsPlayer = isPlayer;
            GraphicName = this.IsPlayer ? "Player.png" : "Enemy.png";
        }

        public int TurnNumber { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Tile Tile { get; set; }
        public bool IsPlayer { get; set; }

        //original main stats without bonus
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Sanity { get; set; }
        public int Perception { get; set; }
        public int Health { get; set; } = 10;

        //constant
        [NotMapped]
        public int MaxEquip { get; } = 5;

        [NotMapped]
        public int Threat
        {
            get
            {
                return Strength + Perception + Items.Sum(item => item.ThreatBonus);
            }
        }

        [NotMapped]
        public int Survivability
        {
            get
            {
                return Sanity + Speed + Items.Sum(item => item.SurvivalBonus);
            }
        }

        [NotMapped]
        public int CurrentEquip => Items.Count;

        [Required]
        public virtual Game Game { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        [NotMapped]
        public virtual Event Event { get; set; }
        public virtual Combatant Killer { get; set; }
        public string GraphicName { get; set; }

        public virtual ICollection<GameAction> GameActions { get; set; } = new List<GameAction>();
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

        [Required]
        public Combatant Combatant { get; set; }

    }

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

        [Required]
        public virtual Game Game { get; set; }
    }

    public class TurnManager
    {
        public int Id { get; set; }

        public List<Combatant> TurnOrder { get; set; }

        [NotMapped]
        public Combatant CurrentTurn => TurnOrder.First();

        public bool IsPlayerAction { get; set; } // determine who is on defending if attack

        public List<int> DieResult { get; set; } = new List<int>();
        public List<string> DieResultGraphics { get; set; } = new List<string>();
        public List<int> DieResultEnemy { get; set; } = new List<int>(); // if empty event was drawn else attack
        public List<string> DieResultEnemyGraphics { get; set; } = new List<string>();
        public Event Event { get; set; }
        public int Healthloss { get; set; }

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
            modelBuilder.Entity<Combatant>().HasOptional(u => u.Tile).WithOptionalDependent(g => g.Combatant);
        }

        public DbSet<Game> Games { get; set; }
    }
}