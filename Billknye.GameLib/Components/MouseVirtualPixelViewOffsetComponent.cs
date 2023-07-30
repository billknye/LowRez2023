using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides virtual pixel mouse location with view offset applied.
/// </summary>
public sealed class MouseVirtualPixelViewOffsetComponent : IDisposable
{
    private readonly MouseVirtualPixelComponent mouseVirtualPixelComponent;
    private readonly ViewOffsetStateComponent viewOffsetStateComponent;

    private readonly IDisposable updateRegistration;

    public Point Location { get; set; }

    public MouseVirtualPixelViewOffsetComponent(
        IGameComponentManager gameComponentManager,
        MouseVirtualPixelComponent mouseVirtualPixelComponent,
        ViewOffsetStateComponent viewOffsetStateComponent)
    {
        this.mouseVirtualPixelComponent = mouseVirtualPixelComponent;
        this.viewOffsetStateComponent = viewOffsetStateComponent;

        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
    }

    private void Update(GameTime gameTime)
    {
        var mouseVirtualPixelLocation = mouseVirtualPixelComponent.Location;
        var viewOffset = viewOffsetStateComponent.ViewOffset;

        Location = new Point(mouseVirtualPixelLocation.X + viewOffset.X, mouseVirtualPixelLocation.Y + viewOffset.Y);
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }
}