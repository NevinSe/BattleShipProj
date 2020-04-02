using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BattleShip
{
    public class Player
    {
        // Member Variables
        public string name { get; set; }
        public GameBoard playerBoard;
        public GameBoard opponentBoard;
        public Ships battleShip;
        public Ships destroyer;
        public Ships aircraftCarrier;
        public Ships submarine;
        public bool[] sunkBools { get; set; }
        public List<int[]> shipPlacements;
        public List<string> xAxis = new List<string>
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t"
        };
        public List<int[]> totalGuesses;
        public List<int[]> hits;

        // Constructor
        public Player()
        {
            playerBoard = new GameBoard();
            opponentBoard = new GameBoard();
            battleShip = new BattleShip();
            destroyer = new Destroyer();
            aircraftCarrier = new AircraftCarrier();
            submarine = new Submarine();
            totalGuesses = new List<int[]> { };
            shipPlacements = new List<int[]> { };
            hits = new List<int[]> { };
            sunkBools = new bool[4] { false, false, false, false };
        }
        // Member Methods
        public virtual void ShipPlacement(Player guesser, Ships ship, string shipOrientation, int[] startLocation)
        {
            playerBoard.gameBoard[startLocation[0], startLocation[1]] = ship.abbreviation;
            switch (shipOrientation)
            {
                case "left":
                    for (int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0], startLocation[1] - i };
                        playerBoard.gameBoard[nextLocation[0], nextLocation[1]] = ship.abbreviation;
                        guesser.shipPlacements.Add(nextLocation);
                    }
                    break;
                case "right":
                    for (
                        int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0], startLocation[1] + i };
                        playerBoard.gameBoard[nextLocation[0], nextLocation[1]] = ship.abbreviation;
                        guesser.shipPlacements.Add(nextLocation);
                    }
                    break;
                case "up":
                    for (int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0] - i, startLocation[1] };
                        playerBoard.gameBoard[nextLocation[0], nextLocation[1]] = ship.abbreviation;
                        guesser.shipPlacements.Add(nextLocation);
                    }
                    break;
                case "down":
                    for (int i = 0; i < ship.length; i++)
                    {
                        int[] nextLocation = new int[] { startLocation[0] + i, startLocation[1] };
                        playerBoard.gameBoard[nextLocation[0], nextLocation[1]] = ship.abbreviation;
                        guesser.shipPlacements.Add(nextLocation);
                    }
                    break;
                default:
                    break;
            }
        }
        public virtual void PlaceShip(Player player, Ships ship)
        {
            bool isValid = false;
            Console.WriteLine($"\r\n{player.name}, Enter Starting Location Of {ship.name}");
            string shipPlacement = Console.ReadLine().ToLower().Trim();
            shipPlacement = UI.ShipLocationInterpretation(player, ship, shipPlacement);
            Console.WriteLine("\r\nEnter Its Orientation: \r\n('Up', 'Down', 'Left', 'Right')");
            string shipOrientation = Console.ReadLine();
            isValid = Game.ValidPlacement(ship, player.MoveInterpritation(shipPlacement), shipOrientation);
            if (player.shipPlacements.Count > 0 && isValid == true)
            {
                isValid = Game.CheckOverlappingShips(player, ship, player.MoveInterpritation(shipPlacement), shipOrientation);
            }
            if (isValid)
            {
                player.ShipPlacement(player, ship, shipOrientation, player.MoveInterpritation(shipPlacement));
                player.playerBoard.DisplayBoard(player);
            }
            else
            {
                PlaceShip(player, ship);
            }
        }

        public int[] MoveInterpritation(string guessLocation)
        {
            int moveX = 0;
            int moveY;
            try
            {
                try
                {
                    string[] move = guessLocation.Trim().ToLower().Split(' ');
                    moveY = int.Parse(move[1]);
                    for (int i = 0; i < xAxis.Count; i++)
                    {
                        if (move[0] == xAxis[i])
                        {
                            moveX = i + 1;
                        }
                    }
                    int[] moves = new int[] { moveY, moveX };
                    return moves;
                }
                catch
                {

                    char[] move = guessLocation.ToCharArray();
                    if (move.Length > 2)
                    {
                        string char1 = move[1].ToString();
                        string char2 = move[2].ToString();
                        string joinedChars = String.Join("", char1, char2);
                        moveY = int.Parse(joinedChars);
                        for (int i = 0; i < xAxis.Count; i++)
                        {
                            if (move[0].ToString() == xAxis[i])
                            {
                                moveX = i + 1;
                            }
                        }
                    }
                    else
                    {
                        moveY = int.Parse(move[1].ToString());
                        for (int i = 0; i < xAxis.Count; i++)
                        {
                            if (move[0].ToString() == xAxis[i])
                            {
                                moveX = i + 1;
                            }
                        }
                    }
                    int[] moves = new int[] { moveY, moveX };
                    return moves;
                }
            }
            catch
            {
                Console.WriteLine("Enter A Valid Ship Location");
                string newLocation = Console.ReadLine();
                return MoveInterpritation(newLocation);
            }

        }

        public virtual void PlayerGuess(Player guesser, Player opponent, GameBoard playerBoard)
        {
            Console.WriteLine($"\r\n{guesser.name}, Enter A Guess Location:");
            string guessMove = Console.ReadLine().ToLower().Trim();
            guessMove = UI.GuessMoveInterpretation(guessMove, guesser, opponent, playerBoard);
            int[] MoveThing = MoveInterpritation(guessMove);
            for (int i = 0; i < totalGuesses.Count; i++)
            {
                if (totalGuesses[i] == MoveThing)
                {
                    Console.WriteLine("\r\nPlease enter a valid choice");
                    PlayerGuess(guesser, opponent, playerBoard);
                }
                else
                {
                    totalGuesses.Add(MoveThing);

                }
            }
            guesser.HitChecker(MoveThing, guesser, opponent);
            guesser.CheckShipSunk(opponent);
            guesser.opponentBoard.DisplayBoard(guesser);
        }

        public virtual void HitChecker(int[] moveCheck, Player guesser, Player opponent)
        {
            bool ishit = false;
            for (int i = 0; i < opponent.shipPlacements.Count; i++)
            {
                if (moveCheck[0] == opponent.shipPlacements[i][0] && moveCheck[1] == opponent.shipPlacements[i][1])
                {
                    guesser.hits.Add(moveCheck);
                    Console.ForegroundColor = ConsoleColor.Red;
                    guesser.opponentBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[X]";
                    opponent.playerBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[X]";
                    Console.ResetColor();
                    int shipPosition = i;
                    AddHitOnShip(opponent, shipPosition);
                    UI.DisplayHit();
                    ishit = true;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    guesser.opponentBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[O]";
                    opponent.playerBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[O]";
                    Console.ResetColor();
                }

            }
            if (!ishit)
            {
                UI.DisplayMiss();
            }
        }
        public virtual void AddHitOnShip(Player opponent, int shipPosition)
        {
            if (0 <= shipPosition && shipPosition <= 3)
            {
                opponent.battleShip.hitsOnShip++;
            }
            else if (4 <= shipPosition && shipPosition <= 8)
            {
                opponent.aircraftCarrier.hitsOnShip++;
            }
            else if (9 <= shipPosition && shipPosition <= 11)
            {
                opponent.submarine.hitsOnShip++;
            }
            else
            {
                opponent.destroyer.hitsOnShip++;
            }
        }
        public virtual void CheckShipSunk(Player opponent)
        {
            if (opponent.battleShip.hitsOnShip == 4 && sunkBools[0] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents battleship!");
                sunkBools[0] = !sunkBools[0];
            }
            else if (opponent.aircraftCarrier.hitsOnShip == 5 && sunkBools[1] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents aircraft carrier!");
                sunkBools[1] = !sunkBools[1];
            }
            else if (opponent.submarine.hitsOnShip == 3 && sunkBools[2] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents submarine");
                sunkBools[2] = !sunkBools[2];
            }
            else if (opponent.destroyer.hitsOnShip == 2 && sunkBools[3] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents destroyer!");
                sunkBools[3] = !sunkBools[3];
            }


        }
    }
}