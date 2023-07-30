using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides output to a virtual fixed size buffer.
/// </summary>
public sealed class FixedVirtualOutputComponent : IDisposable
{
    private readonly GraphicsDevice graphicsDevice;
    private readonly OutputBoundsStateComponent outputBoundsStateComponent;
    private readonly RenderTarget2D renderTarget;
    private readonly SpriteBatch spriteBatch;

    private readonly IDisposable beforeDrawRegistration;
    private readonly IDisposable afterDrawRegistration;

    public FixedVirtualOutputComponent(
        IGameComponentManager gameComponentManager,
        GraphicsDevice graphicsDevice,
        OutputBoundsStateComponent outputBoundsStateComponent,
        IOptions<ScreenOptions> options)
    {
        this.graphicsDevice = graphicsDevice;
        this.outputBoundsStateComponent = outputBoundsStateComponent;

        var size = options.Value.OutputSize ?? throw new InvalidOperationException("Output size must be specified.");

        renderTarget = new RenderTarget2D(graphicsDevice, size, size);
        spriteBatch = new SpriteBatch(graphicsDevice);

        beforeDrawRegistration = gameComponentManager.RegisterForBeforeDraw(BeforeDraw);
        afterDrawRegistration = gameComponentManager.RegisterForAfterDraw(AfterDraw);
    }

    private void BeforeDraw(GameTime gameTime)
    {
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Magenta);
    }

    private void AfterDraw(GameTime gameTime)
    {
        graphicsDevice.SetRenderTarget(null);
        graphicsDevice.Clear(Color.Black);

        var outputBounds = outputBoundsStateComponent.WindowRelativeOutputBounds;

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(renderTarget, outputBounds, Color.White);
        spriteBatch.End();
    }

    public void Dispose()
    {
        beforeDrawRegistration.Dispose();
        afterDrawRegistration.Dispose();
    }
}
