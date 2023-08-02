using Billknye.GameLib.Noise;
using System;

namespace LowRez2023.Simulation;

public class Map
{
    private readonly Tile[] tiles;
    private readonly int width, height;

    public int Width => width;

    public int Height => height;

    public ref Tile this[int x, int y]
    {
        get
        {
            return ref tiles[x + y * Width];
        }
    }

    public Neighbors GetNonWaterNeighbors(int x, int y)
        => GetNeighbors(x, y, t => t.Terrain != Terrain.Water);

    public Neighbors GetRoadNeighbors(int x, int y)
        => GetNeighbors(x, y, t => t.Improvement == Improvement.Road);

    public Neighbors GetNeighbors(int x, int y, Func<Tile, bool> predicate)
    {
        var neighbors = Neighbors.None;

        if (y > 0 && predicate(this[x, y - 1]))
            neighbors |= Neighbors.North;

        if (y < Height - 1 && predicate(this[x, y + 1]))
            neighbors |= Neighbors.South;

        if (x > 0 && predicate(this[x - 1, y]))
            neighbors |= Neighbors.West;

        if (x < Width - 1 && predicate(this[x + 1, y]))
            neighbors |= Neighbors.East;

        return neighbors;
    }

    public Map(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new Tile[width * height];

        var noise = new WrappingPerlinNoise2D(123, 0.05);
        noise.WrapWidth = width * 256;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var elevation = noise.GetValue(x, y);

                if (elevation < 0.1)
                {
                    this[x, y].Terrain = Terrain.Water;
                }
            }
        }

    }
}
