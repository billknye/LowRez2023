using Microsoft.Xna.Framework;

namespace Billknye.GameLib.States;

public interface IGameStateManager
{
    void Draw(GameTime gameTime);
    TState EnterState<TState>(object[]? parameters = null) where TState : GameState;
    void ExitState<TState>(TState exitingState) where TState : GameState;
    void Update(GameTime gameTime);
}

