using Billknye.GameLib.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Billknye.GameLib;

public sealed class GameHost<TInitialState> : Game
    where TInitialState : GameState
{
    private GraphicsDeviceManager graphics;

    private readonly string[] args;
    private readonly Action<IServiceCollection>? services;
    private IHost? host;

    private IGameStateManager? gameStateManager;

    public GameHost(string[] args, Action<IServiceCollection>? services = null)
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        this.args = args;
        this.services = services;
    }

    protected override void Initialize()
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddOptions();

        builder.Services.AddSingleton<Game>(this);
        builder.Services.AddSingleton(graphics);
        builder.Services.AddSingleton(GraphicsDevice);
        builder.Services.AddSingleton(Content);
        builder.Services.AddSingleton(Window);

        builder.Services.AddSingleton<IGameStateManager, GameStateManager>();

        services?.Invoke(builder.Services);

        host = builder.Build();

        var hostTask = host.RunAsync();

        gameStateManager = host.Services.GetRequiredService<IGameStateManager>();
        gameStateManager.EnterState<TInitialState>();
        base.Initialize();
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        if (host != null)
        {
            host.StopAsync(TimeSpan.FromSeconds(4.0)).GetAwaiter().GetResult();
        }

        base.OnExiting(sender, args);
    }

    protected override void LoadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        gameStateManager?.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        gameStateManager?.Draw(gameTime);
        base.Draw(gameTime);
    }
}
