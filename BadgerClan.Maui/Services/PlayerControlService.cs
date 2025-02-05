using BadgerClan.Maui.Models;
namespace BadgerClan.Maui.Services;

public class PlayerControlService : IPlayerControlService
{
    public List<Client> Clients { get; } = [];
    public Client? CurrentClient { get; private set; } = null;

    public void AddClient(string name, string baseUrl)
    {
        if (!baseUrl.EndsWith("/")) baseUrl += "/";
        HttpClient apiClient = new HttpClient() { BaseAddress = new Uri(baseUrl) };
        Clients.Add(new Client() { Name = name, ApiClient = apiClient });
    }

    public void SetCurrentClient(string name)
    {
        CurrentClient = Clients.First(c => c.Name == name);
    }

    private async Task MakeRequest(int playMode)
    {
        if (CurrentClient == null) throw new InvalidOperationException("There is no client set.");
        await CurrentClient.ApiClient.PostAsync($"client?playmode={playMode}", null);
    }

    public async Task AttackAsync() => await MakeRequest(0);
    public async Task DefendAsync() => await MakeRequest(1);
    public async Task ScatterAsync() => await MakeRequest(2);
    public async Task StopAsync() => await MakeRequest(3);
}
