using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billknye.GameLib.PathFinding;

public struct NeighborProperties<T>
{
    /// <summary>
    /// Location of the neighbor
    /// </summary>
    public T Location;
    /// <summary>
    /// Cost of movement to this neighbor
    /// </summary>
    public int Cost;

    public NeighborProperties(T location, int cost)
    {
        this.Location = location;
        this.Cost = cost;
    }
}
