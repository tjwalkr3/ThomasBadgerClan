using BadgerClan.Maui.Models;
using BadgerClan.Shared;
using Microsoft.Extensions.Logging;
namespace BadgerClan.Maui.Services;

public class PlayerControlService(ILogger<PlayerControlService> logger) : IPlayerControlService
{
    public List<Client> Clients { get; } = [];
    public Client? CurrentClient { get; private set; } = null;

    public void AddClient(string name, string baseUrl, bool grpcEnabled)
    {
        if (!baseUrl.EndsWith("/")) baseUrl += "/";

        if (grpcEnabled)
        {
            GrpcClient apiClient = new GrpcClient(baseUrl);
            Clients.Add(new Client() { Name = name, GrpcClient = apiClient, GrpcEnabled = grpcEnabled });
        }
        else
        {
            HttpClient apiClient = new HttpClient() { BaseAddress = new Uri(baseUrl) };
            Clients.Add(new Client() { Name = name, ApiClient = apiClient, GrpcEnabled = grpcEnabled });
        }
    }

    public void SetCurrentClient(string name)
    {
        CurrentClient = Clients.First(c => c.Name == name);
    }

    private async Task MakeRequest(int playMode)
    {
        try
        {
            if (CurrentClient == null) throw new InvalidOperationException("There is no client set.");
            if (CurrentClient.GrpcEnabled && CurrentClient.GrpcClient is not null)
            {
                MoveRequest request = new() { PlayStyle = playMode };
                MoveResponse response = await CurrentClient.GrpcClient.Client.ChangeStrategy(request);
            }
            else if (CurrentClient.ApiClient is not null)
            {
                await CurrentClient.ApiClient!.PostAsync($"client?playmode={playMode}", null);
            }
            else
            {
                throw new InvalidOperationException("No API client available.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
        }
    }

    public async Task AttackAsync() => await MakeRequest(0);
    public async Task DefendAsync() => await MakeRequest(1);
    public async Task ScatterAsync() => await MakeRequest(2);
    public async Task AdvancedAsync() => await MakeRequest(3);
    public async Task StopAsync() => await MakeRequest(4);
}
