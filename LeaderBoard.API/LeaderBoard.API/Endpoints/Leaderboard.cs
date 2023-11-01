using LeaderBoard.API.Models;
using LeaderBoard.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeaderBoard.API.Endpoints;

public static class Leaderboard
{
    public static void MapLeaderboardEndpoints(this WebApplication app)
    {
        app.MapPost("/add-score", AddScore)
            .WithDescription("Add or update a player's score.");

        app.MapGet("/top-k", TopKPlayers)
            .WithDescription("Gets the Top k players.");

        app.MapGet("/reset", ResetScore)
            .WithDescription("Resets the player's score to 0 and excludes player from the leaderboard.");
    }

    private static async ValueTask<IResult> AddScore(
        [FromServices] ILeaderboardServices services,
        [FromBody] UpsertPlayerDto playerDto
        )
    {
        if (playerDto.Score <= 0) return Results.BadRequest(new
        {
            Message = "player score is less-than 0.",
            Player = playerDto,
        });
        var result = services.UpsertScore(playerDto.Name, playerDto.Score);
        return Results.Ok(result);
    }

    private static async ValueTask<IResult> TopKPlayers( 
        [FromServices] ILeaderboardServices services,
        [FromQuery] int k = 10
        )
    {
        if (k <= 0)
        {
            return Results.Ok(Array.Empty<Player>());
        }
        var result = services.TopKPlayers(k);
        return Results.Ok(result);
    }

    private static async ValueTask<IResult> ResetScore( 
        [FromServices] ILeaderboardServices services,
        [FromQuery] string name
        )
    {
        var success = services.ResetScore(name);
        return success ? Results.Ok(success) : Results.NotFound(success);
    }
}
