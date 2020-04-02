using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BattleShip
{
    class Computer : Player
    {
        // Member Variables
        bool LastGuessHit = false;
        int[] firstHit;
        bool CurrentGuessSequence = false;
        List<int[]> CurrentGuessSequenceHits;
        int CurrentGuessSequenceCounter;
        int[] MoveThing;
        int linearGuesses = 1;
        int reverseLinearGuesses = 0;
        bool shipSunk = false;
        // Constructor
        public Computer(string name)
        {
            this.name = name;
            CurrentGuessSequenceHits = new List<int[]> { };
        }
        
        public override void PlayerGuess(Player guesser, Player opponent, GameBoard playerBoard)
        {
            if (CurrentGuessSequence==false)
            {
                MoveThing = RandomGuess();
            }
            else if (CurrentGuessSequence==true && CurrentGuessSequenceHits.Count==1)
            {
                MoveThing = FourSquareGuess();
                CurrentGuessSequenceCounter++;

            }
            else if (CurrentGuessSequenceHits.Count>1)
            {
                switch(CurrentGuessSequenceCounter)
                {
                    case 1:
                        VerticalUpGuess();
                        break;
                    case 2:
                        HorizontalLeftGuess();
                        break;
                    case 3:
                        VerticalDownGuess();
                        break;
                    case 4:
                        HorizontalRightGuess();
                        break;
                }
            }
            guesser.HitChecker(MoveThing, guesser, opponent);
            guesser.CheckShipSunk(opponent);
            guesser.opponentBoard.DisplayBoard(guesser);

        }
        public override void PlaceShip(Player player, Ships ship)
        {
            int randomX = Game.rng.Next(1, 21);
            System.Threading.Thread.Sleep(20);
            int randomY = Game.rng.Next(1, 21);
            int[] randomStartLocation = { randomX, randomY };
            System.Threading.Thread.Sleep(20);
            int randomOri = Game.rng.Next(0, 4);
            string randomOrientation;
            bool isValid = false;
            switch (randomOri)
            {
                case 0:
                    randomOrientation = "right";
                    break;
                case 1:
                    randomOrientation = "down";
                    break;
                case 2:
                    randomOrientation = "left";
                    break;
                case 3:
                    randomOrientation = "up";
                    break;
                default:
                    randomOrientation = "right";
                    break;
            }
            
            string shipOrientation = randomOrientation;
            isValid = Game.ValidPlacement(ship, randomStartLocation, shipOrientation);
            if (player.shipPlacements.Count > 0 && isValid == true)
            {
                isValid = Game.CheckOverlappingShips(player, ship, randomStartLocation, shipOrientation);
            }
            if (isValid)
            {
                player.ShipPlacement(player, ship, shipOrientation, randomStartLocation);
                if(Game.numberOfComputers == 2)
                {
                    player.playerBoard.DisplayBoard(player);
                }
            }
            else
            {
                this.PlaceShip(player, ship);
            }

        }
        public override void HitChecker(int[] moveCheck, Player guesser, Player opponent)
        {
            bool ishit = false;
            for (int i = 0; i < opponent.shipPlacements.Count; i++)
            {
                if (moveCheck[0] == opponent.shipPlacements[i][0] && moveCheck[1] == opponent.shipPlacements[i][1])
                {
                    if (CurrentGuessSequence == false)
                    {
                        firstHit = moveCheck;
                    }
                    LastGuessHit = true;
                    CurrentGuessSequence = true;
                    guesser.opponentBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[X]";
                    opponent.playerBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[X]";
                    totalGuesses.Add(moveCheck);
                    CurrentGuessSequenceHits.Add(moveCheck);
                    guesser.hits.Add(moveCheck);
                    int shipPosition = i;
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

                    UI.DisplayHit();
                    ishit = true;
                    break;
                }
                else
                {
                    totalGuesses.Add(moveCheck);
                    guesser.opponentBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[O]";
                    opponent.playerBoard.gameBoard[moveCheck[0], moveCheck[1]] = "[O]";
                    LastGuessHit = false;
                }

            }
            if (!ishit)
            {
                UI.DisplayMiss();
            }
        }

        public override void CheckShipSunk(Player opponent)
        {

            if (opponent.battleShip.hitsOnShip == 4 && sunkBools[0] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents battleship!");
                sunkBools[0] = !sunkBools[0];
                LastGuessHit = false;
                CurrentGuessSequence = false;
                CurrentGuessSequenceHits.Clear();
                CurrentGuessSequenceCounter = 0;
                linearGuesses = 1;
                reverseLinearGuesses = 0;
            }
            else if (opponent.aircraftCarrier.hitsOnShip == 5 && sunkBools[1] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents aircraft carrier!");
                sunkBools[1] = !sunkBools[1];
                LastGuessHit = false;
                CurrentGuessSequence = false;
                CurrentGuessSequenceHits.Clear();
                CurrentGuessSequenceCounter = 0;
                linearGuesses = 1;
                reverseLinearGuesses = 0;

            }
            else if (opponent.submarine.hitsOnShip == 3 && sunkBools[2] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents submarine");
                sunkBools[2] = !sunkBools[2];
                LastGuessHit = false;
                CurrentGuessSequence = false;
                CurrentGuessSequenceHits.Clear();
                CurrentGuessSequenceCounter = 0;
                linearGuesses = 1;
                reverseLinearGuesses = 0;

            }
            else if (opponent.destroyer.hitsOnShip == 2 && sunkBools[3] == false)
            {
                Console.WriteLine("\r\nYou sunk your opponents destroyer!");
                sunkBools[3] = !sunkBools[3];
                LastGuessHit = false;
                CurrentGuessSequence = false;
                CurrentGuessSequenceHits.Clear();
                CurrentGuessSequenceCounter = 0;
                linearGuesses = 1;
                reverseLinearGuesses = 0;

            }
        }
        // Member Methods
        //public override void PickShipsLocation(string shipName, int shipLength)
        //{

        //}
        public int[] RandomGuess()
        {

            int xAxis = Game.rng.Next(1, 21);
            int yAxis = Game.rng.Next(1, 21);
            MoveThing = new int[2] { xAxis, yAxis };
            if (totalGuesses.Count > 0)
            {
                for (int i = 0; i < totalGuesses.Count; i++)
                {
                    if (MoveThing[0] == totalGuesses[i][0] && MoveThing[1] == totalGuesses[i][1])
                    {
                        RandomGuess();
                    }
                }

            }
            return MoveThing;
        }

        public int[] FourSquareGuess()

        {
         
            switch (CurrentGuessSequenceCounter)
            {
                case 0:
                    if (firstHit[0]-1 <1)
                    {
                        CurrentGuessSequenceCounter++;
                        FourSquareGuess();
                        break;
                    }
                    MoveThing = new int[2] { firstHit[0] - 1, firstHit[1] };
                    break;
                case 1:
                    if (firstHit[1] - 1 < 1)
                    {
                        CurrentGuessSequenceCounter++;
                        FourSquareGuess();
                        break;
                    }
                    MoveThing = new int[2] { firstHit[0], firstHit[1] - 1 };
                    break;
                case 2:
                    if (firstHit[0] + 1 > 20)
                    {
                        CurrentGuessSequenceCounter++;
                        FourSquareGuess();
                        break;
                    }
                    MoveThing = new int[2] { firstHit[0] + 1, firstHit[1] };
                    break;
                case 3:
                    if (firstHit[1] + 1 > 20)
                    {
                        CurrentGuessSequenceCounter++;
                        FourSquareGuess();
                        break;
                    }
                    MoveThing = new int[2] { firstHit[0], firstHit[1] + 1 };
                    break;
                default:
                    break;

            }
            
            return MoveThing;
        }

        public int[] VerticalUpGuess()
        {
            if (LastGuessHit)
            {
                if (firstHit[0] - linearGuesses == 1)
                {
                    reverseLinearGuesses--;
                    linearGuesses = reverseLinearGuesses;
                }
                else if (linearGuesses >= 0)
                {
                    linearGuesses++;
                }
                else
                {
                    linearGuesses--;
                }
                

            }
            else
            {
                reverseLinearGuesses--;
                linearGuesses = reverseLinearGuesses;

            }
            MoveThing = new int[] { firstHit[0] - linearGuesses, firstHit[1] };
            return MoveThing;
        }
        public int[] VerticalDownGuess()
        {
            linearGuesses++;
            MoveThing = new int[] { firstHit[0] + linearGuesses, firstHit[1] };
            return MoveThing;
        }
        public int[] HorizontalLeftGuess()
        {
            if (LastGuessHit)
                if(firstHit[1] - linearGuesses ==1)
                {
                    reverseLinearGuesses--;
                    linearGuesses = reverseLinearGuesses;
                }
                else if (linearGuesses >= 0)
                {
                    linearGuesses++;
                }
                else
                {
                    linearGuesses--;
                }
            else
            {
                reverseLinearGuesses--;
                linearGuesses = reverseLinearGuesses;
            }
            MoveThing = new int[] { firstHit[0], firstHit[1] - linearGuesses };
            return MoveThing;
        }
        public int[] HorizontalRightGuess()
        {
            linearGuesses++;
            MoveThing = new int[] { firstHit[0], firstHit[1] + linearGuesses };
            return MoveThing;
        }
    }
}
