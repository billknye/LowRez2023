namespace LowRez2023.Simulation;

public struct Bits
{
    private uint bits;

    public readonly uint Data => bits;

    public Bits(uint data)
    {
        bits = data;
    }

    public readonly struct Section
    {
        private readonly byte offsetBits;
        private readonly byte lengthBits;
        private readonly uint valueMask;
        private readonly uint excludeMask;

        public Section(byte offsetBits, byte lengthBits)
        {
            this.offsetBits = offsetBits;
            this.lengthBits = lengthBits;
            this.valueMask = (1u << offsetBits << lengthBits) - 1u;
            this.excludeMask = ~(valueMask << offsetBits);
        }

        public uint GetValue(Bits bits)
        {
            return (bits.bits >> offsetBits) & valueMask;
        }

        public Bits SetValue(Bits bits, uint value)
        {
            return new Bits(bits.bits = bits.bits & excludeMask | (value & valueMask) << offsetBits);
        }
    }
}