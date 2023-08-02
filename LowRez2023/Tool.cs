﻿using LowRez2023.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LowRez2023;

internal sealed class Tool
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly TextRenderer textRenderer;
    private readonly Camera camera;
    private readonly Map map;

    private readonly RoadToolMode roadTool;

    private ToolMode? toolMode;

    public Tool(SpriteRenderer spriteRenderer, TextRenderer textRenderer, Camera camera, Map map)
    {
        this.spriteRenderer = spriteRenderer;
        this.textRenderer = textRenderer;
        this.camera = camera;
        this.map = map;

        roadTool = new RoadToolMode(map);
        toolMode = roadTool;
    }

    public void Update()
    {
        if (camera.LastMouse.LeftButton == ButtonState.Released
            && camera.CurrentMouse.LeftButton == ButtonState.Pressed)
        {
            // Mouse down, begin use tool
            toolMode = roadTool;
            toolMode.BeginOperation(camera.MouseTile);
        }
        else if (camera.CurrentMouse.LeftButton == ButtonState.Released
            && camera.LastMouse.LeftButton == ButtonState.Pressed
            && toolMode is not null)
        {
            // Mouse up, end use tool
            toolMode.TryCompleteOperation(camera.MouseTile);
            toolMode = null;
        }
        else if (camera.CurrentMouse.LeftButton == ButtonState.Pressed
            && toolMode is not null)
        {
            // Move?
            toolMode.UpdateOperation(camera.MouseTile);
        }
    }

    public void Draw()
    {
        if (toolMode == null)
            return;

        var bounds = toolMode.GetToolBounds();
        var src = new Rectangle(0, 0, 5, 5);

        for (int x = bounds.X; x < bounds.Right; x++)
        {
            for (int y = bounds.Y; y < bounds.Bottom; y++)
            {
                var dest = new Rectangle(x * 5 - camera.ViewOffset.X, y * 5 - camera.ViewOffset.Y, 5, 5);

                var tile = map[x, y];
                var color = toolMode.GetOverlayColor(tile);
                spriteRenderer.DrawSprite(src, dest, color);
            }
        }
    }
}

internal abstract class ToolMode
{
    protected Point tileStart;
    protected Point tileCurrent;

    public virtual Color GetOverlayColor(Tile tile)
    {
        return Color.FromNonPremultiplied(255, 255, 255, 64);
    }

    public Rectangle GetToolBounds()
    {
        var minX = MathHelper.Min(tileStart.X, tileCurrent.X);
        var minY = MathHelper.Min(tileStart.Y, tileCurrent.Y);
        var maxX = MathHelper.Max(tileStart.X, tileCurrent.X);
        var maxY = MathHelper.Max(tileStart.Y, tileCurrent.Y);

        return GetToolBoundsInternal(minX, minY, maxX, maxY);
    }

    public virtual Rectangle GetToolBoundsInternal(int minX, int minY, int maxX, int maxY)
    {
        return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }

    public virtual void BeginOperation(Point tileStart)
    {
        this.tileStart = tileStart;
        this.tileCurrent = tileStart;
    }

    public virtual void UpdateOperation(Point tileCurrent)
    {
        this.tileCurrent = tileCurrent;
    }

    public virtual void CancelOperation()
    {
        tileStart = default;
        tileCurrent = default;
    }

    public abstract bool TryCompleteOperation(Point tileEnd);
}

internal sealed class RoadToolMode : ToolMode
{
    private readonly Map map;

    public RoadToolMode(Map map)
    {
        this.map = map;
    }

    public override Color GetOverlayColor(Tile tile)
    {
        if (tile.Terrain != Terrain.Grass)
        {
            return Color.FromNonPremultiplied(255, 0, b: 0, 64);
        }
        else
        {
            return Color.FromNonPremultiplied(128, 255, 128, 64);
        }
    }

    public override Rectangle GetToolBoundsInternal(int minX, int minY, int maxX, int maxY)
    {
        if (maxX - minX > maxY - minY)
        {
            return new Rectangle(minX, tileStart.Y, maxX - minX + 1, 1);
        }
        else
        {
            return new Rectangle(tileStart.X, minY, 1, maxY - minY + 1);
        }
    }

    public override bool TryCompleteOperation(Point tileEnd)
    {
        var bounds = GetToolBounds();
        for (int x = bounds.X; x < bounds.Right; x++)
        {
            for (int y = bounds.Y; y < bounds.Bottom; y++)
            {
                ref var tile = ref map[x, y];

                if (tile.Terrain == Terrain.Grass
                    && tile.Improvement == Improvement.None)
                {
                    tile.Improvement = Improvement.Road;
                }
            }
        }

        return false;
    }
}