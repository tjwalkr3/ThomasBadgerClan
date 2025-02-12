using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;
using Grpc.Net.Client;
using BadgerClan.Shared;
namespace BadgerClan.Maui.Services;

public class GrpcClient : IDisposable
{
    private GrpcChannel channel;
    public IGrpcMoveService Client { get; }

    public GrpcClient(string grpcApiAddress)
    {
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        channel = GrpcChannel.ForAddress(grpcApiAddress, new GrpcChannelOptions { HttpHandler = httpHandler });
        Client = channel.CreateGrpcService<IGrpcMoveService>();
    }

    public void Dispose()
    {
        channel.Dispose();
    }
}
