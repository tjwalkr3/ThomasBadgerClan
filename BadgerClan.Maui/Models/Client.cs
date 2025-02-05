namespace BadgerClan.Maui.Models;

public class Client
{
    public string Name { get; set; } = default!;
    public HttpClient ApiClient { get; set; } = default!;
}
