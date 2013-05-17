using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaloonsPop
{
    public class ConsoleRenderer
    {
        public static void PrintGameMatrix(string[,] gameMatrix)
        {
            Console.WriteLine(GameMatrixToString(gameMatrix));
        }

        public static void PrintGreetingMessage()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons. Use 'top' to view the " +
                              "top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");
        }

        private static string GameMatrixToString(string[,] matrix)
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
    }
}