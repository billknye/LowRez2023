using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billknye.GameLib.PathFinding;

public interface IPathFinder<T, N>
{
    void Initialize(N objectOrMoveProps, T start, T end);
    PathFindingResult Step();
    PathFindingResult Steps(int num);
    PathFindingResult StepToEnd();
    Stack<T> GetPath();
}
