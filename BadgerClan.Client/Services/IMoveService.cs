using BadgerClan.Logic;
namespace BadgerClan.Client.Services;

public interface IMoveService
{
    MoveResponse GetMoves(MoveRequest request);
    bool SetPlayMode(int playMode);
}