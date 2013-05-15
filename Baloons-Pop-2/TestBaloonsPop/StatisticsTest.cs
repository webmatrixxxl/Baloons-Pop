using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestBaloonsPop
{
    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void AddPlayer_SevenPlayers()
        {
            Statistics stats = new Statistics();
            stats.AddPlayer("Player4", 14);
            stats.AddPlayer("Player1", 11);
            stats.AddPlayer("Player2", 12);
            stats.AddPlayer("Player8", 18);
            stats.AddPlayer("Player5", 15);
            stats.AddPlayer("Player7", 17);
            stats.AddPlayer("Player3", 13);

            string actual = stats.ToString();
            string expected =
                "Scoreboard:\r\n" +
                "1. Player1 --> 11 moves\r\n" +
                "2. Player2 --> 12 moves\r\n" +
                "3. Player3 --> 13 moves\r\n" +
                "4. Player4 --> 14 moves\r\n" +
                "5. Player5 --> 15 moves\r\n";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPlayer_ThreePlayers_SameNames()
        {
            Statistics stats = new Statistics();
            stats.AddPlayer("Player7", 17);
            stats.AddPlayer("Player", 12);
            stats.AddPlayer("Player", 15);

            string actual = stats.ToString();
            string expected =
                "Scoreboard:\r\n" +
                "1. Player --> 12 moves\r\n" +
                "2. Player --> 15 moves\r\n" +
                "3. Player7 --> 17 moves\r\n";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPlayer_ThreePlayers_SameScores()
        {
            Statistics stats = new Statistics();
            stats.AddPlayer("Player7", 17);
            stats.AddPlayer("Player", 12);
            stats.AddPlayer("Player", 12);

            string actual = stats.ToString();
            string expected =
                "Scoreboard:\r\n" +
                "1. Player --> 12 moves\r\n" +
                "2. Player --> 12 moves\r\n" +
                "3. Player7 --> 17 moves\r\n";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_NoPlayers()
        {
            Statistics stats = new Statistics();
            string actual = stats.ToString();
            Assert.AreEqual("Scoreboard:\r\n", actual);
        }
    }
}
