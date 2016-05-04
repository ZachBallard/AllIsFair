using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
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
                    Combatants = GenerateCombatants(2),
                    User = user

                };
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
                db.SaveChanges();
                return _game;
            }
        }

        private ICollection<Combatant> GenerateCombatants(int num)
        {
            var combatants = new List<Combatant>();

            var player = new Combatant()
            {
                Name = "Player",
                X = 12,
                Y = 12,
                IsPlayer = true,
                GraphicName = "Player.png",
                Strength = 4,
                Speed = 2,
                Sanity = 5
            };

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
                    Sanity = 5
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
            return View(GetGame());
        }

        private GameInfo GetGame()
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

            var model = new GameInfo
            {
                Id = CurrentGame.Id,
                AliveCombatants = CurrentGame.Combatants.Count(x => x.Killer == null),
                Combatants = CurrentGame.Combatants.ToList(),
                Tiles = CurrentGame.Tiles.ToList(),
                Player = player,
                PossibleMoves = possibleMoves
            };

            return model;
        }

        //Post: Game/TryMove
        [HttpPost]
        public ActionResult TryMove(int x2, int y2)
        {
            var player = CurrentGame.Combatants.FirstOrDefault(p => p.IsPlayer);
            var whereFrom = CurrentGame.Tiles.FirstOrDefault(t => t.Combatant == player);
            var whereTo = CurrentGame.Tiles.FirstOrDefault(t => t.X == x2 && t.Y == y2);

            if (whereTo.Combatant != null)
            {
                return HttpNotFound();
            }

            var x1 = player.X;
            var y1 = player.Y;
            var moveDifference = CalculateDistance(x1, y1, x2, y2);

            if (moveDifference > player.Speed + 0.5)
            {
                return HttpNotFound();
            }

            whereFrom.Combatant = null;
            whereTo.Combatant = player;

            player.X = x2;
            player.Y = y2;

            db.SaveChanges();

            var result = new { x1, y1, x2, y2 };
            return Json(result);
        }

        private int CalculateDistance(int x1, int y1, int x2, int y2)
        {
            int result = Convert.ToInt32(Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
            return result;
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
