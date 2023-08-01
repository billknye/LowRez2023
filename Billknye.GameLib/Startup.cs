using Billknye.GameLib.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib;

public static class Startup
{
    public static Game CreateGame<TInitialState>(string[] args, Action<IServiceCollection> services = null)
        where TInitialState : GameState
    {
        var game = new GameHost<TInitialState>(args, services);
        return game;
    }
}