using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Description;
using Microsoft.Ajax.Utilities;
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
        }


        public Combatant CurrentPlayer => _db.Games.Find(_currentGameId).Combatants.FirstOrDefault(x => x.TurnOrder == CurrentGame.CurrentTurnOrder);

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
            game.CurrentTurnNumber = 1;
            game.CurrentTurnOrder = 1;

           // game.Events = GetEvents();

            var playerStartingTile = game.Tiles.First(x => x.X == 12 && x.Y == 12);
            var enemyStartingTile = game.Tiles.First(x => x.X == 2 && x.Y == 2);
            game.Combatants.First(x => x.IsPlayer).Tile = playerStartingTile;
            game.Combatants.First(x => !x.IsPlayer).Tile = enemyStartingTile;

            _db.Games.Add(game);

            _db.SaveChanges();

            return game;
        }

        private List<Event> GetEvents()
        {
            var eventList = new List<Event>();

            eventList.Add(new Event()
            {
                Description = "A promising looking shed stands lonely before you. You try the door but it seems barred from the other side. You think you can move it with enough force.",
                Name = "Suspicious Shed",
                TargetNumber = 8,
                RequiredStat = Stat.Strength,
                StatReward = 0,
                ItemReward = new Item() {IsWeapon = false, Name = "Medical Kit", DoesCount = true, Counter = 3, SurvivalBonus = 3, GraphicName = "medkit.png"},
                GraphicName = "suspiciousshed.png",
                Type = EventType.City
            });

            eventList.Add(new Event()
            {
                Description = "This part of the city is unnaturally dark. It claws at your vision. You stumble through the streets hoping to find your way, but something may have found you first.",
                Name = "Something Waiting",
                TargetNumber = 10,
                RequiredStat = Stat.Perception,
                StatReward = 1,
                GraphicName = "inthedark.png",
                Type = EventType.City
            });

            eventList.Add(new Event()
            {
                Description = "An old, battered building hides a lucky find. A revolver. But just as you try to leave with the reward the structure gives in leaving you trapped in a heap. The noise has surely attracted guests.",
                Name = "Trapped!",
                TargetNumber = 22,
                RequiredStat = Stat.Survivability,
                ItemReward = new Item() { IsWeapon = true, Name = "Revolver", DoesCount = false, ThreatBonus = 2, WeaponRange = 2, GraphicName = "revolver.png" },
                GraphicName = "trapped.png",
                Type = EventType.City
            });

            eventList.Add(new Event()
            {
                Description = "The forest wants to keep you forever. It bends and swells to keep you going in circles. Panic begins to set in.",
                Name = "Lost",
                TargetNumber = 6,
                RequiredStat = Stat.Sanity,
                StatReward = 1,
                GraphicName = "lost.png",
                Type = EventType.Wilderness
            });

            eventList.Add(new Event()
            {
                Description = "This used to be a camp. Tatters of clothing and tents are everywhere. Perhaps you can find something useful.",
                Name = "Forgotten Camp",
                TargetNumber = 12,
                RequiredStat = Stat.Perception,
                ItemReward = new Item() { IsWeapon = false, Name = "Bag of Junk", DoesCount = true, Counter = 5, SurvivalBonus = 1, GraphicName = "bagofjunk.png" },
                GraphicName = "forgottencamp.png",
                Type = EventType.Wilderness
            });

            eventList.Add(new Event()
            {
                Description = "Oh look, a baby bear. How cute. My, my, that mother is tall. You need to run. Now.",
                Name = "Mad Mother",
                TargetNumber = 12,
                RequiredStat = Stat.Speed,
                ItemReward = new Item() { IsWeapon = false, Name = "Adrenaline!", DoesCount = true, Counter = 2, SurvivalBonus = 2, GraphicName = "adrenaline.png"},
                GraphicName = "madmother.png",
                Type = EventType.Wilderness
            });

            eventList.Add(new Event()
            {
                Description = "Suddenly, the ground errupts in front of you. Sniper! You need to figure out where the shot came from fast. And not be where another can find you.",
                Name = "Sniper",
                TargetNumber = 9,
                RequiredStat = Stat.Perception,
                StatReward = 1,
                GraphicName = "sniper.png",
                Type = EventType.Expanse
            });

            eventList.Add(new Event()
            {
                Description = "Something is on your trail. Lots of somethings. You can hear the barks and howls. You need to move quickly.",
                Name = "Wolves",
                TargetNumber = 18,
                RequiredStat = Stat.Threat,
                ItemReward = new Item() { IsWeapon = false, Name = "Adrenaline!", DoesCount = true, Counter = 2, SurvivalBonus = 2, GraphicName = "adrenaline.png" },
                GraphicName = "wolves.png",
                Type = EventType.Expanse
            });

            eventList.Add(new Event()
            {
                Description = "Wide open spaces. Nothing to see, nothing to do. But you found a walking stick! With a little work this could make an excellent spear.",
                Name = "Lucky Spear",
                TargetNumber = 19,
                RequiredStat = Stat.Survivability,
                ItemReward = new Item() { IsWeapon = true, Name = "Spear", SurvivalBonus = 1, ThreatBonus = 1, WeaponRange = 1, GraphicName = "adrenaline.png" },
                GraphicName = "luckyspear.png",
                Type = EventType.Expanse
            });

            eventList.Add(new Event()
            {
                Description = "Its hard work getting through these mountains. And it is a long way down. You must press on.",
                Name = "Tricky Climb",
                TargetNumber = 10,
                RequiredStat = Stat.Strength,
                StatReward = 2,
                GraphicName = "trickyclimb.png",
                Type = EventType.Mountain
            });

            eventList.Add(new Event()
            {
                Description = "The sun goes down before you can make it up the cliff, but you found a small overhang. You can hardly see your own hands. Looks like you found your bed. Getting rest here will not be easy.",
                Name = "Stuck",
                TargetNumber = 10,
                RequiredStat = Stat.Sanity,
                StatReward = 1,
                GraphicName = "stuck.png",
                Type = EventType.Mountain
            });

            eventList.Add(new Event()
            {
                Description = "Your handhold gives in, tumbling down the rocky face. A lucky branch catches your bag. You are safe, but it wont be easy getting back up.",
                Name = "Cliffhanger",
                TargetNumber = 8,
                RequiredStat = Stat.Strength,
                StatReward = 1,
                GraphicName = "cliffhanger.png",
                Type = EventType.Mountain
            });

            return eventList;
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

            Random random1 = new Random();
            Random random2 = new Random((int)DateTime.Now.Ticks + random1.Next(0,500));

            for (var i = 1; i <= x; i++)
            {
                for (var j = 1; j <= y; j++)
                {
                    
                    var file = "";
                    var eventType = EventType.Expanse;

                    switch (random2.Next(1, 5))
                    {
                        case 1:
                            file = "Plains.png";
                            eventType = EventType.Expanse;
                            break;
                        case 2:
                            file = "Forest.png";
                            eventType = EventType.Wilderness;
                            break;
                        case 3:
                            file = "Mountain.png";
                            eventType = EventType.Mountain;
                            break;
                        case 4:
                            file = "Town.png";
                            eventType = EventType.City;
                            break;
                    }

                    if (i == 1 || i == 12 || j == 1 || j == 12)
                    {
                        file = "Beach.png";
                        eventType = EventType.Expanse;
                    }

                    map.Add(new Tile { GraphicName = file, X = i, Y = j, Type = eventType });
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

            player.Items.Add(knife);

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

            if (to.Combatant != null && to.Combatant != attacker)
            {
                //attack logic
                var didAttack = TryAttack(attacker, attacker.Tile, to);
                _db.SaveChanges();

                if (didAttack)
                {
                    return EventType.None;
                }

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
            var healthloss = 0;

            if (attackerRoll.Sum() > defenderRoll.Sum())
            {
                healthloss = attackerRoll.Sum() - defenderRoll.Sum();
                defender.Health -= healthloss;
            }

            attacker.Results.Add(new Result()
            {
                TurnNumber = CurrentGame.CurrentTurnNumber,
                Rolls = attackerRoll
            });

            defender.Results.Add(new Result()
            {
                Healthloss = healthloss,
                TurnNumber = CurrentGame.CurrentTurnNumber,
                Rolls = defenderRoll
            });

            RecordAction(Action.Move, $"Attacked {to.Combatant.Name} at {@to.X},{@to.Y}.");
            _db.SaveChanges();


            return true;
        }

        public void DrawEventCard(Combatant player, EventType type)
        {
            var eventCard = CurrentGame.Events.FirstOrDefault(x => x.Type == type);
            CurrentGame.Events.Remove(eventCard);
            CurrentGame.Events.Add(eventCard);

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
            var statReward = eventCard.StatReward * flip;

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

            player.Results.Add(new Result()
            {
                TurnNumber = CurrentGame.CurrentTurnNumber,
                Event = eventCard,
                Rolls = dieResults,
                StatReward = statReward
            });

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

        public ResultVM GetResult()
        {
            var result = new ResultVM();

            var playerResults = CurrentGame.Results.FirstOrDefault(x => x.TurnNumber == CurrentGame.CurrentTurnNumber && x.Combatant == CurrentPlayer);
            var enemyResults = CurrentGame.Results.FirstOrDefault(x => x.TurnNumber == CurrentGame.CurrentTurnNumber && x.Combatant != CurrentPlayer);

            if (playerResults != null)
            {
                result.TurnNumber = CurrentGame.CurrentTurnNumber;
                result.Event = new EventVM()
                {
                    Name = playerResults.Event.Name,
                    GraphicName = playerResults.Event.GraphicName == null ? "" : "/Graphics/" + playerResults.Event.GraphicName,
                    RequiredStat = playerResults.Event.RequiredStat,
                    TargetNumber = playerResults.Event.TargetNumber,
                    Type = playerResults.Event.Type,
                    StatReward = playerResults.Event.StatReward,
                    Description = playerResults.Event.Description
                };

                result.ItemReward = new ItemVM(playerResults.Event.ItemReward);
                result.Rolls = playerResults.Rolls.ToList();
                result.DieResultGraphics = GetDieGraphics(result.Rolls);
                result.StatReward = playerResults.StatReward;
            }

            if (enemyResults == null) return result;

            result.IsAttack = true;
            result.EnemyRolls = enemyResults.Rolls.ToList();
            result.DieResultEnemyGraphics = GetDieGraphics(result.EnemyRolls);
            result.Healthloss = enemyResults.Healthloss;

            return result;
        }

        public void ChangePlayer()
        {
            if (CurrentPlayer.TurnOrder == CurrentGame.Combatants.Count)
            {
                _db.Games.Find(_currentGameId).CurrentTurnNumber++;
                _db.Games.Find(_currentGameId).CurrentTurnOrder = 1;
            }
            else
            {
                _db.Games.Find(_currentGameId).CurrentTurnOrder++;
            }

            _db.SaveChanges();
        }
    }
}