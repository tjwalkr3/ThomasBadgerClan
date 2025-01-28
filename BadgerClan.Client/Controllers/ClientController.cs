namespace BadgerClan.Client.Controllers;

using BadgerClan.Client.Services;
using BadgerClan.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("[controller]")]
public class ClientController(ILogger<ClientController> logger, IMoveService moveService) : ControllerBase
{
    [HttpGet]
    public IResult TestEndpoint()
    {
        return Results.Ok("Thomas Jones' BadgerClan bot, client endpoint.");
    }

    [HttpPost]
    public IResult SetPlayMode([FromQuery] int playMode)
    {
        if (moveService.SetPlayMode(playMode))
        {
            logger.LogInformation("Set play mode to {playModeString}", ((PlayMode)playMode).ToString());
            return Results.Ok();
        }
        else
        {
            logger.LogError("Invalid play mode {playMode}", playMode);
            return Results.BadRequest();
        }
    }
}