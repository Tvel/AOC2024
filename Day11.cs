using Xunit.Abstractions;

namespace AOC2024;

public class Day11
{
    private readonly ITestOutputHelper testOutput;

    public Day11(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }

    [Theory]
    [InlineData(Inputs.Sample, 55312)]
    [InlineData(Inputs.Input, 189092)]
    public void Part1(string inputString, long expected)
    {
        string[] initialStones = inputString.Split(' ');

        List<List<string>> blinks = [];
        blinks.Add(initialStones.ToList());
        for (int i = 1; i < 26; i++)
        {
            List<string> blink = [];
            foreach (string stone in blinks[i - 1])
            {
                if (stone is "0") blink.Add("1");
                else if (stone.Length % 2 == 0)
                {

                    string firstHalf = ulong.Parse(stone[..(stone.Length / 2)]).ToString();
                    string secondHalf = ulong.Parse(stone[(stone.Length / 2)..]).ToString();
                    blink.Add(firstHalf);
                    blink.Add(secondHalf);
                }
                else
                {
                    blink.Add((ulong.Parse(stone) * 2024UL).ToString());
                }
            }
            blinks.Add(blink);
        }

        var result = blinks.Last().Count;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Inputs.Input, 224869647102559)]
    public void Part2(string inputString, ulong expected)
    {
        int blinkTimes = 75;
        ulong[] initialStones = inputString.Split(' ').Select(x => ulong.Parse(x)).ToArray();

        Dictionary<(ulong stone, int blink), ulong> cache = new();
        ulong count = 0;
        foreach (ulong stone in initialStones)
        {
            count += Process(stone, 0);
        }

        ulong Process(ulong stone, int blink)
        {
            if (blink == blinkTimes)
            {
                cache[(stone, blink)] = 1;
                return 1;
            }
            if (cache.ContainsKey((stone, blink))) return cache[(stone, blink)];

            if (stone is 0UL)
            {
                ulong ret = Process(1, blink + 1);
                cache[(1, blink + 1)] = ret;
                return ret;
            }

            string stoneStr = stone.ToString();
            if (stoneStr.Length % 2 == 0)
            {
                ulong firstHalf = ulong.Parse(stoneStr[..(stoneStr.Length / 2)]);
                ulong secondHalf = ulong.Parse(stoneStr[(stoneStr.Length / 2)..]);
                ulong ret1 = Process(firstHalf, blink + 1);
                ulong ret2 = Process(secondHalf, blink + 1);
                cache[(firstHalf, blink + 1)] = ret1;
                cache[(secondHalf, blink + 1)] = ret2;
                return ret1 + ret2;
            }

            {
                ulong ret = Process(stone * 2024UL, blink + 1);
                cache[(stone * 2024UL, blink + 1)] = ret;
                return ret;
            }
        }

        Assert.Equal(expected, count);
    }
}

file static class Inputs
{
    public const string Sample =
        """
        125 17
        """;

    public const string Input =
        """
        0 5601550 3914 852 50706 68 6 645371
        """;
}