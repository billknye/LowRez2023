using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides the currently visible tile bounds;
/// </summary>
public sealed class VisibleTileAreaComponent : IDisposable
{
    private readonly OutputScaleStateComponent outputScaleStateComponent;
    private readonly OutputBoundsStateComponent outputBoundsStateComponent;
    private readonly ViewOffsetStateComponent viewOffsetStateComponent;
    private readonly IOptions<TilesOptions> options;
    private readonly IDisposable updateRegistration;

    public Rectangle VisibleTiles { get; private set; }

    public VisibleTileAreaComponent(
        IGameComponentManager gameComponentManager,
        OutputScaleStateComponent outputScaleStateComponent,
        OutputBoundsStateComponent outputBoundsStateComponent,
        ViewOffsetStateComponent viewOffsetStateComponent,
        IOptions<TilesOptions> options)
    {
        this.outputScaleStateComponent = outputScaleStateComponent;
        this.outputBoundsStateComponent = outputBoundsStateComponent;
        this.viewOffsetStateComponent = viewOffsetStateComponent;
        this.options = options;
        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }

    private void Update(GameTime gameTime)
    {
        var tileSize = options.Value.Size ?? throw new InvalidOperationException("Tile size must be specified.");
        var pixelScale = outputScaleStateComponent.PixelScale;
        var outputSize = outputBoundsStateComponent.WindowRelativeOutputBounds.Size;
        var viewOffset = viewOffsetStateComponent.ViewOffset;

        if (outputSize.X == 0 || outputSize.Y == 0)
        {
            VisibleTiles = default;
            return;
        }

        var minx = viewOffset.X / tileSize;
        var miny = viewOffset.Y / tileSize;
        var maxx = (int)Math.Ceiling(viewOffset.X + outputSize.X / pixelScale / (float)tileSize);
        var maxy = (int)Math.Ceiling(viewOffset.Y + outputSize.Y / pixelScale / (float)tileSize);

        VisibleTiles = new Rectangle(minx, miny, maxx - minx, maxy - miny);
    }
}