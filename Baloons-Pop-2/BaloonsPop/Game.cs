using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaloonsPop
{
    public class Game
    {
        private const int ROWS_COUNT = 5;
        private const int COLS_COUNT = 10;

        private int cellsLeft = ROWS_COUNT * COLS_COUNT;
        private int userMoves = 0;
        private int clearedCells = 0;

        public string[,] gameMatrix;
        public StringBuilder userInput = new StringBuilder();
        private Statistics stats;

        public void Start()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons. Use 'top' to view the " +
                              "top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");
            cellsLeft = ROWS_COUNT * COLS_COUNT;
            stats = new Statistics();
            userMoves = 0;

            clearedCells = 0;
            gameMatrix = CreateGameMatrix(ROWS_COUNT, COLS_COUNT);
            ConsoleRenderer.PrintGameMatrix(gameMatrix);
            GameLogic(userInput);
        }

        public string[,] CreateGameMatrix(int rows, int cols)
        {
            string[,] matrix = new string[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    matrix[row, col] = RandomGenerator.GetRandomInt();
                }
            }

            return matrix;
        }

        public void GameLogic(StringBuilder userInput)
        {
            PlayGame();
            userMoves++;
            userInput.Clear();
            GameLogic(userInput);
        }

        private bool IsLegalMove(int row, int col)
        {
            if ((row < 0) || (col < 0) || (col > COLS_COUNT - 1) || (row > ROWS_COUNT - 1))
            {
                return false;
            }
            else
            {
                return (gameMatrix[row, col] != ".");
            }
        }

        private void InvalidInput()
        {
            Console.WriteLine("Invalid move or command");
            userInput.Clear();
            GameLogic(userInput);
        }

        private void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing balloon!");
            userInput.Clear();
            GameLogic(userInput);
        }

        private void Exit()
        {
            Console.WriteLine("Good Bye!");
            Thread.Sleep(1000);
            Console.WriteLine(userMoves.ToString());
            Console.WriteLine(cellsLeft.ToString());
            Environment.Exit(0);
        }

        private void Restart()
        {
            Start();
        }

        private void ReadTheIput()
        {
            if (!IsFinished())
            {
                Console.Write("Enter a row and column: ");
                userInput.Append(Console.ReadLine());
            }
            else
            {
                Console.Write("Congratulations! You popped all balloons in " + userMoves + " moves." +
                              "\r\nPlease enter your name for the top scoreboard:");
                userInput.Append(Console.ReadLine());
                stats.AddPlayer(userInput.ToString(), userMoves);
                Console.WriteLine(stats.ToString());
                userInput.Clear();
                Start();
            }
        }

        private void PlayGame()
        {
            int i = -1;
            int j = -1;

        Play:
            ReadTheIput();

            string hop = userInput.ToString();

            if (userInput.ToString() == string.Empty)
            {
                InvalidInput();
            }

            if (userInput.ToString() == "top")
            {
                Console.WriteLine(stats.ToString());
                userInput.Clear();
                goto Play;
            }

            if (userInput.ToString() == "restart")
            {
                userInput.Clear();
                Restart();
            }

            if (userInput.ToString() == "exit")
            {
                Exit();
            }

            string activeCell;
            userInput.Replace(" ", string.Empty);

            try
            {
                i = Int32.Parse(userInput.ToString()) / 10;
                j = Int32.Parse(userInput.ToString()) % 10;
            }
            catch (Exception)
            {
                InvalidInput();
            }

            if (IsLegalMove(i, j))
            {
                activeCell = gameMatrix[i, j];
                RemoveAllBaloons(i, j, activeCell);
            }
            else
            {
                InvalidMove();
            }

            ClearEmptyCells();
            ConsoleRenderer.PrintGameMatrix(gameMatrix);
        }

        private void RemoveAllBaloons(int row, int col, string activeCell)
        {
            if ((row >= 0) && (row <= 4) && (col <= 9) && (col >= 0) && (gameMatrix[row, col] == activeCell))
            {
                gameMatrix[row, col] = ".";
                clearedCells++;
                //Up
                RemoveAllBaloons(row - 1, col, activeCell);
                //Down
                RemoveAllBaloons(row + 1, col, activeCell);
                //Left
                RemoveAllBaloons(row, col + 1, activeCell);
                //Right
                RemoveAllBaloons(row, col - 1, activeCell);
            }
            else
            {
                cellsLeft -= clearedCells;
                clearedCells = 0;

                return;
            }
        }

        private void ClearEmptyCells()
        {
            int row;
            int col;
            Queue<string> collumnFallDown = new Queue<string>();

            for (col = COLS_COUNT - 1; col >= 0; col--)
            {
                for (row = ROWS_COUNT - 1; row >= 0; row--)
                {
                    if (gameMatrix[row, col] != ".")
                    {
                        collumnFallDown.Enqueue(gameMatrix[row, col]);
                        gameMatrix[row, col] = ".";
                    }
                }

                row = ROWS_COUNT - 1;

                while (collumnFallDown.Count > 0)
                {
                    gameMatrix[row, col] = collumnFallDown.Dequeue();
                    row--;
                }

                collumnFallDown.Clear();
            }
        }

        private bool IsFinished()
        {
            return (cellsLeft == 0);
        }
    }
}