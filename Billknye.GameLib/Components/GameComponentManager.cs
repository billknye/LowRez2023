using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

internal sealed class GameComponentManager : IGameComponentManager
{
    private readonly List<Action<GameTime>> updates;
    private readonly List<Action<GameTime>> draws;
    private readonly List<Action<GameTime>> beforeDraws;
    private readonly List<Action<GameTime>> afterDraws;

    public GameComponentManager()
    {
        updates = new List<Action<GameTime>>();
        draws = new List<Action<GameTime>>();
        beforeDraws = new List<Action<GameTime>>();
        afterDraws = new List<Action<GameTime>>();
    }

    public void Draw(GameTime gameTime)
    {
        for (int i = 0; i < beforeDraws.Count; i++)
        {
            beforeDraws[i].Invoke(gameTime);
        }

        for (int i = 0; i < draws.Count; i++)
        {
            draws[i].Invoke(gameTime);
        }

        for (int i = 0; i < afterDraws.Count; i++)
        {
            afterDraws[i].Invoke(gameTime);
        }
    }

    public IDisposable RegisterForAfterDraw(Action<GameTime> afterDraw)
    {
        var registration = new GameComponentRegistration(() => afterDraws.Remove(afterDraw));
        afterDraws.Add(afterDraw);
        return registration;
    }

    public IDisposable RegisterForBeforeDraw(Action<GameTime> beforeDraw)
    {
        var registration = new GameComponentRegistration(() => beforeDraws.Remove(beforeDraw));
        beforeDraws.Add(beforeDraw);
        return registration;
    }

    public IDisposable RegisterForDraw(Action<GameTime> draw)
    {
        var registration = new GameComponentRegistration(() => draws.Remove(draw));
        draws.Add(draw);
        return registration;
    }

    public IDisposable RegisterForUpdates(Action<GameTime> update)
    {
        var registration = new GameComponentRegistration(() => updates.Remove(update));
        updates.Add(update);
        return registration;
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < updates.Count; i++)
        {
            updates[i].Invoke(gameTime);
        }
    }

    internal void RemoveDraw(Action<GameTime> action)
    {
        draws.Remove(action);
    }

    internal void RemoveUpdate(Action<GameTime> action)
    {
        updates.Remove(action);
    }

    internal sealed class GameComponentRegistration : IDisposable
    {
        private readonly Action deregistration;

        public GameComponentRegistration(Action deregistration)
        {
            this.deregistration = deregistration;
        }

        public void Dispose()
        {
            deregistration();
        }
    }
}

