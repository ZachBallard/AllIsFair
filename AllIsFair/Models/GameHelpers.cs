using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace AllIsFair.Models
{
    public static class GameHelpers
    {
        public static List<int> RollDie(int number)
        {
            var allRolls = new List<int>();
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                allRolls.Add(random.Next(1, 7));
            }

            return allRolls;
        }

        public static int Distance(this Tile tile1, Tile tile2 )
        {
            var result = Convert.ToInt32(Math.Sqrt(Math.Pow((tile2.X - tile1.X), 2) + Math.Pow((tile2.Y - tile1.Y), 2)));
            return result;
        }

        public static bool CanMove(this Combatant c, Tile tile)
        {
            var distance = c.Tile.Distance(tile);
            return distance < c.Speed + 0.5;
        }

        public static bool CanAttack(this Combatant attacker, Combatant defender)
        {
            //attack logic
            return true;
        }

        public static Tile GetTile(this IEnumerable<Tile> tiles, int x, int y)
        {
            return tiles.FirstOrDefault(t => t.X == x && t.Y == y);
        }

        public static List<Tile> GetPossibleMoves(this Combatant c, IEnumerable<Tile> tiles )
        {
            //determine viable moves for player
            var possibleMoves = new List<Tile>();

            foreach (var tile in tiles)
            {
                var distance = c.Tile.Distance(tile);

                if (distance < c.Speed + 0.5)
                {
                    possibleMoves.Add(tile);
                }
            }

            return possibleMoves;
        } 
    }
}