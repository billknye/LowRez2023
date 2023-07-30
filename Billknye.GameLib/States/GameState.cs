using Microsoft.Xna.Framework;

namespace Billknye.GameLib.States;

public abstract class GameState : IDisposable
{
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Update(GameTime gameTime)
    {
        UpdateInternal(gameTime);
    }

    protected virtual void UpdateInternal(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime)
    {
        DrawInternal(gameTime);
    }

    protected virtual void DrawInternal(GameTime gameTime)
    {

    }

    public void StateEntered()
    {
        StateEnteredInternal();
    }

    public void StateExited()
    {
        StateExitedInternal();
    }

    public void StatePaused()
    {
        StatePausedInternal();
    }

    public void StateResumed()
    {
        StateResumedInternal();
    }

    protected virtual void StateEnteredInternal()
    {

    }

    protected virtual void StateExitedInternal()
    {

    }

    protected virtual void StatePausedInternal()
    {

    }

    protected virtual void StateResumedInternal()
    {

    }
}

