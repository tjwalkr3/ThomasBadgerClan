
namespace BadgerClan.Maui.Services
{
    public interface IPlayerControlService
    {
        void SetBaseUrl(string baseUrl);
        Task AttackAsync();
        Task DefendAsync();
        Task ScatterAsync();
        Task StopAsync();
    }
}