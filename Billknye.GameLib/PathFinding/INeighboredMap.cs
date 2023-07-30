using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billknye.GameLib.PathFinding;

/// <summary>
/// Provides a standard way for maps to provide neighbor node information for path finding
/// </summary>
public interface INeighboredMap<LocationType, ObjectType>
{
    /// <summary>
    /// Returns a set of neighboring nodes for a given node with the applicable object or movement properties and the previous node in this path if significant
    /// </summary>
    /// <param name="previous"></param>
    /// <param name="p"></param>
    /// <param name="objectOrMoveProps"></param>
    /// <returns></returns>
    IEnumerable<NeighborProperties<LocationType>> GetNeighbors(MovementStep<LocationType> previous, LocationType location, ObjectType objectOrMoveProps);
}
