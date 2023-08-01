using LowRez2023.Simulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
namespace LowRez2023;

internal sealed class Camera
{
    public const int OutputSize = 64;
    public const int TileSize = 5;

    private readonly GameWindow gameWindow;
    private readonly Map map;
    private MouseState lastMouse;
    private Point? mouseCapturePoint;
    private Point mouseMoveAccumulator;
    private bool mouseMoved;

    public int PixelScale { get; private set; }

    public Rectangle OutputBounds { get; private set; }

    public Point ViewOffset { get; private set; }

    public Rectangle VisibleTiles { get; private set; }

    public Point MouseTile { get; private set; }

    public Point MouseVirtualPixel { get; private set; }

    public Camera(GameWindow gameWindow, Map map)
    {
        this.gameWindow = gameWindow;
        this.map = map;
    }

    public void Update()
    {
        var minSize = Math.Min(gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
        PixelScale = (int)Math.Floor(minSize / (float)OutputSize);

        var drawSize = OutputSize * PixelScale;
        var horizontalPadding = (gameWindow.ClientBounds.Width - drawSize) / 2;
        var verticalPadding = (gameWindow.ClientBounds.Height - drawSize) / 2;
        OutputBounds = new Rectangle(horizontalPadding, verticalPadding, drawSize, drawSize);

        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released)
        {
            mouseCapturePoint = new Point(mouseState.X, mouseState.Y);
            mouseMoveAccumulator = new Point();
            mouseMoved = false;
        }
        else if (mouseState.LeftButton == ButtonState.Pressed && mouseCapturePoint != null)
        {
            var diff = new Point(lastMouse.X - mouseState.X, lastMouse.Y - mouseState.Y);
            mouseMoveAccumulator += diff;

            var moved = new Point(mouseMoveAccumulator.X / PixelScale, mouseMoveAccumulator.Y / PixelScale);

            mouseMoved |= moved != default;
            if (moved != default)
            {
                var viewOffset = ViewOffset;
                viewOffset += moved;
                mouseMoveAccumulator -= new Point(moved.X * PixelScale, moved.Y * PixelScale);

                viewOffset.X = Math.Max(0, viewOffset.X);
                viewOffset.Y = Math.Max(0, viewOffset.Y);

                viewOffset.X = Math.Min(map.Width * TileSize - OutputSize, viewOffset.X);
                viewOffset.Y = Math.Min(map.Height * TileSize - OutputSize, viewOffset.Y);

                ViewOffset = viewOffset;
            }
        }
        else if (mouseCapturePoint != null && mouseState.LeftButton == ButtonState.Released)
        {
            mouseCapturePoint = null;
        }

        var mouseInOutputArea = OutputBounds.Contains(mouseState.X, mouseState.Y);
        if (mouseInOutputArea)
        {
            var mouseOutputRelativeInPixels = new Point(mouseState.X - OutputBounds.X, mouseState.Y - OutputBounds.Y);
            MouseVirtualPixel = new Point((int)Math.Floor(mouseOutputRelativeInPixels.X / (double)PixelScale) + ViewOffset.X, (int)Math.Floor(mouseOutputRelativeInPixels.Y / (double)PixelScale + ViewOffset.Y));

            MouseTile = new Point((int)Math.Floor(MouseVirtualPixel.X / (double)TileSize), (int)Math.Floor(MouseVirtualPixel.Y / (double)TileSize));
        }

        lastMouse = mouseState;

        var minx = ViewOffset.X / TileSize;
        var miny = ViewOffset.Y / TileSize;
        var maxx = (int)Math.Ceiling((ViewOffset.X + OutputSize) / (double)TileSize);
        var maxy = (int)Math.Ceiling((ViewOffset.Y + OutputSize) / (double)TileSize);

        VisibleTiles = new Rectangle(minx, miny, maxx - minx, maxy - miny);
    }
}
