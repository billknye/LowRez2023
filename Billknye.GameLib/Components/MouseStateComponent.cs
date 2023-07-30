using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides access to the current and last update mouse states.
/// </summary>
public sealed class MouseStateComponent : IDisposable
{
    private readonly IDisposable updateRegistration;

    public MouseState CurrentState { get; private set; }

    public MouseState LastState { get; private set; }

    public MouseStateComponent(IGameComponentManager gameComponentManager)
    {
        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
    }

    private void Update(GameTime gameTime)
    {
        LastState = CurrentState;
        CurrentState = Mouse.GetState();
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }
}
