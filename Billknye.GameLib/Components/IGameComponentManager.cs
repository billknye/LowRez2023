using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

public interface IGameComponentManager
{
    public IDisposable RegisterForUpdates(Action<GameTime> update);
    public IDisposable RegisterForDraw(Action<GameTime> draw);

    public IDisposable RegisterForBeforeDraw(Action<GameTime> beforeDraw);
    public IDisposable RegisterForAfterDraw(Action<GameTime> afterDraw);

    public void Update(GameTime gameTime);

    public void Draw(GameTime gameTime);
}

