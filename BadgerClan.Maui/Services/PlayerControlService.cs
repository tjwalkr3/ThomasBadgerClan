namespace BadgerClan.Maui.Services;

public class PlayerControlService : IPlayerControlService
{
    private HttpClient? _client = null;

    public void SetBaseUrl(string baseUrl)
    {
        if (!baseUrl.EndsWith("/")) baseUrl += "/";
        _client = new HttpClient() { BaseAddress = new Uri(baseUrl) };
    }
}
