using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billknye.GameLib.PathFinding;

public interface IPathFindingHeuristic<T>
{
    int GetHeuristic(T start, T end);
}
