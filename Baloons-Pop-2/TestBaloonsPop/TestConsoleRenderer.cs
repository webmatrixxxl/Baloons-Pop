using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BaloonsPop;

namespace TestBaloonsPop
{
    [TestClass]
    public class TestConsoleRenderer
    {
        [TestMethod]
        public void TestPrintGameMatrix()
        {
            string[,] gameMatrix = { { "1", "2" }, { "3", "4" } };
            string expectedOutput = "    0 1 2 3 4 5 6 7 8 9\r\n" +
                "   ---------------------\r\n" +
                "0 | 1 2 | \r\n" +
                "1 | 3 4 | \r\n" +
                "   ---------------------\r\n\r\n";

            StringWriter output = new StringWriter();
            Console.SetOut(output);
            ConsoleRenderer.PrintGameMatrix(gameMatrix);

            Assert.AreEqual(expectedOutput, output.ToString());
        }
    }
}
