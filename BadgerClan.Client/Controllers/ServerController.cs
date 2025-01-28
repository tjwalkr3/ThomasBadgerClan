using BadgerClan.Logic;
using BadgerClan.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadgerClan.Client.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerController(ILogger<ServerController> logger, IMoveService moveService) : ControllerBase
{
    [HttpGet]
    public IResult TestEndpoint()
    {
        return Results.Ok("Thomas Jones' BadgerClan bot, server endpoint.");
    }

    [HttpPost]
    public IResult Move([FromBody] MoveRequest request)
    {
        logger.LogInformation("Received move request for game {gameId} turn {turnNumber}", request.GameId, request.TurnNumber);

        MoveResponse response = moveService.GetMoves(request);
        return Results.Ok(response);
    }
}
