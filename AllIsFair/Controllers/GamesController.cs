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

        private TurnManager tmgr;

        public ActionResult GameState()
        {
            //while (tmgr.CurrentTurn != mgr.CurrentPlayer)
           // {
                //do other players turns

           //     var toLast = tmgr.TurnOrder.First();
           //     tmgr.TurnOrder.Add(toLast);
            //}

            var model = new GameVM
            {
                NumOfAlive = mgr.CurrentGame.Combatants.Count(),
                NumOfDead = mgr.CurrentGame.Combatants.Count()
            };

            model.GameActions = mgr.CurrentGame.GameActions.ToList();
            model.PossibleMoves = mgr.CurrentPlayer.GetPossibleMoves(mgr.CurrentGame.Tiles);

            model.Tiles = mgr.CurrentGame.Tiles.Select(x => new TileVM()
            {
                Id = x.Id,
                X = x.X,
                Y = x.Y,
                Type = (EventType) x.Type,
                GraphicName = x.GraphicName,
                CombatantGraphicName = x.Combatant?.GraphicName,
            });

            model.Player = new PlayerVM()
            {
                Name = mgr.CurrentPlayer.Name,
                Strength = mgr.CurrentPlayer.Strength.ToString(),
                Speed = mgr.CurrentPlayer.Speed.ToString(),
                Sanity = mgr.CurrentPlayer.Sanity.ToString(),
                Perception = mgr.CurrentPlayer.Perception.ToString(),
                Threat = mgr.CurrentPlayer.Threat.ToString(),
                Survivability = mgr.CurrentPlayer.Survivability.ToString(),
                Items = mgr.CurrentPlayer.Items.ToList(),
                GraphicName = mgr.CurrentPlayer.GraphicName
            };
            model.Combatants = mgr.CurrentGame.Combatants;

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
