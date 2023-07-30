using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides dynamic centering of a fixed output buffer within the window client bounds.
/// </summary>
public sealed class FixedOutputBoundsBehaviorComponent : IDisposable
{
    IDisposable updateRegistration;
    private readonly GameWindow gameWindow;
    private readonly OutputBoundsStateComponent outputBoundsComponent;
    private readonly OutputScaleStateComponent outputScaleStateComponent;
    private readonly IOptions<ScreenOptions> options;

    public FixedOutputBoundsBehaviorComponent(
        IGameComponentManager gameComponentManager,
        GameWindow gameWindow,
        OutputBoundsStateComponent outputBoundsComponent,
        OutputScaleStateComponent outputScaleStateComponent,
        IOptions<ScreenOptions> options)
    {
        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
        this.gameWindow = gameWindow;
        this.outputBoundsComponent = outputBoundsComponent;
        this.outputScaleStateComponent = outputScaleStateComponent;
        this.options = options;
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }

    private void Update(GameTime gameTime)
    {
        var screenSize = options.Value.OutputSize ?? throw new InvalidOperationException("Output size must be defined.");

        var drawSize = screenSize * outputScaleStateComponent.PixelScale;
        var horizontalPadding = (gameWindow.ClientBounds.Width - drawSize) / 2;
        var verticalPadding = (gameWindow.ClientBounds.Height - drawSize) / 2;
        outputBoundsComponent.WindowRelativeOutputBounds = new Rectangle(horizontalPadding, verticalPadding, drawSize, drawSize);
    }
}
