using System;
using System.Text;

public class Statistics
{
    private const int NUMBER_OF_PLAYERS_TO_SHOW = 5;
    Player[] topFive;
    int players = 0;

    public Statistics()
    {
        topFive = new Player[NUMBER_OF_PLAYERS_TO_SHOW + 1];

        for (int i = 0; i < topFive.Length; i++)
        {
            topFive[i] = new Player();
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Scoreboard:");


        for (int i = 0; i < players; i++)
        {
            builder.AppendFormat("{0}. {1} --> {2} moves\r\n", i + 1, topFive[i].Name, topFive[i].Score);
        }

        return builder.ToString();
    }

    public void AddPlayer(string name, int score)
    {
        topFive[players].Name = name;
        topFive[players].Score = score;

        if (players < NUMBER_OF_PLAYERS_TO_SHOW)
        {
            players++;
        }

        SortPlayers(topFive);
    }

    private void SortPlayers(Player[] players)
    {
        Array.Sort(players, delegate(Player player1, Player player2)
        {
            return player1.Score.CompareTo(player2.Score);
        });
    }
}

internal class Player
{
    public string Name { get; set; }
    public int Score { get; set; }

    public Player()
    {
        this.Score = int.MaxValue;
        this.Name = string.Empty;
    }
}