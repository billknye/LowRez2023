using Billknye.GameLib.States;
using LowRez2023.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LowRez2023;

public sealed class PlayingGameState : GameState
{
    private readonly RenderTarget2D renderTarget;
    private readonly SpriteBatch spriteBatch;
    private readonly GraphicsDevice graphicsDevice;

    private readonly SpriteRenderer spriteRenderer;
    private readonly TextRenderer textRenderer;

    private readonly Camera camera;
    private readonly Tool tool;
    private readonly MapRenderer renderMapComponent;
    private readonly Notices notices;

    private readonly Map map;

    public PlayingGameState(GraphicsDevice graphicsDevice, GameWindow gameWindow, ContentManager contentManager)
    {
        renderTarget = new RenderTarget2D(graphicsDevice, Camera.OutputSize, Camera.OutputSize);
        spriteBatch = new SpriteBatch(graphicsDevice);
        this.graphicsDevice = graphicsDevice;

        map = new Map(256, 256);

        var spriteSheet = contentManager.Load<Texture2D>("Sprites");
        spriteRenderer = new SpriteRenderer(spriteBatch, spriteSheet);
        textRenderer = new TextRenderer(spriteRenderer);

        notices = new Notices(textRenderer);
        camera = new Camera(gameWindow, map);
        tool = new Tool(spriteRenderer, textRenderer, camera, map, notices);
        renderMapComponent = new MapRenderer(camera, map, spriteRenderer);
    }

    protected override void StateEnteredInternal()
    {
    }

    protected override void UpdateInternal(GameTime gameTime)
    {
        camera.Update();
        tool.Update();
        notices.Update(gameTime);
    }

    protected override void DrawInternal(GameTime gameTime)
    {
        // Switch to virtual output target.
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Magenta);

        spriteBatch.Begin();

        renderMapComponent.Draw();
        tool.Draw();

        notices.Draw();

        textRenderer.DrawText(new Point(0, 0), "$1,000,000");

        spriteBatch.End();

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
