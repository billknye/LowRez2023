using Billknye.GameLib.Components;
using Billknye.GameLib.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace LowRez2023;

internal sealed class TestingGameState : GameState
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly FixedVirtualOutputComponent fixedVirtualOutputComponent;
    private readonly FixedVirtualOutputScaleBehaviorComponent fixedVirtualOutputScaleBehaviorComponent;
    private readonly FixedOutputBoundsBehaviorComponent fixedOutputBoundsBehaviorComponent;
    private readonly RenderMapComponent renderMapComponent;
    private readonly MousePanComponent mousePanComponent;
#pragma warning restore IDE0052 // Remove unread private members


    public TestingGameState(
        FixedVirtualOutputComponent fixedVirtualOutputComponent,
        FixedVirtualOutputScaleBehaviorComponent fixedVirtualOutputScaleBehaviorComponent,
        FixedOutputBoundsBehaviorComponent fixedOutputBoundsBehaviorComponent,
        RenderMapComponent renderMapComponent,
        MousePanComponent mousePanComponent
        )
    {
        this.fixedVirtualOutputComponent = fixedVirtualOutputComponent;
        // Request behavior to setup fixed scale behavior.
        this.fixedVirtualOutputScaleBehaviorComponent = fixedVirtualOutputScaleBehaviorComponent;
        this.fixedOutputBoundsBehaviorComponent = fixedOutputBoundsBehaviorComponent;

        // Request render map component for map rendering;
        this.renderMapComponent = renderMapComponent;
        this.mousePanComponent = mousePanComponent;
    }

    protected override void StateEnteredInternal()
    {
        base.StateEnteredInternal();

    }

    protected override void UpdateInternal(GameTime gameTime)
    {
    }

    protected override void DrawInternal(GameTime gameTime)
    {

    }
}

internal sealed class MousePanComponent : IDisposable
{
    private readonly IGameComponentManager gameComponentManager;
    private readonly MouseStateComponent mouseStateComponent;
    private readonly ViewOffsetStateComponent viewOffsetStateComponent;
    private readonly OutputScaleStateComponent outputScaleStateComponent;
    IDisposable updateRegistration;


    private Point? mouseCapturePoint;
    private Point mouseMoveAccumulator;
    private bool mouseMoved;


    public MousePanComponent(
        IGameComponentManager gameComponentManager,
        MouseStateComponent mouseStateComponent,
        ViewOffsetStateComponent viewOffsetStateComponent,
        OutputScaleStateComponent outputScaleStateComponent)
    {
        updateRegistration = gameComponentManager.RegisterForUpdates(Update);
        this.gameComponentManager = gameComponentManager;
        this.mouseStateComponent = mouseStateComponent;
        this.viewOffsetStateComponent = viewOffsetStateComponent;
        this.outputScaleStateComponent = outputScaleStateComponent;
    }

    public void Dispose()
    {
        updateRegistration.Dispose();
    }

    private void Update(GameTime gameTime)
    {
        var pixelScale = outputScaleStateComponent.PixelScale;
        var mouseState = mouseStateComponent.CurrentState;
        var lastMouse = mouseStateComponent.LastState;

        if (mouseState.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released)
        {
            mouseCapturePoint = new Point(mouseState.X, mouseState.Y);
            mouseMoveAccumulator = new Point();
            mouseMoved = false;
        }
        else if (mouseState.LeftButton == ButtonState.Pressed && mouseCapturePoint != null)
        {
            var diff = new Point(lastMouse.X - mouseState.X, lastMouse.Y - mouseState.Y);
            mouseMoveAccumulator += diff;

            var moved = new Point(mouseMoveAccumulator.X / pixelScale, mouseMoveAccumulator.Y / pixelScale);

            mouseMoved |= moved != default;
            if (moved != default)
            {
                var viewOffset = viewOffsetStateComponent.ViewOffset;
                viewOffset += moved;
                mouseMoveAccumulator -= new Point(moved.X * pixelScale, moved.Y * pixelScale);

                viewOffset.X = Math.Max(0, viewOffset.X);
                viewOffset.Y = Math.Max(0, viewOffset.Y);
                viewOffset.X = Math.Min(256 * 5 - 64, viewOffset.X);
                viewOffset.Y = Math.Min(256 * 5 - 64, viewOffset.Y);

                viewOffsetStateComponent.ViewOffset = viewOffset;
            }
        }
        else if (mouseCapturePoint != null && mouseState.LeftButton == ButtonState.Released)
        {
            mouseCapturePoint = null;
        }
    }
}
