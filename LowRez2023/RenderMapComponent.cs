using Billknye.GameLib;
using Billknye.GameLib.Components;
using LowRez2023.Simulation;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LowRez2023;

internal sealed class RenderMapComponent : IDisposable
{
    private readonly IDisposable drawRegistration;
    private readonly IGameComponentManager gameComponentManager;
    private readonly ViewOffsetStateComponent viewOffsetStateComponent;
    private readonly VisibleTileAreaComponent visibleTileAreaComponent;
    private readonly MouseTileComponent mouseTileComponent;
    private readonly IOptions<TilesOptions> tileOptions;
    private readonly SpriteBatch spriteBatch;
    private readonly Texture2D spriteSheet;

    private readonly Map map;

    public RenderMapComponent(
        GraphicsDevice graphicsDevice,
        ContentManager contentManager,
        IGameComponentManager gameComponentManager,
        ViewOffsetStateComponent viewOffsetStateComponent,
        VisibleTileAreaComponent visibleTileAreaComponent,
        MouseTileComponent mouseTileComponent,
        IOptions<TilesOptions> tileOptions)
    {
        drawRegistration = gameComponentManager.RegisterForDraw(Draw);
        this.gameComponentManager = gameComponentManager;
        this.viewOffsetStateComponent = viewOffsetStateComponent;
        this.visibleTileAreaComponent = visibleTileAreaComponent;
        this.mouseTileComponent = mouseTileComponent;
        this.tileOptions = tileOptions;
        spriteBatch = new SpriteBatch(graphicsDevice);
        spriteSheet = contentManager.Load<Texture2D>("Sprites");


        map = new Map(256, 256);

    }

    public void Dispose()
    {
        drawRegistration.Dispose();
        spriteSheet.Dispose();
        spriteBatch.Dispose();
    }

    private void Draw(GameTime gameTime)
    {
        var tileSize = tileOptions.Value.Size ?? throw new InvalidOperationException();

        spriteBatch.Begin();

        DrawMap(tileSize);

        Rectangle src;
        Rectangle dest;

        var mouseTile = mouseTileComponent.Location;
        var viewOffset = viewOffsetStateComponent.ViewOffset;

        src = new Rectangle(tileSize, 0, tileSize, tileSize);
        dest = new Rectangle(mouseTile.X * tileSize - viewOffset.X, mouseTile.Y * tileSize - viewOffset.Y, tileSize, tileSize);
        spriteBatch.Draw(spriteSheet, dest, src, Color.FromNonPremultiplied(0, 0, 0, 128));

        spriteBatch.End();
    }

    private void DrawMap(int tileSize)
    {
        var viewOffset = viewOffsetStateComponent.ViewOffset;
        var visibleTiles = visibleTileAreaComponent.VisibleTiles;

        for (int x = visibleTiles.X; x < visibleTiles.Right; x++)
        {
            for (int y = visibleTiles.Y; y < visibleTiles.Bottom; y++)
            {
                DrawTile(viewOffset, tileSize, map, x, y);
            }
        }
    }

    private void DrawTile(Point viewOffset, int tileSize, Map map, int x, int y)
    {
        Rectangle src;
        Rectangle dest;

        var tile = map[x, y];
        dest = new Rectangle(x * 5 - viewOffset.X, y * tileSize - viewOffset.Y, tileSize, tileSize);

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
