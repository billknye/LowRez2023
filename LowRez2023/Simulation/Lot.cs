using Microsoft.Xna.Framework;

namespace LowRez2023.Simulation;

public class Lot
{
    /// <summary>
    /// Gets the origin point of the lot, this is the north-west corner of the lot.
    /// </summary>
    public Point Origin { get; }

    /// <summary>
    /// Gets the size of the lot.
    /// </summary>
    public Point Size { get; }

    /// <summary>
    /// Gets the type of lot represented.
    /// </summary>
    public LotType Type { get; }

    /// <summary>
    /// Gets the orientation of the lot.
    /// </summary>
    public Orientation Orientation { get; }

    public Lot(Point origin, Point size, LotType type, Orientation orientation)
    {
        Origin = origin;
        Size = size;
        Type = type;
        Orientation = orientation;
    }
}
