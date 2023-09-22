using LowRez2023.Simulation;

namespace LowRez2023.Tests.Simulation;

public class BitsTests
{
    [Fact]
    public void Test1()
    {
        var section1 = new Bits.Section(30, 2);

        var a = new Bits();

        a = section1.SetValue(a, 3);

        Console.WriteLine();
    }
}