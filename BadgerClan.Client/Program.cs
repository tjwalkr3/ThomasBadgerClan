using BadgerClan.Client.Services;
using BadgerClan.Client.GrpcServices;
using ProtoBuf.Grpc.Server;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Configure kestrel to run using the right ports and protocols
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddCodeFirstGrpc();
builder.Services.AddSingleton<IMoveService, MoveService>();

var app = builder.Build();

// client endpoint
app.MapGrpcService<GrpcMoveService>();

// server endpoint
app.MapControllers();

app.Run();
