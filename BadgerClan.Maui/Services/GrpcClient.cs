using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using Grpc.Net.Client;
using BadgerClan.Shared;
namespace BadgerClan.Maui.Services;

public class GrpcClient : IDisposable
{
#if DEBUG
    private const string GrpcApiAddress = "http://localhost:5000";
#else
    private const string GrpcApiAddress = "http://localhost:5000";
#endif

    private GrpcChannel channel;
    public IGrpcMoveService Client { get; }

    public GrpcClient()
    {
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        channel = GrpcChannel.ForAddress(GrpcApiAddress, new GrpcChannelOptions { HttpHandler = httpHandler });
        Client = channel.CreateGrpcService<IGrpcMoveService>();
    }

    public void Dispose()
    {
        channel.Dispose();
    }
}
