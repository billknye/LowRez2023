using Microsoft.Xna.Framework;

namespace LowRez2023;

public class TextRenderer
{
    private readonly SpriteRenderer spriteRenderer;

    public TextRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
    }

    public void DrawText(Point topLeft, string text, Color? color = null)
    {
        var drawColor = color ?? Color.White;

        Rectangle dest = new Rectangle(topLeft, new Point(5, 7));

        foreach (var ch in text)
        {
            spriteRenderer.DrawSprite(new Rectangle((ch % 32) * 5, 128 + (ch / 32) * 7, 5, 7), dest, drawColor);
            dest.X += 4;
        }
    }
}