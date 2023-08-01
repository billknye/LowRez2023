using Billknye.GameLib;
using LowRez2023;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

#if !BLAZOR
var game = GameStart.CreateGame(args);
game.Run();
#endif

public static partial class GameStart
{
    public static Game CreateGame(string[]? args)
    {
        var game = Startup.CreateGame<TestingGameState>(args, AddGameServices);
        return game;
    }

    static void AddGameServices(IServiceCollection services)
    {

    }
}