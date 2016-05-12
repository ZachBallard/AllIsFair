using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace AllIsFair.Models
{
    public class GameManager
    {
        private readonly int _currentGameId;
        private readonly string _currentUserId;

        public GameManager(string currentUserId, ApplicationDbContext dbContext)
        {
            _currentUserId = currentUserId;
            _db = dbContext;
            var game = _db.Games.FirstOrDefault(x => x.User.Id == currentUserId) ?? CreateNewGame();

            _currentGameId = game.Id;

            TurnManager = new TurnManager(_db);
        }

        public TurnManager TurnManager { get; set; }

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
                Perception = 3,
                TurnOrder = 1
            };

            var knife = new Item
            {
                Combatant = player,
                GraphicName = "SmallKnife.png",
                Name = "Knife",
                IsWeapon = true,
                WeaponRange = 1,
                ThreatBonus = 1,
               

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
                    Perception = 3,
                    TurnOrder = 2 + i
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

        public EventType PlayerMove(int newX, int newY, Combatant attacker)
        {
            var to = CurrentGame.Tiles.GetTile(newX, newY);

            if (!attacker.CanMove(to))
            {
                return EventType.None;
            }

            if (to.Combatant != null && to.Combatant != attacker)
            {
                //attack logic
                 TryAttack(attacker, attacker.Tile, to);
                _db.SaveChanges();
                return EventType.None;

            }

            attacker.Tile.Combatant = null;
            attacker.Tile = to;
            RecordAction(Action.Move, $"Moved from {attacker.Tile.X},{attacker.Tile.Y} to {newX},{newY}.");
            _db.SaveChanges();
            return @to.Type;
        }

        private bool TryAttack(Combatant attacker, Tile @from, Tile @to)
        {
            var distance = @from.Distance(to);
            var defender = @to.Combatant;
            var weapon = attacker.Items.FirstOrDefault(x => x.IsWeapon);

            TurnManager.IsPlayerAction = attacker.IsPlayer;

           
            var range = 1.5;

            if (weapon != null)
            {
                range = weapon.WeaponRange + 0.5;
            }

            if (distance > range)
            {
                return false;
            }


            var attackerRoll = GameHelpers.RollDie(attacker.Threat);
            var defenderRoll = GameHelpers.RollDie(defender.Survivability);

            TurnManager.DieResult = attackerRoll;
            TurnManager.DieResultEnemy = defenderRoll;
            TurnManager.DieResultGraphics = GetDieGraphics(attackerRoll);
            TurnManager.DieResultEnemyGraphics = GetDieGraphics(defenderRoll);

            if (attackerRoll.Sum() > defenderRoll.Sum())
            {
                defender.Health -= attackerRoll.Sum() - defenderRoll.Sum();
                TurnManager.Healthloss = defender.Health;
            }

            RecordAction(Action.Move, $"Attacked {to.Combatant.Name} at {@to.X},{@to.Y}.");
            _db.SaveChanges();


            return true;
        }

        public void DrawEventCard(Combatant player, EventType type)
        {
            var eventCard = CurrentGame.Events.FirstOrDefault(x => x.Type == type);
            CurrentGame.Events.Remove(eventCard);
            CurrentGame.Events.Add(eventCard);
            TurnManager.Event = eventCard;

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

            TurnManager.DieResult = dieResults;
            TurnManager.DieResultGraphics = GetDieGraphics(dieResults);

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
                    if (player.ItemsEquippedCount >= player.MaxEquip)
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

     

        public List<string> GetDieGraphics(IEnumerable<int> dieResult)
        {
            var graphicList = new List<string>();

            graphicList.AddRange(dieResult.Select(result => $"/Graphics/die{result}.png"));

            return graphicList;
        }
    }
}