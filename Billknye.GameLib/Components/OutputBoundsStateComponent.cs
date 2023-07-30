using Microsoft.Xna.Framework;

namespace Billknye.GameLib.Components;

/// <summary>
/// Provides shared access to the window relative output bounds.
/// </summary>
public sealed class OutputBoundsStateComponent
{
    public Rectangle WindowRelativeOutputBounds { get; set; }
}
