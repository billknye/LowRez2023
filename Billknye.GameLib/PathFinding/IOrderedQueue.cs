using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billknye.GameLib.PathFinding;

public interface IOrderedQueue<T> where T : IComparable<T>
{
    bool HasValues
    {
        get;
    }
    void Enqueue(T value);
    T Dequeue();
    void Clear();
}
