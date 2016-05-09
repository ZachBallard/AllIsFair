using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using AllIsFair.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace AllIsFair.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private Game _game = null;

        protected Game CurrentGame
        {
            get
            {

                var currentUserId = User.Identity.GetUserId();
                var user = db.Users.Include(x => x.Game).FirstOrDefault(x => x.Id == currentUserId);

                if (user == null) return null;

                _game = user.Game;
                if (_game != null) return _game;

                _game = new Game()
                {
                    Tiles = GenerateMap(12, 12),

                    User = user

                };

                _game.Combatants = GenerateCombatants(_game, 2);
                foreach (var tile in _game.Tiles)
                {
                    foreach (var combatant in _game.Combatants)
                    {
                        if (combatant.X == tile.X && combatant.Y == tile.Y)
                        {
                            tile.Combatant = combatant;
                        }
                    }
                }

                user.Game = _game;

                return _game;
            }
        }

        private ICollection<Combatant> GenerateCombatants(Game game, int num)
        {
            var combatants = new List<Combatant>();

            var player = new Combatant()
            {
                Game = game,
                Name = "Player",
                X = 12,
                Y = 12,
                IsPlayer = true,
                GraphicName = "Player.png",
                Strength = 4,
                Speed = 2,
                Sanity = 4,
                Perception = 3
            };

            var knife = new Item() { Combatant = player, GraphicName = "SmallKnife.png", Name = "Knife", IsWeapon = true, WeaponRange = 1, ThreatBonus = 1, Game = game };
            var emptyCard = new Item() { Combatant = player, GraphicName = "unknowncard.png", Name = "???", Game = game };
            player.Items.Add(knife);
            player.Items.Add(emptyCard);

            combatants.Add(player);

            for (int i = 2; i <= num; i++)
            {

                var enemy = new Combatant()
                {
                    Name = "Enemy" + i,
                    X = i,
                    Y = i,
                    IsPlayer = false,
                    GraphicName = "Enemy.png",
                    Strength = 4,
                    Speed = 2,
                    Sanity = 5,
                    Perception = 3
                };

                combatants.Add(enemy);
            }

            return combatants;
        }

        private ICollection<Tile> GenerateMap(int x, int y)
        {
            var map = new List<Tile>();

            for (int i = 1; i <= x; i++)
            {
                for (int j = 1; j <= y; j++)
                {
                    string file = "Plains.png";
                    if (i == 1 || i == 12 || j == 1 || j == 12)
                    {
                        file = "Beach.png";
                    }

                    map.Add(new Tile() { GraphicName = file, X = i, Y = j });

                }
            }

            return map;
        }

        // GET: Game
        public ActionResult GameState()
        {
            //if (CurrentGame.PlayerDone)
            {
                //check turn
                //if enemy do enemy stuff
                    //if attack check if dead
                //when player
            //    GetGame();
            }

            return View(CurrentGame);
        }

        private void GetGame()
        {
            //determine viable moves for player
            var possibleMoves = new List<Tile>();

            var player = CurrentGame.Combatants.FirstOrDefault(x => x.IsPlayer);

            foreach (var tile in CurrentGame.Tiles)
            {
                var x1 = player.X;
                var y1 = player.Y;
                var x2 = tile.X;
                var y2 = tile.Y;

                var distance = CalculateDistance(x1, y1, x2, y2);

                if (distance < player.Speed + 0.5)
                {
                    possibleMoves.Add(tile);
                }
            }
            //finish view model 
            var CurrentGameInfo = new GameInfo()
            {
                Id = CurrentGame.Id
            };
        }

        private bool TryAttack(int distance, int x1, int y1, int x2, int y2)
        {
            var player = CurrentGame.Combatants.FirstOrDefault(p => p.IsPlayer);
            var weapon = player.Items.FirstOrDefault(x => x.IsWeapon);

            var didAttack = false;
            var range = 1.5;

            if (weapon != null)
            {
                range = weapon.WeaponRange + 0.5;
            }

            if (distance < range)
            {
                didAttack = true;
                //complicated attack stuff
            }

            return didAttack;
        }

        //Post: Game/TryMove
        [HttpPost]
        public ActionResult TryMove(int x2, int y2)
        {
            var player = CurrentGame.Combatants.FirstOrDefault(p => p.IsPlayer);
            var whereFrom = CurrentGame.Tiles.FirstOrDefault(t => t.Combatant == player);
            var whereTo = CurrentGame.Tiles.FirstOrDefault(t => t.X == x2 && t.Y == y2);

            var didAttack = false;

            var x1 = player.X;
            var y1 = player.Y;

            if (whereTo != whereFrom)
            {
                var distance = CalculateDistance(x1, y1, x2, y2);

                if (whereTo.Combatant != null)
                {
                    didAttack = TryAttack(distance, x1, y1, x2, y2);
                }
                else if (distance < player.Speed + 0.5)
                {
                    whereFrom.Combatant = null;
                    whereTo.Combatant = player;

                    player.X = x2;
                    player.Y = y2;

                    db.SaveChanges();
                }
            }

            CurrentGame.PlayerDone = false;

            if (didAttack)
            {
                CurrentGame.ShowResult = true;
            }

            var result = new {didAttack};
            return Json(result);
        }

        private int CalculateDistance(int x1, int y1, int x2, int y2)
        {
            var result = Convert.ToInt32(Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
            return result;
        }

        //GET: Game/DrawEvent
        [HttpPost]
        public ActionResult DrawEvent(int type)
        {
            var player = CurrentGame.Combatants.FirstOrDefault(x => x.IsPlayer);
            var eventCard = CurrentGame.Events.FirstOrDefault(x => x.Type == type);
            CurrentGame.Events.Remove(eventCard);
            CurrentGame.Events.Add(eventCard);
            var usedStat = 0;
            switch (eventCard.RequiredStat)
            {
                case 1:
                    usedStat = player.Strength;
                    break;
                case 2:
                    usedStat = player.Speed;
                    break;
                case 3:
                    usedStat = player.Sanity;
                    break;
                case 4:
                    usedStat = player.Perception;
                    break;
            }

            var dieResult = RollDie(usedStat);

            if (dieResult.Count > eventCard.TargetNumber)
            {
                if (eventCard.StatReward >0)
                {
                    switch (eventCard.RequiredStat)
                    {
                        case 1:
                            player.Strength += eventCard.StatReward;
                            break;
                        case 2:
                            player.Speed += eventCard.StatReward;
                            break;
                        case 3:
                            player.Sanity += eventCard.StatReward;
                            break;
                        case 4:
                            player.Perception += eventCard.StatReward;
                            break;
                    }
                }

                if (eventCard.ItemReward != null)
                {
                    var itemReward = CurrentGame.Items.FirstOrDefault(x => x == eventCard.ItemReward);

                    if (itemReward.IsWeapon && player.Items.Any(x => x.IsWeapon))
                    {
                        if (player.Items.Any(x => x.IsWeapon))
                        {
                            CurrentGame.AskWeapon = true;
                        }
                        else
                        {
                            player.Items.Add(itemReward);
                        }
                    }
                    else
                    {
                        if (player.CurrentEquip < player.MaxEquip)
                        {
                            player.Items.Add(itemReward);
                        }
                        else
                        {
                            CurrentGame.AskItem = true;
                        }
                        
                    }
                }
            }

            db.SaveChanges();

            CurrentGame.ShowResult = true;
            return Content("Done!");
        }

        public List<int> RollDie(int number)
        {
            var allRolls = new List<int>();
            Random random = new Random();
            for (int i = 1; i <= number; i++)
            {
                allRolls.Add(random.Next(1, 7));
            }

            return allRolls;
        }
        // GET: Game/Delete
        public ActionResult Delete()
        {
            //Delete all stuff
            var game = db.Games.FirstOrDefault();
            if (game != null)
            {
                db.Games.Remove(game);
                db.SaveChanges();
            }

            return Content("Done!");
        }
    }
}
