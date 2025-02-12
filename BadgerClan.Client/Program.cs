using BadgerClan.Client.Services;
using BadgerClan.Client.GrpcServices;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddCodeFirstGrpc();
builder.Services.AddSingleton<IMoveService, MoveService>();

var app = builder.Build();

// client endpoint
app.MapGrpcService<GrpcMoveService>();

// server endpoint
app.MapControllers();

string url = app.Configuration["ASPNETCORE_URLS"]?.Split(";").Last() ?? "http://localhost:5001";
int port = new Uri(url).Port;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Welcome to the Sample BadgerClan Bot!");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Change the code in Program.cs to add custom behavior.");
Console.WriteLine("If you're running this locally, use the following URL to join your bot:");
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\t{url}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine("For the competition, start a DevTunnel for this port with the following commands:");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\t winget install Microsoft.devtunnel");
Console.WriteLine("\t [ restart your command line after installing devtunnel ]");
Console.WriteLine("\t devtunnel user login");
Console.WriteLine($"\t devtunnel host -p {port} --allow-anonymous");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine("In the output from the 'devtunnel host' command, look for the \"Connect via browser:\" URL.  Paste that in the browser as your bot's address");

app.Run();
