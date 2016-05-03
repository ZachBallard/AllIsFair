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
using Microsoft.Owin.Security.Provider;

namespace AllIsFair.Controllers
{
    public class GamesController : Controller
    {
        private readonly AllIsFairContext db = new AllIsFairContext();
        private readonly ApplicationDbContext userdb = new ApplicationDbContext();

        private Game _game = null;

        protected Game CurrentGame
        {
            get
            {
                var currentUser = User.Identity;
                var currentUserInfo = userdb.Users.FirstOrDefault(x => x.Id == currentUser.Name);
                _game = currentUserInfo.Games.FirstOrDefault();
                if (_game == null)
                {
                    _game = new Game()
                    {
                        Tiles = GenerateMap(12, 12)
                    };
                    db.Games.Add(_game);
                    db.SaveChanges();
                }
                return _game;
            }
        }

        private ICollection<Tile> GenerateMap(int x, int y)
        {
            var Map = new List<Tile>();

            for (int i = 1; i < x; i++)
            {
                for (int j = 1; j < y; j++)
                {
                    if (i == 1 || i == 12)
                    {
                        Map.Add(new Tile() { GraphicName = @"~\Graphics\Beach.png", X = i, Y = j });
                    }
                    else
                    {
                        if (j == 1 || j == 12)
                        {
                            Map.Add(new Tile() { GraphicName = @"~\Graphics\Beach.png", X = i, Y = j });
                        }
                        else
                        {
                            Map.Add(new Tile() { GraphicName = @"~\Graphics\Plains.png", X = i, Y = j });

                        }
                    }
                }
            }

            return Map;
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
