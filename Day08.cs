using Xunit.Abstractions;

namespace AOC2024;

public class Day08
{
    private readonly ITestOutputHelper testOutput;

    public Day08(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }
    
    [Theory]
    [InlineData(Inputs.Sample, 14)]
    [InlineData(Inputs.Sample2, 2)]
    [InlineData(Inputs.Sample3, 4)]
    [InlineData(Inputs.Input, 400)]
    public void Part1(string inputString, int expected)
    {
        char[][] map = inputString.Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();
        int maxX = map.Length;
        int maxY = map[0].Length;
        Dictionary<char, List<(int x, int y)>> coordinates = [];
        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map[x].Length; y++)
            {
                if (map[x][y] is '.') continue;

                if (coordinates.ContainsKey(map[x][y]))
                {
                    coordinates[map[x][y]].Add((x,y));
                }
                else
                {
                    coordinates[map[x][y]] = [(x, y)];
                }
            }
        }

        HashSet<(int, int)> antiNodeLocations = [];
        foreach ((char antennaType, List<(int x, int y)> antennaCoordinates) in coordinates)
        {
            for (int a1i = 0; a1i < antennaCoordinates.Count; a1i++)
            {
                (int x, int y) a1 = antennaCoordinates[a1i];
                for (int a2i = a1i+1; a2i < antennaCoordinates.Count; a2i++)
                {
                    (int x, int y) a2 = antennaCoordinates[a2i];

                    (int x, int y) anti1 = (a1.x + a1.x - a2.x, a1.y + a1.y - a2.y);
                    (int x, int y) anti2 = (a2.x + a2.x - a1.x, a2.y + a2.y - a1.y);

                    if (!(anti1.x < 0 || anti1.x >= maxX || anti1.y < 0 || anti1.y >= maxY))
                    {
                        if (map[anti1.x][anti1.y] is '.')
                        {
                            map[anti1.x][anti1.y] = '#';
                        }

                        antiNodeLocations.Add((anti1.x, anti1.y));

                    }

                    if (!(anti2.x < 0 || anti2.x >= maxX || anti2.y < 0 || anti2.y >= maxY))
                    {
                        if (map[anti2.x][anti2.y] is '.')
                        {
                            map[anti2.x][anti2.y] = '#';
                        }
                        antiNodeLocations.Add((anti2.x, anti2.y));

                    }
                }
            }
        }

        string res = string.Join(Environment.NewLine, map.Select(x => new string(x)));
        testOutput.WriteLine(res);

        Assert.Equal(expected, antiNodeLocations.Count);
    }

    [Theory]
    [InlineData(Inputs.Sample, 34)]
    [InlineData(Inputs.Sample4, 9)]
    [InlineData(Inputs.Input, 1280)]
    public void Part2(string inputString, int expected)
    {
        char[][] map = inputString.Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();
        int maxX = map.Length;
        int maxY = map[0].Length;
        Dictionary<char, List<(int x, int y)>> coordinates = [];
        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map[x].Length; y++)
            {
                if (map[x][y] is '.') continue;

                if (coordinates.ContainsKey(map[x][y]))
                {
                    coordinates[map[x][y]].Add((x, y));
                }
                else
                {
                    coordinates[map[x][y]] = [(x, y)];
                }
            }
        }

        HashSet<(int, int)> antiNodeLocations = [];
        foreach ((char antennaType, List<(int x, int y)> antennaCoordinates) in coordinates)
        {
            if(antennaCoordinates.Count == 1) continue;

            for (int a1i = 0; a1i < antennaCoordinates.Count; a1i++)
            {
                (int x, int y) a1 = antennaCoordinates[a1i];
                antiNodeLocations.Add(a1);
                for (int a2i = a1i + 1; a2i < antennaCoordinates.Count; a2i++)
                {
                    (int x, int y) a2 = antennaCoordinates[a2i];
                    antiNodeLocations.Add(a2);

                    (int x, int y) offset1 = (a1.x - a2.x, a1.y - a2.y);
                    (int x, int y) offset2 = (a2.x - a1.x, a2.y - a1.y);

                    (int x, int y) anti1 = (a1.x, a1.y);
                    (int x, int y) anti2 = (a2.x, a2.y);

                    while (true)
                    {
                        anti1 = (anti1.x + offset1.x, anti1.y + offset1.y);
                        if (!(anti1.x < 0 || anti1.x >= maxX || anti1.y < 0 || anti1.y >= maxY))
                        {
                            if (map[anti1.x][anti1.y] is '.')
                            {
                                map[anti1.x][anti1.y] = '#';
                            }

                            antiNodeLocations.Add((anti1.x, anti1.y));
                        }
                        else break;
                    }

                    while (true)
                    {
                        anti2 = (anti2.x + offset2.x, anti2.y + offset2.y);
                        if (!(anti2.x < 0 || anti2.x >= maxX || anti2.y < 0 || anti2.y >= maxY))
                        {
                            if (map[anti2.x][anti2.y] is '.')
                            {
                                map[anti2.x][anti2.y] = '#';
                            }

                            antiNodeLocations.Add((anti2.x, anti2.y));

                        } else break;
                    }

                }
            }
        }

        string res = string.Join(Environment.NewLine, map.Select(x => new string(x)));
        testOutput.WriteLine(res);

        Assert.Equal(expected, antiNodeLocations.Count);
    }
}

