using Billknye.GameLib.States;
using Microsoft.Extensions.DependencyInjection;

namespace Billknye.GameLib;

public static class Startup
{
    public static void Run<TInitialState>(string[] args, Action<IServiceCollection> services = null)
        where TInitialState : GameState
    {
        using var game = new GameHost<TInitialState>(args, services);
        game.Run();
    }
}