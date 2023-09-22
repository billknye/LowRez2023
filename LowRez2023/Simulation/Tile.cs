using System;

namespace LowRez2023.Simulation;

public struct Tile
{
    private uint lotOrZoneData;

    public Terrain Terrain { get; set; }

    public Improvement Improvement { get; set; }

    public LotReference Lot
    {
        get => Improvement == Improvement.Lot ? new LotReference(lotOrZoneData) : default;
        set
        {
            if (Improvement == Improvement.Lot && value.Data != 0)
                lotOrZoneData = value.Data;
            else
                throw new InvalidOperationException("Tile must be a lot to have lot ref set and lot ref must not be default.");
        }
    }
}
