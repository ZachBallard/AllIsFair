using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Policy;
using WebGrease.Css.Extensions;

namespace AllIsFair.Models
{
    public class GameManager
    {
        private readonly int _currentGameId;
        private readonly string _currentUserId;

        private bool IsPlayerAction { get; set; } // determine who is on defending if attack

        public List<int> DieResult { get; set; } = new List<int>();
        public List<string> DieResultGraphics { get; set; } = new List<string>();
        public List<int> DieResultEnemy { get; set; } = new List<int>(); // if empty event was drawn else attack
        public List<string> DieResultEnemyGraphics { get; set; } = new List<string>(); 
        public Event Event { get; set; }
        public int Healthloss { get; set; }

        public GameManager(string currentUserId, ApplicationDbContext dbContext)
        {
            _currentUserId = currentUserId;
            _db = dbContext;
            var game = _db.Games.FirstOrDefault(x => x.User.Id == currentUserId) ?? CreateNewGame();

            _currentGameId = game.Id;
        }

        public Combatant CurrentPlayer
        {
            get { return _db.Games.Find(_currentGameId).Combatants.FirstOrDefault(x => x.IsPlayer); }
        }

        public Game CurrentGame => _db.Games.Find(_currentGameId);

        // ReSharper disable once InconsistentNaming
        private ApplicationDbContext _db { get; }

        public Game CreateNewGame()
        {
            var game = new Game
            {
                Tiles = GenerateMap(12, 12),
                User = _db.Users.Find(_currentUserId),
                Combatants = GenerateCombatants(1)
            };

            game.Tiles.ForEach(x => x.Game = game);

            var playerStartingTile = game.Tiles.First(x => x.X == 12 && x.Y == 12);
            var enemyStartingTile = game.Tiles.First(x => x.X == 2 && x.Y == 2);
            game.Combatants.First(x => x.IsPlayer).Tile = playerStartingTile;
            game.Combatants.First(x => !x.IsPlayer).Tile = enemyStartingTile;

            _db.Games.Add(game);

            _db.SaveChanges();

            return game;
        }

        public void DeleteGame()
        {
            //Delete all stuff
            var game = _db.Games.Find(_currentGameId);
            if (game != null)
            {
                _db.Games.Remove(game);
                _db.SaveChanges();
            }
        }

        private ICollection<Tile> GenerateMap(int x, int y)
        {
            var map = new List<Tile>();

            for (var i = 1; i <= x; i++)
            {
                for (var j = 1; j <= y; j++)
                {
                    var file = "Plains.png";
                    if (i == 1 || i == 12 || j == 1 || j == 12)
                    {
                        file = "Beach.png";
                    }

                    map.Add(new Tile { GraphicName = file, X = i, Y = j });
                }
            }

            return map;
        }

        private ICollection<Combatant> GenerateCombatants(int num)
        {
            var combatants = new List<Combatant>();

            var player = new Combatant("Player", true)
            {
                Strength = 4,
                Speed = 2,
                Sanity = 4,
                Perception = 3
            };

            var knife = new Item
            {
                Combatant = player,
                GraphicName = "SmallKnife.png",
                Name = "Knife",
                IsWeapon = true,
                WeaponRange = 1,
                ThreatBonus = 1
            };
            var emptyCard = new Item { Combatant = player, GraphicName = "unknowncard.png", Name = "???" };
            player.Items.Add(knife);
            player.Items.Add(emptyCard);

            combatants.Add(player);

            for (var i = 0; i < num; i++)
            {
                var enemy = new Combatant("Enemy " + i, false)
                {
                    Strength = 4,
                    Speed = 2,
                    Sanity = 5,
                    Perception = 3
                };

                combatants.Add(enemy);
            }

            return combatants;
        }

        private void RecordAction(Action action, string message, Combatant player = null)
        {
            CurrentGame.GameActions.Add(new GameAction(CurrentGame, action, $"{player?.Name}: {message}", player));
            _db.SaveChanges();
        }

        public int PlayerMove(int newX, int newY, Combatant attacker)
        {
            var to = CurrentGame.Tiles.GetTile(newX, newY);
            var didAttack = false;

            if (attacker.CanMove(to))
            {
                if (to.Combatant != null && to.Combatant != attacker)
                {
                    //attack logic
                    didAttack = TryAttack(attacker, attacker.Tile, to);
                    if (didAttack)
                    {
                        RecordAction(Action.Move, $"Attacked {to.Combatant.Name} at {newX},{newY}.");
                    }
                }
                else
                {
                    var oldX = attacker.Tile.X;
                    var oldY = attacker.Tile.Y;
                    attacker.Tile.Combatant = null;
                    attacker.Tile = to;
                    RecordAction(Action.Move, $"Moved from {oldX},{oldY} to {newX},{newY}.");
                }
            }

            _db.SaveChanges();
            if (didAttack)
            {
                return @to.Type;
            }
            return 0;
        }

