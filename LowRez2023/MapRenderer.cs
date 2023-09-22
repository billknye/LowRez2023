using LowRez2023.Simulation;
using Microsoft.Xna.Framework;

namespace LowRez2023;

internal sealed class MapRenderer
{
    private readonly Camera camera;
    private readonly Map map;
    private readonly SpriteRenderer spriteRenderer;

    public MapRenderer(Camera camera, Map map, SpriteRenderer spriteRenderer)
    {
        this.camera = camera;
        this.map = map;
        this.spriteRenderer = spriteRenderer;
    }

    public void Draw()
    {
        var tileSize = Camera.TileSize;

        DrawMap(tileSize);

        Rectangle src;
        Rectangle dest;

        var mouseTile = camera.MouseTile;
        var viewOffset = camera.ViewOffset;

        src = new Rectangle(tileSize, 0, tileSize, tileSize);
        dest = new Rectangle(mouseTile.X * tileSize - viewOffset.X, mouseTile.Y * tileSize - viewOffset.Y, tileSize, tileSize);
        spriteRenderer.DrawSprite(src, dest, Color.FromNonPremultiplied(0, 0, 0, 128));
    }

    private void DrawMap(int tileSize)
    {
        var visibleTiles = camera.VisibleTiles;

        for (int x = visibleTiles.X; x < visibleTiles.Right; x++)
        {
            for (int y = visibleTiles.Y; y < visibleTiles.Bottom; y++)
            {
                DrawTile(tileSize, map, x, y);
            }
        }
    }

    private void DrawTile(int tileSize, Map map, int x, int y)
    {
        Rectangle src;
        Rectangle dest;

        var tile = map[x, y];
        dest = new Rectangle(x * 5 - camera.ViewOffset.X, y * tileSize - camera.ViewOffset.Y, tileSize, tileSize);

        // Terrain draw
        src = new Rectangle(0 + tile.Terrain == Terrain.Water ? tileSize : 0, 10, tileSize, tileSize);
        spriteRenderer.DrawSprite(src, dest, Color.White);

        // Shore draw
        if (tile.Terrain == Terrain.Water)
        {
            var neighbors = map.GetNonWaterNeighbors(x, y);
            src = new Rectangle((int)neighbors * tileSize, 3 * tileSize, tileSize, tileSize);
            spriteRenderer.DrawSprite(src, dest, Color.White);
        }

        if (tile.Improvement == Improvement.Road)
        {
            var neighbors = map.GetRoadNeighbors(x, y);
            src = new Rectangle(tileSize * (int)neighbors, 30, tileSize, tileSize);
            spriteRenderer.DrawSprite(src, dest, Color.White);
        }
        else if (tile.Improvement == Improvement.Lot)
        {
            src = new Rectangle(0, 0, tileSize, tileSize);
            spriteRenderer.DrawSprite(src, dest, Color.FromNonPremultiplied(0, 255, 0, 128));

            if (tile.Lot.Flags.HasFlag(LotFlags.TransportAccess))
            {
                src = new Rectangle(55 + (int)tile.Lot.Orientation * 5, 0, 5, 5);
                spriteRenderer.DrawSprite(src, dest, Color.FromNonPremultiplied(0, 255, 0, 128));
            }
        }

        // Grid overlay draw
        src = new Rectangle(5 * tileSize, 0, tileSize, tileSize);

        spriteRenderer.DrawSprite(src, dest, Color.FromNonPremultiplied(0, 0, 0, 32));
    }
}
