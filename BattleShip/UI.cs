using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BattleShip
{
    public static class UI
    {
        public static void DisplayHit()
        {
            Console.ReadLine();
            Console.WriteLine("\r\nIT'S A HIT!\r\n");
        }
        public static void DisplayMiss()
        {
            Console.ReadLine();
            Console.WriteLine("\r\nIt's A Miss.\r\n");
        }
        public static string ShipLocationInterpretation(Player player, Ships ship, string userStartLocation)
        {
            Regex regex = new Regex(@"[a-z]\d\d?");
            Regex regex1 = new Regex(@"[a-z]\s\d\d?");

            if (regex.IsMatch(userStartLocation))
            {
                string curStr = "";
                if (userStartLocation.Length == 2)
                {
                    curStr += userStartLocation[0];
                    curStr += " ";
                    curStr += userStartLocation[1];
                    return curStr;
                }
                else if (userStartLocation.Length == 3)
                {
                    curStr += userStartLocation[0];
                    curStr += " ";
                    curStr += userStartLocation[1];
                    curStr += userStartLocation[2];
                    return curStr;
                }
            }
            else if (regex1.IsMatch(userStartLocation))
            {
                return userStartLocation;
            }
            player.PlaceShip(player, ship);
            Console.WriteLine("\r\nYour Move Was Not Valid.");
            return "Invalid";
        }
        public static string GuessMoveInterpretation(string guessMove, Player guesser, Player opponent, GameBoard playerBoard)
        {
            Regex regex = new Regex(@"[a-z]\d\d?");
            Regex regex1 = new Regex(@"[a-z]\s\d\d?");

            if (regex.IsMatch(guessMove))
            {
                string curStr = "";
                if (guessMove.Length == 2)
                {
                    curStr += guessMove[0];
                    curStr += " ";
                    curStr += guessMove[1];
                    return curStr;
                }
                else if (guessMove.Length == 3)
                {
                    curStr += guessMove[0];
                    curStr += " ";
                    curStr += guessMove[1];
                    curStr += guessMove[2];
                    return curStr;
                }
            }
            else if (regex1.IsMatch(guessMove))
            {
                return guessMove;
            }
            guesser.PlayerGuess(guesser, opponent, playerBoard);
            Console.WriteLine("\r\nYour Move Was Not Valid.");
            return "Invalid";
        }
    }
}