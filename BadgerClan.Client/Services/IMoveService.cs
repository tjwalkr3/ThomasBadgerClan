using BadgerClan.Client.Logic;

namespace BadgerClan.Client.Services
{
    public interface IMoveService
    {
        Task<MoveResponse> GetResponse(MoveRequest request);
        bool SetPlayMode(int playMode);
    }
}