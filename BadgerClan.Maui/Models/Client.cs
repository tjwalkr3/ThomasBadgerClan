using BadgerClan.Maui.Services;
namespace BadgerClan.Maui.Models;

public class Client
{
    public string Name { get; set; } = default!;
    public HttpClient? ApiClient { get; set; } = default!;
    public GrpcClient? GrpcClient { get; set; } = default!;
    public bool GrpcEnabled { get; set; } = false;
}
