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

        public ActionResult GetGameState()
        {
            var model = new GameVM
            {
                NumOfAlive = mgr.CurrentGame.Combatants.Count(),
                NumOfDead = mgr.CurrentGame.Combatants.Count()
            };

            // model.GameActions = mgr.CurrentGame.GameActions.ToList();
            var moves = mgr.CurrentPlayer.GetPossibleMoves(mgr.CurrentGame.Tiles);

            model.Tiles = mgr.CurrentGame.Tiles.Select(x => new TileVM()
            {
                Id = x.Id,
                X = x.X,
                Y = x.Y,
                Type = (EventType)x.Type,
                GraphicName = "/Graphics/" + x.GraphicName,
                CombatantGraphicName = x.Combatant == null ? "" : "/Graphics/" + x.Combatant.GraphicName,
                IsPossibleMove = moves.Any(t => t.Id == x.Id)
            });

            model.Player = new PlayerVM()
            {
                Name = mgr.CurrentPlayer.Name,
                Health = mgr.CurrentPlayer.Health.ToString(),
                Strength = mgr.CurrentPlayer.Strength.ToString(),
                Speed = mgr.CurrentPlayer.Speed.ToString(),
                Sanity = mgr.CurrentPlayer.Sanity.ToString(),
                Perception = mgr.CurrentPlayer.Perception.ToString(),
                Threat = mgr.CurrentPlayer.Threat.ToString(),
                Survivability = mgr.CurrentPlayer.Survivability.ToString(),
                Items = mgr.CurrentPlayer.Items.Select(x => new ItemVM(x)).ToList(),
                GraphicName = mgr.CurrentPlayer.GraphicName
            };

            model.DieResult = mgr.DieResult;
            model.DieResultEnemy = mgr.DieResultEnemy;
            model.Event = new EventVM()
            {
                GraphicName = mgr.Event.GraphicName,
                Name = mgr.Event.Name,
                Description = mgr.Event.Description,
                RequiredStat = mgr.Event.RequiredStat,
                StatReward = mgr.Event.StatReward,
                TargetNumber = mgr.Event.TargetNumber,
                Type = mgr.Event.Type
            };

            model.Reward = new ItemVM(mgr.Event.ItemReward);

            mgr.RemoveResults();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GameState()
        {
            var model = new GameVM
            {
                NumOfAlive = mgr.CurrentGame.Combatants.Count(x => x.Killer == null),
                NumOfDead = mgr.CurrentGame.Combatants.Count(x => x.Killer != null)
            };

            //model.GameActions = mgr.CurrentGame.GameActions.ToList();
            var moves = mgr.CurrentPlayer.GetPossibleMoves(mgr.CurrentGame.Tiles);

            model.Tiles = mgr.CurrentGame.Tiles.Select(x => new TileVM()
            {
                Id = x.Id,
                X = x.X,
                Y = x.Y,
                Type = (EventType)x.Type,
                GraphicName = "~/Graphics/" + x.GraphicName,
                CombatantGraphicName = x.Combatant == null ? "" : "~/Graphics/" + x.Combatant.GraphicName,
                IsPossibleMove = moves.Any(t => t.Id == x.Id)
            });

            model.Player = new PlayerVM()
            {
                Name = mgr.CurrentPlayer.Name,
                Health = mgr.CurrentPlayer.Health.ToString(),
                Strength = mgr.CurrentPlayer.Strength.ToString(),
                Speed = mgr.CurrentPlayer.Speed.ToString(),
                Sanity = mgr.CurrentPlayer.Sanity.ToString(),
                Perception = mgr.CurrentPlayer.Perception.ToString(),
                Threat = mgr.CurrentPlayer.Threat.ToString(),
                Survivability = mgr.CurrentPlayer.Survivability.ToString(),
                Items = mgr.CurrentPlayer.Items.Select(x => new ItemVM(x)).ToList(),
                GraphicName = mgr.CurrentPlayer.GraphicName
            };

            

            return View(model);
        }

        //Post: Game/TryMove
        [HttpPost]
        public ActionResult TryMove(int x2, int y2)
        {
            var eventType = mgr.PlayerMove(x2, y2, mgr.CurrentPlayer);
            
            var result = new { eventType, Message = "message" };
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
