using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            foreach (string stone in blinks[i-1])
            {
                if(stone is "0") blink.Add("1");
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
    [InlineData(Inputs.Sample, 0)]
    [InlineData(Inputs.Input, 0)]
    public void Part2(string inputString, ulong expected)
    {
        string[] initialStones = inputString.Split(' ');

        //todo: cache
        ulong count = 0;
        foreach (string istone in initialStones)
        {
            List<string> blink = [istone];
            for (int i = 1; i < 76; i++)
            {
                List<string> nextblink = [];
                foreach (string stone in blink)
                {
                    if (stone is "0") nextblink.Add("1");
                    else if (stone.Length % 2 == 0)
                    {

                        string firstHalf = ulong.Parse(stone[..(stone.Length / 2)]).ToString();
                        string secondHalf = ulong.Parse(stone[(stone.Length / 2)..]).ToString();
                        nextblink.Add(firstHalf);
                        nextblink.Add(secondHalf);
                    }
                    else
                    {
                        nextblink.Add((ulong.Parse(stone) * 2024UL).ToString());
                    }
                }

                blink = nextblink;
            }

            count += (ulong)blink.Count;
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