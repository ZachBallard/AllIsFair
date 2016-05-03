using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AllIsFair.Models;

namespace AllIsFair.Controllers
{
    public class GameController : Controller
    {
        public class GamesController : Controller
        {
            private readonly AllIsFairContext db = new AllIsFairContext();

            private Game _game = null;

            protected Game CurrentGame
            {
                get
                {
                    _game = db.Games.FirstOrDefault();
                    if (_game == null)
                    {
                        _game = new Game();
                        db.Games.Add(_game);
                        db.SaveChanges();
                    }
                    return _game;
                }
            }

            // GET: Game
            public ActionResult GameState()
            {
                return View(db.Games.ToList());
            }

            // GET: Game/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Game game = db.Games.Find(id);
                if (game == null)
                {
                    return HttpNotFound();
                }
                return View(game);
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
}
