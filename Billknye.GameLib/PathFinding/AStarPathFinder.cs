namespace Billknye.GameLib.PathFinding;

/// <summary>
/// A basic implementation of the A* path finding algorithm.  
/// Relies on T.Equals(T) to determine if a point is the end point or not.
/// </summary>
/// <typeparam name="LocationType"></typeparam>
/// <typeparam name="ObjectType"></typeparam>
public class AStarPathFinder<LocationType, ObjectType> : IPathFinder<LocationType, ObjectType>
{
    INeighboredMap<LocationType, ObjectType> world;
    IPathFindingHeuristic<LocationType> heuristic;

    Dictionary<LocationType, MovementStep<LocationType>> visited = new Dictionary<LocationType, MovementStep<LocationType>>();
    IOrderedQueue<MovementStep<LocationType>> openQueue;

    MovementStep<LocationType> endStep = null;
    ObjectType entity;
    LocationType end;
    LocationType start;

    public int Id { get; set; }

    public AStarPathFinder(INeighboredMap<LocationType, ObjectType> world, IPathFindingHeuristic<LocationType> heuristic, IOrderedQueue<MovementStep<LocationType>> queue)
    {
        this.world = world;
        this.heuristic = heuristic;
        this.openQueue = queue;
    }

    public void Initialize(ObjectType objectOrMovementProps, LocationType start, LocationType end)
    {
        this.entity = objectOrMovementProps;
        this.start = start;
        this.end = end;

        MovementStep<LocationType> startStep = new MovementStep<LocationType>();
        startStep.Location = start;
        startStep.PreCost = 0;
        startStep.PostCost = heuristic.GetHeuristic(start, end);

        openQueue.Clear();
        visited.Clear();
        openQueue.Enqueue(startStep);
        visited.Add(start, startStep);
    }

    public Stack<LocationType> GetPath()
    {
        var stack = new Stack<LocationType>();
        var step = endStep;
        while (step.Parent != null)
        {
            stack.Push(step.Location);
            step = step.Parent;
        }

        return stack;
    }

    public PathFindingResult Step()
    {
        if (!openQueue.HasValues)
        {
            return PathFindingResult.CantFind;
        }

        MovementStep<LocationType> current = openQueue.Dequeue(); // openList.Pop();

        foreach (var tile in world.GetNeighbors(current.Parent, current.Location, entity))
        {
            if (tile.Location.Equals(end))
            {
                MovementStep<LocationType> step = new MovementStep<LocationType>();
                step.Parent = current;
                step.Location = tile.Location;
                step.PreCost = current.PreCost + tile.Cost;
                step.PostCost = 0;
                endStep = step;

                return PathFindingResult.Found;
            }

            if (!visited.ContainsKey(tile.Location))
            {
                MovementStep<LocationType> step = new MovementStep<LocationType>();
                step.Parent = current;
                step.Location = tile.Location;
                step.PreCost = current.PreCost + tile.Cost;
                step.PostCost = heuristic.GetHeuristic(tile.Location, end);
                openQueue.Enqueue(step);
                visited.Add(tile.Location, step);
            }
        }

        return PathFindingResult.Working;
    }

    public LocationType GetNextStep()
    {
        var ret = endStep;
        endStep = endStep.Parent;
        return ret.Location;
    }

    public PathFindingResult Steps(int num)
    {
        PathFindingResult cur = PathFindingResult.Working;

        int n = 0;

        while (n < num)
        {
            cur = Step();

            if (cur == PathFindingResult.Found)
            {
                return cur;
            }
            else if (cur == PathFindingResult.CantFind)
            {
                return cur;
            }
            n++;
        }

        return cur;
    }

    public PathFindingResult StepToEnd()
    {
        PathFindingResult cur = Step();
        while (cur == PathFindingResult.Working)
        {
            cur = Step();
        }

        return cur;
    }
}
