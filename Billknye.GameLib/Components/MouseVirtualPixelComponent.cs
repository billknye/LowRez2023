using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides virtual pixel mouse location.
/// </summary>
public sealed class MouseVirtualPixelComponent : IDisposable
{
    private readonly OutputScaleStateComponent outputScaleStateComponent;
    private readonly OutputBoundsStateComponent outputBoundsStateComponent;
    private readonly MouseStateComponent mouseStateComponent;

    private readonly IDisposable updateRegistration;

    public Point Location { get; private set; }

    public MouseVirtualPixelComponent(
        IGameComponentManager gameComponentManager,
        OutputScaleStateComponent outputScaleStateComponent,
        OutputBoundsStateComponent outputBoundsStateComponent,
        MouseStateComponent mouseStateComponent)
    {
        this.outputScaleStateComponent = outputScaleStateComponent;
        this.outputBoundsStateComponent = outputBoundsStateComponent;
        this.mouseStateComponent = mouseStateComponent;

        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
    }

    private void Update(GameTime gameTime)
    {
        var mouseState = mouseStateComponent.CurrentState;
        var mouseInOutputArea = outputBoundsStateComponent.WindowRelativeOutputBounds.Contains(mouseState.X, mouseState.Y);
        var scale = outputScaleStateComponent.PixelScale;

        if (mouseInOutputArea)
        {
            var mouseOutputRelativeInPixels = new Point(mouseState.X - outputBoundsStateComponent.WindowRelativeOutputBounds.X, mouseState.Y - outputBoundsStateComponent.WindowRelativeOutputBounds.Y);
            Location = new Point((int)Math.Floor(mouseOutputRelativeInPixels.X / (double)scale), (int)Math.Floor(mouseOutputRelativeInPixels.Y / (double)scale));
        }
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }
}
