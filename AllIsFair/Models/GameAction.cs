using System;

namespace AllIsFair.Models
{

    public enum EventType
    {
        City = 1,
        Wilderness = 2,
        Expanse = 3,
        Mountain =  4
    }
    public enum Action
    {
        None,
        Move,
        Attack,
        DrawEventCard
    }

    public class GameAction
    {
        public int Id { get; set; }
        public virtual Combatant Combatant { get; set; }
        public virtual Game CurrentGame { get; set; }
        public Action Action { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public GameAction()
        {

        }


        public GameAction(Game currentGame, Action action, string message, Combatant player = null)
        {
            CurrentGame = currentGame;
            Action = action;
            Message = message;
            Date = DateTime.Now;
            Combatant = player;

        }
    }
}