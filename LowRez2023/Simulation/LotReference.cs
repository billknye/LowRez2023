using Microsoft.Xna.Framework;
using System;

namespace LowRez2023.Simulation;

public struct LotReference
{
    private Bits bits;

    // ID field at 22 bits would allow ~4 million lots, or 2,048 x 2,048 if every tile had its own lot
    private static readonly Bits.Section idField = new Bits.Section(0, 22);
    // Might be able to get rid of x and y and just have boarder flags instead, for drawing?
    private static readonly Bits.Section xField = new Bits.Section(22, 3);
    private static readonly Bits.Section yField = new Bits.Section(25, 3);
    private static readonly Bits.Section orientation = new Bits.Section(28, 2);
    private static readonly Bits.Section flags = new Bits.Section(30, 1);
    private static readonly Bits.Section residentialCommercial = new Bits.Section(31, 1);

    public readonly uint Data => bits.Data;

    public LotReference(uint data)
    {
        this.bits = new Bits(data);
    }

    public uint LotId
    {
        get => idField.GetValue(bits);
        set => bits = idField.SetValue(bits, value);
    }

    public Point OffsetWithinLot
    {
        get => new Point((int)xField.GetValue(bits), (int)yField.GetValue(bits));
        set
        {
            if (value.X >= 8 || value.Y >= 8 || value.X < 0 || value.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "X and Y must be between 0 and 7.");

            bits = xField.SetValue(bits, (uint)value.X);
        }
    }

    public Orientation Orientation
    {
        get => (Orientation)orientation.GetValue(bits);
        set => bits = orientation.SetValue(bits, (uint)value);
    }

    public LotFlags Flags
    {
        get => (LotFlags)flags.GetValue(bits);
        set => bits = flags.SetValue(bits, (uint)value);
    }

    public LotType LotType
    {
        get => (LotType)residentialCommercial.GetValue(bits);
        set => bits = residentialCommercial.SetValue(bits, (uint)value);
    }
}
