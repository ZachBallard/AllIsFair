using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using AllIsFair.Models;
using Microsoft.AspNet.Identity;

namespace AllIsFair.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private GameManager mgr;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            mgr = new GameManager(User.Identity.GetUserId(), new ApplicationDbContext());

            base.OnActionExecuting(filterContext);
        }

        public ActionResult GetGameState(int? turnNumber, int? turnOrder)
        {
            var results = GetResult(turnNumber ?? mgr.CurrentGame.CurrentTurnNumber, turnOrder ?? mgr.CurrentGame.CurrentTurnOrder);

            var playerColor = "possiblemoveblue";
            if (mgr.CurrentPlayer.TurnOrder == 2)
            {
                playerColor = "possiblemovered";
            }

            var moves = mgr.CurrentPlayer.GetPossibleMoves(mgr.CurrentGame.Tiles);

            var model = new GameVM
            {
                NumOfAlive = mgr.CurrentGame.Combatants.Count(),
                NumOfDead = mgr.CurrentGame.Combatants.Count(x => x.Killer != null),
                Result = results,
                PlayerColor = playerColor,
                Tiles = mgr.CurrentGame.Tiles.Select(x => new TileVM
                {
                    Id = x.Id,
                    X = x.X,
                    Y = x.Y,
                    Type = x.Type,
                    GraphicName = "/Graphics/" + x.GraphicName,
                    CombatantGraphicName = x.Combatant == null ? "" : "/Graphics/" + x.Combatant.GraphicName,
                    IsPossibleMove = moves.Any(t => t.Id == x.Id),
                }),
                Player = new PlayerVM
                {
                    Name = mgr.CurrentPlayer.Name,
                    Health = mgr.CurrentPlayer.Health.ToString(),
                    Strength = mgr.CurrentPlayer.Strength.ToString(),
                    Speed = mgr.CurrentPlayer.Speed.ToString(),
                    Sanity = mgr.CurrentPlayer.Sanity.ToString(),
                    Perception = mgr.CurrentPlayer.Perception.ToString(),
                    Threat = mgr.CurrentPlayer.Threat.ToString(),
                    Survivability = mgr.CurrentPlayer.Survivability.ToString(),
                    Items = mgr.CurrentPlayer.Items.Where(i => !i.IsWeapon).Select(x => new ItemVM(x)).ToList(),
                    Weapons = mgr.CurrentPlayer.Items.Where(i => i.IsWeapon).Select(x => new ItemVM(x)).ToList(),
                    GraphicName = mgr.CurrentPlayer.GraphicName
                },
                GameActions =
                    mgr.CurrentGame.GameActions.OrderByDescending(x => x.Date).Take(5).Select(x => new GameActionVM
                    {
                        Id = x.Id,
                        Action = x.Action.ToString(),
                        Date = x.Date.ToString(),
                        Message = x.Message,
                        PlayerName = x.Combatant?.Name
                    }).ToList()
            };

            var killAction = mgr.CurrentGame.GameActions.FirstOrDefault(x => x.Action == Action.Kill);
            if (killAction != null)
            {
                model.GameOverInfo = new GameOverVM {Killer = killAction.Combatant.Name};

            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        //Post: Game/TryMove
        [HttpPost]
        public ActionResult TryMove(int x2, int y2)
        {
            var result = mgr.PlayerMove(x2, y2, mgr.CurrentPlayer);

            return GetGameState(result.TurnNumber, result.TurnOrder);
        }


        public ActionResult Delete()
        {
            mgr.DeleteGame();

            return RedirectToAction("Index", "Games");
        }


        public ResultVM GetResult(int turnNumber, int turnOrder)
        {
            var result = new ResultVM();
            result.TurnNumber = mgr.CurrentGame.CurrentTurnNumber;

            var playerResults =
                mgr.CurrentGame.Combatants.FirstOrDefault(x=>x.TurnOrder == turnOrder).Results.FirstOrDefault(x => x.TurnNumber == turnNumber && x.TurnOrder == turnOrder );
            var enemyResults =
                mgr.CurrentGame.Combatants.FirstOrDefault(x => x.TurnOrder != turnOrder)
                    .Results.FirstOrDefault(x => x.TurnNumber == turnNumber && x.TurnOrder == turnOrder);

            if (playerResults != null)
            {
                if (playerResults.Event != null)
                {
                    result.Event = new EventVM
                    {
                        Name = playerResults.Event.Name,
                        GraphicName =
                            playerResults.Event.GraphicName == null
                                ? ""
                                : "/Graphics/" + playerResults.Event.GraphicName,
                        RequiredStat = playerResults.Event.RequiredStat.ToString(),
                        TargetNumber = playerResults.Event.TargetNumber,
                        Type = playerResults.Event.Type,
                        StatReward = playerResults.Event.StatReward,
                        Description = playerResults.Event.Description
                    };

                    if (playerResults.Event.ItemReward != null)
                        result.ItemReward = new ItemVM(playerResults.Event.ItemReward);
                }

                result.PlayerName = playerResults.Combatant.Name;
                result.Rolls = playerResults.Rolls.ConvertStringToNumberList();
                result.RollsSum = playerResults.RollsSum;
                result.DieResultGraphics = result.Rolls.GetDieGraphics();
                result.StatReward = playerResults.StatReward;
            }

            if (enemyResults == null) return result;

            
            result.IsAttack = true;
            result.EnemyRolls = enemyResults.Rolls.ConvertStringToNumberList();
            result.EnemyRollsSum = enemyResults.RollsSum;
            result.DieResultEnemyGraphics = result.EnemyRolls.GetDieGraphics();
            result.Healthloss = enemyResults.Healthloss;

            return result;
        }

        public ActionResult GameOver(string deadDude, string killer)
        {
            var names = new List<string>() {deadDude, killer};
            return View(names);
        }
    }
}