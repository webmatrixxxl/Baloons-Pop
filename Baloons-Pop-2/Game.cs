﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BalloonsPops
{
    public class Game
    {
        private const int ROWS_COUNT = 5;
        private const int COLS_COUNT = 10;

        private static int cellsLeft = ROWS_COUNT * COLS_COUNT;
        private static int userMoves = 0;
        private static int clearedCells = 0;

        public static string[,] gameMatrix;
        public static StringBuilder userInput = new StringBuilder();
        private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();

        public static void Start()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons. Use 'top' to view the " +
                              "top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");
            cellsLeft = ROWS_COUNT * COLS_COUNT;
            userMoves = 0;

            clearedCells = 0;
            gameMatrix = CreateTable(ROWS_COUNT, COLS_COUNT);
            PrintTable();
            GameLogic(userInput);
        }

        public static string[,] CreateTable(int rows, int cols)
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

        public static string GameMatrixToString(string[,] matrix)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("    0 1 2 3 4 5 6 7 8 9");
            builder.AppendLine("   ---------------------");

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                builder.AppendFormat("{0} | ", row);

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    builder.AppendFormat("{0} ", matrix[row, col]);
                }

                builder.AppendLine("| ");
            }

            builder.AppendLine("   ---------------------");
            return builder.ToString();
        }

        public static void PrintTable()
        {
            Console.WriteLine(GameMatrixToString(gameMatrix));
        }

        public static void GameLogic(StringBuilder userInput)
        {
            PlayGame();
            userMoves++;
            userInput.Clear();
            GameLogic(userInput);
        }

        private static bool IsLegalMove(int i, int j)
        {
            if ((i < 0) || (j < 0) || (j > COLS_COUNT - 1) || (i > ROWS_COUNT - 1))
                return false;
            else
                return (gameMatrix[i, j] != ".");
        }

        private static void InvalidInput()
        {
            Console.WriteLine("Invalid move or command");
            userInput.Clear();
            GameLogic(userInput);
        }

        private static void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing balloon!");
            userInput.Clear();
            GameLogic(userInput);
        }

        private static void ShowStatistics()
        {
            PrintTheScoreBoard();
        }

        private static void Exit()
        {
            Console.WriteLine("Good Bye!");
            Thread.Sleep(1000);
            Console.WriteLine(userMoves.ToString());
            Console.WriteLine(cellsLeft.ToString());
            Environment.Exit(0);
        }

        private static void Restart()
        {
            Start();
        }

        private static void ReadTheIput()
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
                statistics.Add(userMoves, userInput.ToString());
                PrintTheScoreBoard();
                userInput.Clear();
                Start();
            }
        }

        private static void PrintTheScoreBoard()
        {
            int p = 0;

            Console.WriteLine("Scoreboard:");
            foreach (KeyValuePair<int, string> s in statistics)
            {
                if (p == 4)
                    break;
                else
                {
                    p++;
                    Console.WriteLine("{0}. {1} --> {2} moves", p, s.Value, s.Key);
                }
            }
        }

        private static void PlayGame()
        {
            int i = -1;
            int j = -1;

        Play:
            ReadTheIput();

            string hop = userInput.ToString();

            if (userInput.ToString() == "")
                InvalidInput();
            if (userInput.ToString() == "top")
            {
                ShowStatistics();
                userInput.Clear();
                goto Play;
            }
            if (userInput.ToString() == "restart")
            {
                userInput.Clear();
                Restart();
            }
            if (userInput.ToString() == "exit")
                Exit();

            string activeCell;
            userInput.Replace(" ", "");
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
                InvalidMove();
            ClearEmptyCells();
            PrintTable();
        }

        private static void RemoveAllBaloons(int i, int j, string activeCell)
        {
            if ((i >= 0) && (i <= 4) && (j <= 9) && (j >= 0) && (gameMatrix[i, j] == activeCell))
            {
                gameMatrix[i, j] = ".";
                clearedCells++;
                //Up
                RemoveAllBaloons(i - 1, j, activeCell);
                //Down
                RemoveAllBaloons(i + 1, j, activeCell);
                //Left
                RemoveAllBaloons(i, j + 1, activeCell);
                //Right
                RemoveAllBaloons(i, j - 1, activeCell);
            }
            else
            {
                cellsLeft -= clearedCells;
                clearedCells = 0;
                return;
            }
        }

        private static void ClearEmptyCells()
        {
            int i;
            int j;
            Queue<string> temp = new Queue<string>();
            for (j = COLS_COUNT - 1; j >= 0; j--)
            {
                for (i = ROWS_COUNT - 1; i >= 0; i--)
                {
                    if (gameMatrix[i, j] != ".")
                    {
                        temp.Enqueue(gameMatrix[i, j]);
                        gameMatrix[i, j] = ".";
                    }
                }
                i = 4;
                while (temp.Count > 0)
                {
                    gameMatrix[i, j] = temp.Dequeue();
                    i--;
                }
                temp.Clear();
            }
        }

        private static bool IsFinished()
        {
            return (cellsLeft == 0);
        }
    }
}