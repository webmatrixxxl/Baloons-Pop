using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BaloonsPop
{
    public class Game
    {
        private const int ROWS_COUNT = 5;
        private const int COLS_COUNT = 10;
        private int cellsLeft;
        private int userMoves;
        private string[,] gameMatrix;
        private Statistics stats;

        public void Start()
        {
            Reset();
            Run();
        }

        private string[,] CreateGameMatrix(int rows, int cols)
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
            Console.WriteLine(userMoves);
            Console.WriteLine(cellsLeft);
            Environment.Exit(0);
        }

        private void Reset()
        {
            cellsLeft = ROWS_COUNT * COLS_COUNT;
            stats = new Statistics();
            userMoves = 0;
            gameMatrix = CreateGameMatrix(ROWS_COUNT, COLS_COUNT);
            ConsoleRenderer.PrintGreetingMessage();
            ConsoleRenderer.PrintGameMatrix(gameMatrix);
        }

        private Cell ParseInputString(string input)
        {
            Cell cell = new Cell();
            string[] coordinates = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            cell.Row = int.Parse(coordinates[0]);
            cell.Col = int.Parse(coordinates[1]);

            return cell;
        }

        private void EndGame()
        {
            Console.Write("Congratulations! You popped all balloons in " + userMoves + " moves." +
                              "\r\nPlease enter your name for the top scoreboard:");
            string input = Console.ReadLine();
            stats.AddPlayer(input, userMoves);
            Console.WriteLine(stats.ToString());
            Start();
        }

        private void NextMove(string userInputString)
        {
            Cell cell = new Cell();
            try
            {
                cell = ParseInputString(userInputString);
            }
            catch (IndexOutOfRangeException)
            {
                InvalidInput();
                return;
            }
            catch (FormatException)
            {
                InvalidInput();
                return;
            }

            if (cell.Row < 0 || cell.Row >= ROWS_COUNT ||
                cell.Col < 0 || cell.Col >= COLS_COUNT)
            {
                InvalidMove();
                return;
            }

            string activeCell = gameMatrix[cell.Row, cell.Col];

            if (activeCell == ".")
            {
                InvalidMove();
                return;
            }

            RemoveAllBaloons(cell.Row, cell.Col, activeCell);
            ClearEmptyCells();
            ConsoleRenderer.PrintGameMatrix(gameMatrix);

            userMoves++;
        }

        private void Run()
        {
            while (cellsLeft != 0)
            {
                Console.Write("Enter a row and column: ");

                string userInputString = Console.ReadLine();
                switch (userInputString)
                {
                    case "":
                        InvalidInput();
                        break;

                    case "top":
                        Console.WriteLine(stats.ToString());
                        break;

                    case "restart":
                        Reset();
                        break;

                    case "exit":
                        Exit();
                        break;

                    default:
                        NextMove(userInputString);
                        break;
                }
            }

            EndGame();
        }

        private void RemoveAllBaloons(int row, int col, string color)
        {
            bool isInMatrix = (row >= 0) && (row <= ROWS_COUNT - 1) && (col <= COLS_COUNT - 1 ) && (col >= 0);
            if (isInMatrix && gameMatrix[row, col] == color)
            {
                gameMatrix[row, col] = ".";
                cellsLeft--;
                //Up
                RemoveAllBaloons(row - 1, col, color);
                //Down
                RemoveAllBaloons(row + 1, col, color);
                //Left
                RemoveAllBaloons(row, col + 1, color);
                //Right
                RemoveAllBaloons(row, col - 1, color);
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