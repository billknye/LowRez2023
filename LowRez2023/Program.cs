using Billknye.GameLib;
using LowRez2023;
using Microsoft.Extensions.DependencyInjection;

Startup.Run<TestingGameState>(args, services =>
{
    services.AddScoped<RenderMapComponent>();
    services.AddScoped<MousePanComponent>();
});