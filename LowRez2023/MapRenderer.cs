using LowRez2023.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LowRez2023;

internal sealed class MapRenderer
{
    private readonly Camera camera;
    private readonly Map map;
    private readonly Texture2D spriteSheet;
    private readonly SpriteBatch spriteBatch;

    public MapRenderer(Camera camera, Map map, Texture2D spriteSheet, SpriteBatch spriteBatch)
    {
        this.camera = camera;
        this.map = map;
        this.spriteSheet = spriteSheet;
        this.spriteBatch = spriteBatch;
    }

    public void Draw()
    {
        var tileSize = Camera.TileSize;

        spriteBatch.Begin();

        DrawMap(tileSize);

        Rectangle src;
        Rectangle dest;

        var mouseTile = camera.MouseTile;
        var viewOffset = camera.ViewOffset;

        src = new Rectangle(tileSize, 0, tileSize, tileSize);
        dest = new Rectangle(mouseTile.X * tileSize - viewOffset.X, mouseTile.Y * tileSize - viewOffset.Y, tileSize, tileSize);
        spriteBatch.Draw(spriteSheet, dest, src, Color.FromNonPremultiplied(0, 0, 0, 128));

        spriteBatch.End();
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
        spriteBatch.Draw(spriteSheet, dest, src, Color.White);

        // Shore draw
        if (tile.Terrain == Terrain.Water)
        {
            var neighbors = map.GetNonWaterNeighbors(x, y);
            src = new Rectangle((int)neighbors * tileSize, 3 * tileSize, tileSize, tileSize);
            spriteBatch.Draw(spriteSheet, dest, src, Color.White);
        }

        // Grid overlay draw
        src = new Rectangle(5 * tileSize, 0, tileSize, tileSize);
        spriteBatch.Draw(spriteSheet, dest, src, Color.FromNonPremultiplied(0, 0, 0, 32));
    }
}
