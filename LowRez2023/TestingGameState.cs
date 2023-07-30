using Billknye.GameLib.States;
using LowRez2023.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace LowRez2023;

internal sealed class TestingGameState : GameState
{
    private readonly RenderTarget2D renderTarget;
    private readonly SpriteBatch spriteBatch;
    private readonly GraphicsDevice graphicsDevice;

    private readonly Camera camera;
    private readonly MapRenderer renderMapComponent;

    private readonly Map map;

    public TestingGameState(GraphicsDevice graphicsDevice, GameWindow gameWindow, ContentManager contentManager)
    {
        renderTarget = new RenderTarget2D(graphicsDevice, Camera.OutputSize, Camera.OutputSize);
        spriteBatch = new SpriteBatch(graphicsDevice);
        this.graphicsDevice = graphicsDevice;

        map = new Map(256, 256);

        var spriteSheet = contentManager.Load<Texture2D>("Sprites");
        camera = new Camera(gameWindow, map);
        renderMapComponent = new MapRenderer(camera, map, spriteSheet, spriteBatch);
    }
    protected override void StateEnteredInternal()
    {
    }

    protected override void UpdateInternal(GameTime gameTime)
    {
        camera.Update();
    }

    protected override void DrawInternal(GameTime gameTime)
    {
        // Switch to virtual output target.
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Magenta);

        renderMapComponent.Draw();

        // Switch to back buffer.
        graphicsDevice.SetRenderTarget(null);
        graphicsDevice.Clear(Color.Black);

        // Draw scaled virtual output.
        var outputBounds = camera.OutputBounds;
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(renderTarget, outputBounds, Color.White);
        spriteBatch.End();
    }
}
