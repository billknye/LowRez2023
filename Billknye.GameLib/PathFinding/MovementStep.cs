using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billknye.GameLib.PathFinding;

public class MovementStep<T> : IComparable<MovementStep<T>>, IEquatable<MovementStep<T>>
{
    public MovementStep<T> Parent;
    public T Location;

    public int PreCost;
    public int PostCost;

    public int TotalCost
    {
        get
        {
            return PreCost + PostCost;
        }
    }

    #region IComparable<MovementStep> Members

    public int CompareTo(MovementStep<T> other)
    {
        return TotalCost.CompareTo(other.TotalCost);
    }

    #endregion

    #region IEquatable<MovementStep> Members

    public bool Equals(MovementStep<T> other)
    {
        return Location.Equals(other);
    }

    #endregion

    public override int GetHashCode()
    {
        return Location.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is MovementStep<T>)
        {
            return Location.Equals(obj);
        }

        return false;
    }
}
