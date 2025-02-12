using BadgerClan.Client.Services;
using BadgerClan.Client.GrpcServices;
using ProtoBuf.Grpc.Server;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Retrieve Base64-encoded certificate from environment variables
var certBase64 = Environment.GetEnvironmentVariable("CERTIFICATE_BASE64");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
        Console.WriteLine("? Plain HTTP server running on port 5001");
    });

    if (!string.IsNullOrEmpty(certBase64))
    {
        try
        {
            // Decode Base64 string into byte array
            byte[] certBytes = Convert.FromBase64String(certBase64);

            // Load certificate using ReadOnlySpan<byte>
            var cert = X509CertificateLoader.LoadCertificate(certBytes.AsSpan());

            // Configure Kestrel to use the certificate
            options.ListenAnyIP(5000, listenOptions =>
            {
                listenOptions.UseHttps(cert);
                listenOptions.Protocols = HttpProtocols.Http2;
            });

            Console.WriteLine("? SSL Certificate Loaded Successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Failed to load SSL certificate: {ex.Message}");
        }
    }
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
