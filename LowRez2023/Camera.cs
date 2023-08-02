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

    private Point? mouseCapturePoint;
    private Point mouseMoveAccumulator;

    public int PixelScale { get; private set; }

    public Rectangle OutputBounds { get; private set; }

    public Point ViewOffset { get; private set; }

    public Rectangle VisibleTiles { get; private set; }

    public Point MouseTile { get; private set; }

    public Point MouseVirtualPixel { get; private set; }

    public MouseState CurrentMouse { get; private set; }

    public KeyboardState CurrentKeyboard { get; private set; }

    public MouseState LastMouse { get; private set; }

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
        var viewOffset = ViewOffset;

        CurrentKeyboard = Keyboard.GetState();
        if (CurrentKeyboard.IsKeyDown(Keys.W))
        {
            viewOffset.Y--;
        }
        if (CurrentKeyboard.IsKeyDown(Keys.A))
        {
            viewOffset.X--;
        }
        if (CurrentKeyboard.IsKeyDown(Keys.S))
        {
            viewOffset.Y++;
        }
        if (CurrentKeyboard.IsKeyDown(Keys.D))
        {
            viewOffset.X++;
        }

        LastMouse = CurrentMouse;
        CurrentMouse = Mouse.GetState();
        if (CurrentMouse.RightButton == ButtonState.Pressed && LastMouse.RightButton == ButtonState.Released)
        {
            mouseCapturePoint = new Point(CurrentMouse.X, CurrentMouse.Y);
            mouseMoveAccumulator = new Point();
        }
        else if (CurrentMouse.RightButton == ButtonState.Pressed && mouseCapturePoint != null)
        {
            var diff = new Point(LastMouse.X - CurrentMouse.X, LastMouse.Y - CurrentMouse.Y);
            mouseMoveAccumulator += diff;

            var moved = new Point(mouseMoveAccumulator.X / PixelScale, mouseMoveAccumulator.Y / PixelScale);

            if (moved != default)
            {
                viewOffset += moved;
                mouseMoveAccumulator -= new Point(moved.X * PixelScale, moved.Y * PixelScale);
            }
        }
        else if (mouseCapturePoint != null && CurrentMouse.RightButton == ButtonState.Released)
        {
            mouseCapturePoint = null;
        }

        if (viewOffset != ViewOffset)
        {
            viewOffset.X = Math.Max(0, viewOffset.X);
            viewOffset.Y = Math.Max(0, viewOffset.Y);

            viewOffset.X = Math.Min(map.Width * TileSize - OutputSize, viewOffset.X);
            viewOffset.Y = Math.Min(map.Height * TileSize - OutputSize, viewOffset.Y);

            ViewOffset = viewOffset;
        }

        var mouseInOutputArea = OutputBounds.Contains(CurrentMouse.X, CurrentMouse.Y);
        if (mouseInOutputArea)
        {
            var mouseOutputRelativeInPixels = new Point(CurrentMouse.X - OutputBounds.X, CurrentMouse.Y - OutputBounds.Y);
            MouseVirtualPixel = new Point((int)Math.Floor(mouseOutputRelativeInPixels.X / (double)PixelScale) + ViewOffset.X, (int)Math.Floor(mouseOutputRelativeInPixels.Y / (double)PixelScale + ViewOffset.Y));

            MouseTile = new Point((int)Math.Floor(MouseVirtualPixel.X / (double)TileSize), (int)Math.Floor(MouseVirtualPixel.Y / (double)TileSize));
        }

        var minx = ViewOffset.X / TileSize;
        var miny = ViewOffset.Y / TileSize;
        var maxx = (int)Math.Ceiling((ViewOffset.X + OutputSize) / (double)TileSize);
        var maxy = (int)Math.Ceiling((ViewOffset.Y + OutputSize) / (double)TileSize);

        VisibleTiles = new Rectangle(minx, miny, maxx - minx, maxy - miny);
    }
}