file static class Inputs
{
    public const string Sample4 =
        """
        T.........
        ...T......
        .T........
        ..........
        ..........
        ..........
        ..........
        ..........
        ..........
        ..........
        """;

    public const string Sample3 =
        """
        ..........
        ..........
        ..........
        ....a.....
        ........a.
        .....a....
        ..........
        ......A...
        ..........
        ..........
        """;

    public const string Sample2 =
        """
        ..........
        ..........
        ..........
        ....a.....
        ..........
        .....a....
        ..........
        ..........
        ..........
        ..........
        """;

    public const string Sample =
        """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """;

    public const string Input =
        """
        .....................5...............P............
        .............w.....T.........Xh.....5............u
        ...................kX.......T.......H.P...........
        ......u.....k...E..............................P..
        .....................F.........................o..
        ...............k........F...................o.....
        ...............E........x...k..w......S..........a
        ...................E.......9..x.....P.............
        ...............................X..................
        ..............................................X.a.
        ............A.............w........e...u..........
        ..T...................9........x....B..........H..
        ..........Z.....................u.5...........3...
        ....................d..F.....5...zC..B...S........
        ...............TfZ..........F.........7S..e.h...o.
        ....................................3e.........h..
        .....A...............f.........Hb....3O...........
        ..............f..d...............................o
        ......................................4...........
        ......g...................H..Z.........C.3.4..e...
        ............p.....d......................x...h....
        ...............f.p.....................l.......M..
        ..................................a............l..
        ........A.............j..........G................
        ...N...............9.......r..B.z.....C...........
        ............................lg......4..........7S.
        ................K.......Ey.......4.g...........V.7
        ........N......Av...............................G.
        .............b...K...B...................C......V.
        ...........K...................r.....a............
        .................................v...Mg...........
        ......p.....Z..........jr.....Y........J.....O..7.
        .....p....N.....t..........j...O............l.....
        .....................L...Ut.....O..v.....V........
        .d......D...W......n....j0..................s..G..
        .....y........L.........s...0nV.c.M...8...........
        ...........L.............................J..G.....
        ...D.................2............J..R............
        .......m................L.2.....vU8Rc.............
        ....................................n..cz..s......
        ..y....................2.......c..................
        ............w....0....2....8....1.R.6.............
        .............................86...r...........Y...
        KN..............m.................U...............
        .................t....n.0........1..J..z..........
        ......D........................1..................
        m..................W......R......61...M..........Y
        y....................W.b...m...................Y..
        .....D....................U............s..........
        ..............W..6...........tb...................
        """;
}