        private bool TryAttack(Combatant attacker, Tile @from, Tile @to)
        {
            var distance = @from.Distance(to);
            var defender = @to.Combatant;
            var weapon = attacker.Items.FirstOrDefault(x => x.IsWeapon);

            IsPlayerAction = attacker.IsPlayer;

            var didAttack = false;
            var range = 1.5;

            if (weapon != null)
            {
                range = weapon.WeaponRange + 0.5;
            }

            if (distance < range)
            {
                didAttack = true;
                var attackerRoll = GameHelpers.RollDie(attacker.Threat);
                var defenderRoll = GameHelpers.RollDie(defender.Survivability);
                DieResult = attackerRoll;
                DieResultEnemy = defenderRoll;

                if (attackerRoll.Sum() > defenderRoll.Sum())
                {
                    defender.Health -= attackerRoll.Sum() - defenderRoll.Sum();
                    Healthloss = defender.Health;
                }
            }

            _db.SaveChanges();
            return didAttack;
        }

        public void DrawEventCard(Combatant player, EventType type)
        {
            var eventCard = CurrentGame.Events.FirstOrDefault(x => x.Type == type);
            CurrentGame.Events.Remove(eventCard);
            CurrentGame.Events.Add(eventCard);
            Event = eventCard;

            var numOfDice = 0;
            switch (eventCard.RequiredStat)
            {
                case Stat.Strength:
                    numOfDice = player.Strength;
                    break;
                case Stat.Speed:
                    numOfDice = player.Speed;
                    break;
                case Stat.Sanity:
                    numOfDice = player.Sanity;
                    break;
                case Stat.Perception:
                    numOfDice = player.Perception;
                    break;
                case Stat.Threat:
                    numOfDice = player.Threat;
                    break;
                case Stat.Survivability:
                    numOfDice = player.Survivability;
                    break;
            }


            var dieResults = GameHelpers.RollDie(numOfDice);
            var failedRoll = dieResults.Sum() < eventCard.TargetNumber;

            var flip = failedRoll ? -1 : 1;

            switch (eventCard.RequiredStat)
            {
                case Stat.Strength:
                    player.Strength += eventCard.StatReward * flip;
                    break;
                case Stat.Speed:
                    player.Speed += eventCard.StatReward * flip;
                    break;
                case Stat.Sanity:
                    player.Sanity += eventCard.StatReward * flip;
                    break;
                case Stat.Perception:
                    player.Perception += eventCard.StatReward * flip;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            if (!failedRoll && eventCard.ItemReward != null)
            {
                var itemReward = eventCard.ItemReward;

                if (itemReward.IsWeapon)
                {
                    var weapons = player.Items.Where(x => x.IsWeapon);
                    foreach (var w in weapons)
                    {
                        player.Items.Remove(w);
                    }
                    player.Items.Add(itemReward);
                }
                else
                {
                    if (player.CurrentEquip >= player.MaxEquip)
                    {
                        var lowestItem =
                            player.Items.Where(x => !x.IsWeapon).OrderBy(x => x.SurvivalBonus).FirstOrDefault();
                        player.Items.Remove(lowestItem);
                    }
                    player.Items.Add(itemReward);
                }
            }

            //TODO Record draw event card.
            RecordAction(Action.DrawEventCard, "TODO", player);


            _db.SaveChanges();
        }

        public void RemoveResults()
        {
            IsPlayerAction = false;
            DieResult = new List<int>();
            DieResultEnemy = new List<int>();
            Event = new Event();
            Healthloss = 0;
        }

        public List<string> GetDieGraphics(List<int> dieResult)
        {
            var graphicList = new List<string>();

            if (!dieResult.Any())
            {
                return graphicList;
            }

            foreach (var result in dieResult)
            {
                switch (result)
                {
                    case 1:
                        graphicList.Add("/Graphics/die1.png");
                        break;
                    case 2:
                        graphicList.Add("/Graphics/die2.png");
                        break;
                    case 3:
                        graphicList.Add("/Graphics/die3.png");
                        break;
                    case 4:
                        graphicList.Add("/Graphics/die4.png");
                        break;
                    case 5:
                        graphicList.Add("/Graphics/die5.png");
                        break;
                    case 6:
                        graphicList.Add("/Graphics/die6.png");
                        break;
                }
            }

            return graphicList;
        }
    }
}