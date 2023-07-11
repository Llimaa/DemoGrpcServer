using DemoGrpcServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);


// If you use macOS for encoding, you need to configure HTTP/2 endpoint without TLS 
// because kestrel is not supported in .NET7 or earlier.
// We use this just in Development Environment.
builder.WebHost.ConfigureKestrel(options => {
    options.ListenLocalhost(5287, _ => _.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

var app = builder.Build();



// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CounterService>();

app.Run();
