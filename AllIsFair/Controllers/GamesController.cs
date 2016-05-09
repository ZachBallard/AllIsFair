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
    [Authorize]
    public class GamesController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            mgr = new GameManager(User.Identity.GetUserId(), new ApplicationDbContext());

            base.OnActionExecuting(filterContext);
        }

        private GameManager mgr;


        public ActionResult GameState()
        {
            //TODO
            var model = new GameVM();
            model.NumOfAlive = mgr.CurrentGame.Combatants.Count();
            model.NumOfDead = mgr.CurrentGame.Combatants.Count();
            model.Tiles = mgr.CurrentGame.Tiles.Select(x => new TileVM()
            {
                X = x.X,
                Y = x.Y,
                IsPossibleMove = false,
                Type = (EventType) x.Type,
                GraphicName = x.GraphicName
            });
            return View(model);
        }

        //Post: Game/TryMove
        [HttpPost]
        public ActionResult TryMove(int x2, int y2)
        {
            mgr.PlayerMove(x2, y2, mgr.CurrentPlayer);

            var result = new { Message = "message" };
            return Json(result);
        }



        //GET: Game/DrawEvent
        [HttpPost]
        public ActionResult DrawEvent(EventType type)
        {

            mgr.DrawEventCard(mgr.CurrentPlayer, type);

            var result = new { Message = "message" };
            return Json(result);
        }




        public ActionResult Delete()
        {
            mgr.DeleteGame();

            var result = new { Message = "Game Deleted" };
            return Json(result);
        }
    }
}
