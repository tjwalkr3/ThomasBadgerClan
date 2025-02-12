using BadgerClan.Client.Controllers;
using BadgerClan.Client.Services;
using BadgerClan.Shared;

namespace BadgerClan.Client.GrpcServices;

public class GrpcMoveService(ILogger<ClientController> logger, IMoveService moveService) : IGrpcMoveService
{
    public Task<MoveResponse> ChangeStrategy(MoveRequest request)
    {
        int playMode = request.PlayStyle;
        bool succeeded = moveService.SetPlayMode(playMode);
        MoveResponse response = new();

        if (succeeded)
        {
            logger.LogInformation("Set play mode to {playModeString} (gRPC)", ((PlayMode)playMode).ToString());
            response.Success = true;
        }
        else
        {
            logger.LogError("Invalid play mode {playMode} (gRPC)", playMode);
            response.Success = false;
        }

        return Task.FromResult(response);
    }
}
