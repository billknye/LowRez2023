using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Billknye.GameLib.PathFinding;

public class ManhattenPathFindingHeuristic : IPathFindingHeuristic<Point>
{
    #region IPathFindingHeuristic Members

    public int GetHeuristic(Point start, Point end)
    {
        return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
    }

    #endregion

    public static int GetHeuristic(int sx, int sy, int ex, int ey)
    {
        return (Math.Abs(sx - ex) + (Math.Abs(sy - ey)));
    }
}
