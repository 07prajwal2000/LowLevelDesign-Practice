using LeaderBoard.API.Models;

namespace LeaderBoard.API.Services;

public class LeaderboardServices : ILeaderboardServices
{
    private Dictionary<string, Player> leaderboard = new();
    private int idCounter;

    public Player UpsertScore(string name, int score)
    {
        if (leaderboard.TryGetValue(name, out var p))
        {
            p.Score = score;
            return p;
        }
        var player = new Player
        {
            Id = ++idCounter,
            Name = name,
            Score = score
        };
        leaderboard.Add(name, player);
        return player;
    }

    public List<Player> TopKPlayers(int k)
    {
        PriorityQueue<Player, int> sortedPlayers = new(Comparer<int>.Create((x, y) => y - x));
        foreach (var kv in leaderboard)
        {
            var p = kv.Value;
            sortedPlayers.Enqueue(p, p.Score);
        }
        List<Player> result = new();
        var i = 0;
        while (sortedPlayers.Count > 0 && i++ <= k)
        {
            var p = sortedPlayers.Dequeue();
            if (p.Score == 0) continue;
            result.Add(p);
        }
        return result;
    }

    public bool ResetScore(string name)
    {
        if (!leaderboard.TryGetValue(name, out Player? value)) return false;
        var p = value;
        p.Score = 0; 
        return true;
    }
}

public interface ILeaderboardServices
{
    bool ResetScore(string name);
    List<Player> TopKPlayers(int k);
    Player UpsertScore(string name, int score);
}