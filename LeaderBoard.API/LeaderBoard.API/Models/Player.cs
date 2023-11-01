namespace LeaderBoard.API.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
}

public record UpsertPlayerDto(string Name, int Score);