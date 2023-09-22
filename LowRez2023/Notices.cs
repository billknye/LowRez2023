using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LowRez2023;

internal sealed class Notices
{
    private readonly TextRenderer textRenderer;
    private readonly List<NoticeEntry> entries;

    public Notices(TextRenderer textRenderer)
    {
        this.textRenderer = textRenderer;
        entries = new List<NoticeEntry>();
    }

    public void PostNotice(string text)
    {
        entries.Insert(0, new NoticeEntry { Text = text, RemainingTime = 2.0 });
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            entry.RemainingTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (entry.RemainingTime <= 0)
            {
                entries.RemoveAt(i);
                i--;
            }
            else
            {
                entries[i] = entry;
            }
        }
    }

    public void Draw()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            Color color = Color.White;
            var entry = entries[i];
            if (entry.RemainingTime < 1)
            {
                color = Color.FromNonPremultiplied(255, 255, 255, (int)(255 * entry.RemainingTime));
            }
            textRenderer.DrawText(new Point(0, 57 - i * 8), entry.Text, color);
        }
    }

    struct NoticeEntry
    {
        public string Text;
        public double RemainingTime;
    }
}