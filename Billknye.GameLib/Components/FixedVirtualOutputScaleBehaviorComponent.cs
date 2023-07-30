using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides output scale support when using fixed virtual output, like LowRez game jam.
/// </summary>
public sealed class FixedVirtualOutputScaleBehaviorComponent : IDisposable
{
    private readonly GameWindow gameWindow;
    private readonly OutputScaleStateComponent outputScaleStateComponent;
    private readonly IOptions<ScreenOptions> options;
    IDisposable updateRegistration;

    public FixedVirtualOutputScaleBehaviorComponent(
        IGameComponentManager gameComponentManager,
        GameWindow gameWindow,
        OutputScaleStateComponent outputScaleStateComponent,
        IOptions<ScreenOptions> options)
    {
        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
        this.gameWindow = gameWindow;
        this.outputScaleStateComponent = outputScaleStateComponent;
        this.options = options;
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }

    private void Update(GameTime gameTime)
    {
        var screenSize = options.Value.OutputSize ?? throw new InvalidOperationException("Output size must be defined for fixed virtual output scaling.");

        var minSize = Math.Min(gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
        outputScaleStateComponent.PixelScale = (int)Math.Floor(minSize / (float)screenSize);
    }
}
