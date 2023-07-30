using Billknye.GameLib.Noise;

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

    public WaterNeighbors GetNonWaterNeighbors(int x, int y)
    {
        var neighbors = WaterNeighbors.None;

        if (y > 0 && this[x, y - 1].Terrain != Terrain.Water)
            neighbors |= WaterNeighbors.North;

        if (y < Height - 1 && this[x, y + 1].Terrain != Terrain.Water)
            neighbors |= WaterNeighbors.South;

        if (x > 0 && this[x - 1, y].Terrain != Terrain.Water)
            neighbors |= WaterNeighbors.West;

        if (x < Width - 1 && this[x + 1, y].Terrain != Terrain.Water)
            neighbors |= WaterNeighbors.East;

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
