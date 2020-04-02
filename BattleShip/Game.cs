using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Game
    {
        // Member Variables
        public Player player1;
        public Player player2;
        public GameBoard player1GameBoard;
        public GameBoard player2GameBoard;
        public bool gameOver;
        public static Random rng = new Random();
        public static int numberOfComputers;
        // Constructor
        public Game()
        {
           
        }
        // Member Methods

        public void RunGame()
        {
            bool correctMode;
            CreateGameBoard();

            Console.WriteLine("Welcome To: BattleShip");

            do
            {
                Console.WriteLine("\r\n" + "Press '1' for Single-Player, '2' for Multi-Player, or '3' for A Show Down Between Computers.");
                string userInput = Console.ReadLine();
                correctMode = (userInput == "1" || userInput == "2" || userInput == "3");
                switch (userInput)
                {
                    case "1":
                        Console.WriteLine("\r\n" + "Please Enter Your Name:");
                        player1 = new Human(Console.ReadLine());
                        player2 = new Computer("Computer");
                        numberOfComputers = 1;
                        this.MainGame();
                        break;
                    case "2":
                        Console.WriteLine("\r\n" + "Player 1, Please Enter Your Name:");
                        player1 = new Human(Console.ReadLine());
                        Console.WriteLine("\r\n" + "Player 2, Please Enter Your Name:");
                        player2 = new Human(Console.ReadLine());
                        numberOfComputers = 0;
                        this.MainGame();
                        break;
                    case "3":
                        player1 = new Computer("Computer1");
                        player2 = new Computer("Computer2");
                        numberOfComputers = 2;
                        this.MainGame();
                        break;
                    default:
                        Console.WriteLine("\r\n" + "Please Try Again With The Described Game Modes");
                        break;
                }
            }
            while (correctMode == false);
        }

        public void MainGame()
        {
            player1.PlaceShip(player1, player1.battleShip);
            player1.PlaceShip(player1, player1.aircraftCarrier);
            player1.PlaceShip(player1, player1.submarine);
            player1.PlaceShip(player1, player1.destroyer);
            ClearForNumberOfComputers();
            if (numberOfComputers == 1) Console.WriteLine("The Computer Picked");
            player2.PlaceShip(player2, player2.battleShip);
            player2.PlaceShip(player2, player2.aircraftCarrier);
            player2.PlaceShip(player2, player2.submarine);
            player2.PlaceShip(player2, player2.destroyer);
            ClearForNumberOfComputers();
            do
            {
                player1.PlayerGuess(player1, player2, player1.playerBoard);
                gameOver = IsGameOver(player1, player2);
                if(gameOver)
                {
                    break;
                }
                player2.PlayerGuess(player2, player1, player2.playerBoard);
                gameOver = IsGameOver(player1, player2);
                if(gameOver)
                {
                    break;
                }
            }
            while (!gameOver);
            Console.ReadLine();
        }
        public void ClearForNumberOfComputers()
        {
            if(numberOfComputers == 0)
            {
                Console.ReadLine();
                Console.Clear();
            }
            else if(numberOfComputers == 1 || numberOfComputers == 2)
            {
                Console.ReadLine();
            }

        }
        public void CreateGameBoard()
        {
            GameBoard gameBoard1 = new GameBoard();
        }

        public static bool ValidPlacement(Ships ship, int[] startLocation, string shipOrientation)
        {
            switch (shipOrientation)
            {
                case "left":
                    if (startLocation[1] - ship.length <= 1)
                    {
                        return false;
                    }
                    else return true;
                case "right":
                    if (startLocation[1] + ship.length >= 21)
                    {
                        return false;
                    }
                    else return true;
                case "up":
                    if (startLocation[0] - ship.length <= 1)
                    {
                        return false;
                    }
                    else return true;
                case "down":
                    if (startLocation[0] + ship.length >= 21)
                    {
                        return false;
                    }
                    else return true;
                default:
                    return false;
            }
        }
        static public bool CheckOverlappingShips(Player player, Ships ship, int[] startLocation, string shipOrientation)
        {
            List<int[]> currentPlacement = new List<int[]> { };
            switch (shipOrientation)
            {
                case "left":
                    for (int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0], startLocation[1] - i };
                        currentPlacement.Add(nextLocation);
                    }
                    break;
                case "right":
                    for (
                        int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0], startLocation[1] + i };
                        currentPlacement.Add(nextLocation);
                    }
                    break;
                case "up":
                    for (int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0] - i, startLocation[1] };
                        currentPlacement.Add(nextLocation);
                    }
                    break;
                case "down":
                    for (int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0] + i, startLocation[1] };
                        currentPlacement.Add(nextLocation);
                    }
                    break;
            }
            for(int i= 0; i<currentPlacement.Count;i++)
            {
                for (int j = 0; j<player.shipPlacements.Count; j++)
                {
                    if (currentPlacement[i][0] == player.shipPlacements[j][0] && currentPlacement[i][1] == player.shipPlacements[j][1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool IsGameOver(Player player1, Player player2)
        {
            if(player1.sunkBools[0] == true && player1.sunkBools[1] == true && player1.sunkBools[2] == true && player1.sunkBools[3] == true )
            {
                Console.WriteLine($"\r\n{player1.name} Won!");
                return true;
            }
            if (player2.sunkBools[0] == true && player2.sunkBools[1] == true && player2.sunkBools[2] == true && player2.sunkBools[3] == true)
            {
                Console.WriteLine($"\r\n{player2.name} Won!");
                return true;
            }
            return false;
        }
    }
}
