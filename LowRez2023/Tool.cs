using LowRez2023.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LowRez2023;

internal sealed class Tool
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly TextRenderer textRenderer;
    private readonly Camera camera;
    private readonly Map map;

    private Point? toolTileStart;
    private Point? toolTileCurrent;

    public Tool(SpriteRenderer spriteRenderer, TextRenderer textRenderer, Camera camera, Map map)
    {
        this.spriteRenderer = spriteRenderer;
        this.textRenderer = textRenderer;
        this.camera = camera;
        this.map = map;
    }

    public void Update()
    {
        if (camera.LastMouse.LeftButton == ButtonState.Released
            && camera.CurrentMouse.LeftButton == ButtonState.Pressed)
        {
            // Mouse down, begin use tool
            toolTileStart = camera.MouseTile;
            toolTileCurrent = camera.MouseTile;
        }
        else if (camera.CurrentMouse.LeftButton == ButtonState.Released
            && camera.LastMouse.LeftButton == ButtonState.Pressed)
        {
            // Mouse up, end use tool
            if (TryGetToolBounds(out var bounds))
            {
                for (int x = bounds.X; x <= bounds.Right; x++)
                {
                    for (int y = bounds.Y; y <= bounds.Bottom; y++)
                    {
                        map[x, y].Thing = true;
                    }
                }
            }

            toolTileCurrent = null;
            toolTileStart = null;
        }
        else if (camera.CurrentMouse.LeftButton == ButtonState.Pressed)
        {
            // Move?
            toolTileCurrent = camera.MouseTile;
        }
    }

    private bool TryGetToolBounds(out Rectangle bounds)
    {
        if (toolTileCurrent is null || toolTileStart is null)
        {
            bounds = default;
            return false;
        }

        var minX = MathHelper.Min(toolTileStart.Value.X, toolTileCurrent.Value.X);
        var minY = MathHelper.Min(toolTileStart.Value.Y, toolTileCurrent.Value.Y);
        var maxX = MathHelper.Max(toolTileStart.Value.X, toolTileCurrent.Value.X);
        var maxY = MathHelper.Max(toolTileStart.Value.Y, toolTileCurrent.Value.Y);

        bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        return true;
    }

    public void Draw()
    {
        if (!TryGetToolBounds(out Rectangle bounds))
            return;

        var color = Color.FromNonPremultiplied(255, 255, 255, 64);
        var src = new Rectangle(0, 0, 5, 5);

        for (int x = bounds.X; x <= bounds.Right; x++)
        {
            for (int y = bounds.Y; y <= bounds.Bottom; y++)
            {
                var dest = new Rectangle(x * 5 - camera.ViewOffset.X, y * 5 - camera.ViewOffset.Y, 5, 5);
                spriteRenderer.DrawSprite(src, dest, color);
            }
        }

    }
}
