using Xunit.Abstractions;

namespace AOC2024;

public class Day10
{
    private readonly ITestOutputHelper testOutput;

    public Day10(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }

    [Theory]
    [InlineData(Inputs.Sample1, 2)]
    [InlineData(Inputs.Sample2, 4)]
    [InlineData(Inputs.Sample3, 3)]
    [InlineData(Inputs.Sample4, 36)]
    [InlineData(Inputs.Input, 617)]
    public void Part1(string inputString, long expected)
    {
        int[][] map = inputString.Split(Environment.NewLine).Select(x => x.ToCharArray().Select(c => c is '.' ? -1 : int.Parse(c.ToString())).ToArray()).ToArray();
        (int x, int y)[] directions =
        [
            (-1, 0), //up
            (0, 1), //right
            (1, 0), //down
            (0, -1), //left
        ];
        int maxX = map.Length;
        int maxY = map[0].Length;

        List<(int x,int y)> startingPoints = [];
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (map[x][y] is 0) startingPoints.Add((x,y));
            }
        }

        int count = 0;
        foreach (var startingPoint in startingPoints)
        {
            HashSet<(int, int)> ninesVisited = [];
            GoUpTo9(map, startingPoint, directions, ninesVisited, maxX, maxY);
            count += ninesVisited.Count;
        }

        Assert.Equal(expected, count);
    }


    void GoUpTo9(int[][] map, (int x, int y) currentPos, (int x, int y)[] directions, ICollection<(int, int)> ninesVisited, int maxX, int maxY)
    {
        int currentNum = map[currentPos.x][currentPos.y];
        if (currentNum is 9)
        {
            ninesVisited.Add(currentPos);
            return;
        }

        foreach ((int x, int y) direction in directions)
        {
            (int x, int y) newPos = (currentPos.x + direction.x, currentPos.y + direction.y);
            if (newPos.x < 0 || newPos.x >= maxX) continue;
            if (newPos.y < 0 || newPos.y >= maxY) continue;
            if (map[newPos.x][newPos.y] != currentNum +1) continue;

            GoUpTo9(map, newPos, directions, ninesVisited, maxX, maxY);

        }
    }

    [Theory]
    [InlineData(Inputs.Sample5, 3)]
    [InlineData(Inputs.Sample6, 13)]
    [InlineData(Inputs.Sample7, 227)]
    [InlineData(Inputs.Sample4, 81)]
    [InlineData(Inputs.Input, 1477)]
    public void Part2(string inputString, long expected)
    {
        int[][] map = inputString.Split(Environment.NewLine).Select(x => x.ToCharArray().Select(c => c is '.' ? -1 : int.Parse(c.ToString())).ToArray()).ToArray();
        (int x, int y)[] directions =
        [
            (-1, 0), //up
            (0, 1), //right
            (1, 0), //down
            (0, -1), //left
        ];
        int maxX = map.Length;
        int maxY = map[0].Length;

        List<(int x, int y)> startingPoints = [];
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (map[x][y] is 0) startingPoints.Add((x, y));
            }
        }

        int count = 0;
        foreach (var startingPoint in startingPoints)
        {
            List<(int, int)> ninesVisited = [];
            GoUpTo9(map, startingPoint, directions, ninesVisited, maxX, maxY);
            count += ninesVisited.Count;
        }

        Assert.Equal(expected, count);
    }
}

file static class Inputs
{
    public const string Sample1 =
        """
        ...0...
        ...1...
        ...2...
        6543456
        7.....7
        8.....8
        9.....9
        """;

    public const string Sample2 =
        """
        ..90..9
        ...1.98
        ...2..7
        6543456
        765.987
        876....
        987....
        """;

    public const string Sample3 =
        """
        10..9..
        2...8..
        3...7..
        4567654
        ...8..3
        ...9..2
        .....01
        """;

    public const string Sample4 =
        """
        89010123
        78121874
        87430965
        96549874
        45678903
        32019012
        01329801
        10456732
        """;

    public const string Sample5 =
        """
        .....0.
        ..4321.
        ..5..2.
        ..6543.
        ..7..4.
        ..8765.
        ..9....
        """;

    public const string Sample6 =
        """
        ..90..9
        ...1.98
        ...2..7
        6543456
        765.987
        876....
        987....
        """;

    public const string Sample7 =
        """
        012345
        123456
        234567
        345678
        4.6789
        56789.
        """;

    public const string Input =
        """
        789032789543098712965101221019898103456454321
        470101678632105601876234034548781012765467100
        563210589789234512345549123697650101884398234
        454321430765432021236678998780143289991207965
        569834321898321130107654854012234676580312870
        578765010876980233498746763873401785676543541
        654321101945676542567832312964332694650145632
        765010210132156701010901203455963503765234701
        890124301231049878921001892396854412894339899
        981234456745030765432132701287760321023421078
        878965467896121076501245655332981212014322369
        690872356987232387431056986541274301015410450
        781081076578105498922347897890765412176704321
        632198985469216787011056785671893013089894565
        540345672354301017832345894789342104589723676
        891201451654302156901236893201230235678018985
        765432360783213443789107763196541343654321832
        654321078890198532876898632087832452789210101
        763011219912987691945678521087970961087123432
        892100308803676780032109431096981878996098542
        763965496734587654129818012345672345895467621
        354876785021096743016741001096789656760398930
        210787694103210892105432876585438769451207845
        321896543254387431012348965476323478300116596
        456910132123496545678256012365010565212323487
        787851401094563430789107101290126651054334678
        296542349887692321690678910987637892167210509
        101231056743781010541569010856546543098123418
        107645456652110127632438321658034012189014328
        298540365789054308971301434569122343276578789
        343231273210163210780210598778701656965489678
        850140984912378905698345699657892767874300569
        961001545805412344321010780346789876543211430
        873210656766901256543221271239654900612012321
        904589578927810187056102340128763211701012980
        012679865014321092167895434036650132890343478
        123456784325691233456996125675432147812334569
        456321092106780541245587098986789056903425876
        327867801256545650387654387129876541012510945
        010956910347838765495601296032989230109654354
        323843223498929678344760125141090121098701223
        456798114567012389413891434657890010567890110
        012387000345433678102110123766321023498989010
        323456781256921067221087432875435432103478321
        434321098987834554342196501989876301012565432
        """;
}