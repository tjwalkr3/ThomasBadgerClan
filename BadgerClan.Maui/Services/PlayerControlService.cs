namespace BadgerClan.Maui.Services;

public class PlayerControlService : IPlayerControlService
{
    private HttpClient? _client = null;

    public void SetBaseUrl(string baseUrl)
    {
        if (!baseUrl.EndsWith("/")) baseUrl += "/";
        _client = new HttpClient() { BaseAddress = new Uri(baseUrl) };
    }

    private async Task MakeRequest(int playMode)
    {
        if (_client == null) throw new InvalidOperationException("Base URL not set");
        await _client.PostAsync($"client?playmode={playMode}", null);
    }

    public async Task AttackAsync() => await MakeRequest(0);
    public async Task DefendAsync() => await MakeRequest(1);
    public async Task ScatterAsync() => await MakeRequest(2);
    public async Task StopAsync() => await MakeRequest(3);
}
