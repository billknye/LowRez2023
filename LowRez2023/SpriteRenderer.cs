using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LowRez2023;

public class SpriteRenderer
{
    private readonly SpriteBatch spriteBatch;
    private readonly Texture2D spriteSheet;

    public SpriteRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet)
    {
        this.spriteBatch = spriteBatch;
        this.spriteSheet = spriteSheet;
    }

    public void DrawSprite(Rectangle source, Rectangle destination, Color color)
    {
        spriteBatch.Draw(spriteSheet, destination, source, color);
    }
}
