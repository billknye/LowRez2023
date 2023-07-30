using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides access to the tile the mouse is (or has most recently) hovered over.
/// </summary>
public sealed class MouseTileComponent : IDisposable
{
    private readonly ViewOffsetStateComponent viewOffsetComponent;
    private readonly MouseVirtualPixelViewOffsetComponent mouseVirtualPixelViewOffsetComponent;
    private readonly IOptions<TilesOptions> options;

    private readonly IDisposable updateRegistration;

    public Point Location { get; private set; }

    public MouseTileComponent(
        IGameComponentManager gameComponentManager,
        ViewOffsetStateComponent viewOffsetComponent,
        MouseVirtualPixelViewOffsetComponent mouseVirtualPixelViewOffsetComponent,
        IOptions<TilesOptions> options)
    {
        this.viewOffsetComponent = viewOffsetComponent;
        this.mouseVirtualPixelViewOffsetComponent = mouseVirtualPixelViewOffsetComponent;
        this.options = options;

        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
    }

    private void Update(GameTime gameTime)
    {
        var tileSize = options.Value.Size ?? throw new InvalidOperationException("Tile size must be defined.");
        var mouseViewOffsetLocation = mouseVirtualPixelViewOffsetComponent.Location;
        var viewOffset = viewOffsetComponent.ViewOffset;

        Location = new Point((int)Math.Floor((mouseViewOffsetLocation.X) / (double)tileSize), (int)Math.Floor((mouseViewOffsetLocation.Y) / (double)tileSize));
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }
}
