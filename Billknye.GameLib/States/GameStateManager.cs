using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib.States;

/// <summary>
/// Manages the stack of states for the game.
/// </summary>
internal sealed class GameStateManager : IGameStateManager, IDisposable
{
    private readonly Stack<GameStateStackElement> states;
    private readonly IServiceProvider serviceProvider;

    public GameState? CurrentState => states.Count == 0 ? null : states.Peek().State;

    private GameStateStackElement? Current => states.Count == 0 ? default : states.Peek();

    public GameStateManager(IServiceProvider serviceProvider)
    {
        states = new Stack<GameStateStackElement>();
        this.serviceProvider = serviceProvider;
    }

    public TState EnterState<TState>(object[]? parameters = null)
        where TState : GameState
    {
        var previous = Current;
        if (previous != null)
        {
            previous.State.StatePaused();
        }

        var scope = serviceProvider.CreateScope();
        var state = ActivatorUtilities.CreateInstance<TState>(scope.ServiceProvider, parameters ?? Array.Empty<object>());

        states.Push(new GameStateStackElement(state, scope));
        state.StateEntered();
        return state;
    }

    public void ExitState<TState>(TState exitingState)
        where TState : GameState
    {
        var exiting = Current;

        if (exitingState != exiting?.State)
        {
            throw new InvalidOperationException();
        }

        var popped = states.Pop();
        if (popped != exiting)
        {
            throw new InvalidOperationException();
        }

        exiting.State.StateExited();
        exiting.Scope.Dispose();

        if (exiting.State is IDisposable disposable)
            disposable.Dispose();

        exiting = Current;
        if (exiting != null)
        {
            exiting.State.StateResumed();
        }
    }

    public void Update(GameTime gameTime)
    {
        CurrentState?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        CurrentState?.Draw(gameTime);
    }

    public void Dispose()
    {
        var current = CurrentState;
        while (current != null)
        {
            ExitState(current);
            current = CurrentState;
        }
    }

    private record GameStateStackElement(GameState State, IServiceScope Scope);
}

