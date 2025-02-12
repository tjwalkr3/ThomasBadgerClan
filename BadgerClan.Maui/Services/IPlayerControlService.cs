
using BadgerClan.Maui.Models;

namespace BadgerClan.Maui.Services
{
    public interface IPlayerControlService
    {
        List<Client> Clients { get; }
        Client? CurrentClient { get; }
        void SetCurrentClient(string name);

        void AddClient(string name, string baseUrl, bool grpcEnabled);
        Task AttackAsync();
        Task DefendAsync();
        Task ScatterAsync();
        Task StopAsync();
    }
}