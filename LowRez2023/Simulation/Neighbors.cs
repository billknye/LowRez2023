using System;

namespace LowRez2023.Simulation;

[Flags]
public enum Neighbors : byte
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8
}
