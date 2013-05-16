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

        private string[,] gameMatrix;
        private Statistics stats;

        public void Start()
        {
            Reset();
            Run();
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

        private void InvalidInput()
        {
            Console.WriteLine("Invalid move or command");
        }

        private void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing balloon!");
        }

        private void Exit()
        {
            Console.WriteLine("Good Bye!");
            Thread.Sleep(1000);
            Console.WriteLine(userMoves.ToString());
            Console.WriteLine(cellsLeft.ToString());
            Environment.Exit(0);
        }

        private void Reset()
        {
            cellsLeft = ROWS_COUNT * COLS_COUNT;
            stats = new Statistics();
            userMoves = 0;
            clearedCells = 0;
            gameMatrix = CreateGameMatrix(ROWS_COUNT, COLS_COUNT);
            //userInput.Clear();
            ConsoleRenderer.PrintGreetingMessage();
            ConsoleRenderer.PrintGameMatrix(gameMatrix);
        }

        private string ReadInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            return input;
        }

        private Cell ParseInputString(string input)
        {
            Cell cell = new Cell();
            string[] coordinates = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            cell.Row = int.Parse(coordinates[0]);
            cell.Col = int.Parse(coordinates[1]);

            return cell;
        }

        private void Run()
        {
            while (true)
            {
                string userInputString;

                if (cellsLeft != 0)
                {
                    userInputString = ReadInput("Enter a row and column: ");
                }
                else
                {
                    userInputString = ReadInput("Congratulations! You popped all balloons in " + userMoves + " moves." +
                              "\r\nPlease enter your name for the top scoreboard:");
                    stats.AddPlayer(userInputString, userMoves);
                    Console.WriteLine(stats.ToString());
                    Start();
                }

                if (userInputString == string.Empty)
                {
                    InvalidInput();
                }
                else if (userInputString == "top")
                {
                    Console.WriteLine(stats.ToString());
                }
                else if (userInputString == "restart")
                {
                    Reset();
                }
                else if (userInputString == "exit")
                {
                    Exit();
                }
                else
                {
                    Cell cell = new Cell();
                    try
                    {
                        cell = ParseInputString(userInputString);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        InvalidInput();
                        continue;
                    }
                    catch (FormatException)
                    {
                        InvalidInput();
                        continue;
                    }

                    if (cell.Row < 0 || cell.Row > ROWS_COUNT - 1 ||
                        cell.Col < 0 || cell.Col > COLS_COUNT - 1)
                    {
                        InvalidMove();
                        continue;
                    }

                    string activeCell = gameMatrix[cell.Row, cell.Col];

                    if (activeCell == ".")
                    {
                        InvalidMove();
                        continue;
                    }

                    RemoveAllBaloons(cell.Row, cell.Col, activeCell);
                    userMoves++;

                    ClearEmptyCells();
                    ConsoleRenderer.PrintGameMatrix(gameMatrix);
                }
            }
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
    }
}