using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
                var user = db.Users.Include(x=>x.Game).FirstOrDefault(x => x.Id == currentUserId);

                if (user == null) return null;

                _game = user.Game;
                if (_game != null) return _game;

                _game = new Game()
                {
                    Tiles = GenerateMap(12, 12),
                    User = user

                };
                user.Game = _game;
                db.SaveChanges();
                return _game;
            }
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
            var model = new GameInfo
            {
                Id = CurrentGame.Id,
                AliveCombatants = CurrentGame.Combatants.Count(x => x.Killer == null),
                Combatants = CurrentGame.Combatants,
                Tiles = CurrentGame.Tiles,
                Player = CurrentGame.Combatants.Where(x => x.IsPlayer == true).ToList()
            };

            return model;
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
