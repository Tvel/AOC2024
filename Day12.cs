﻿using Xunit.Abstractions;

namespace AOC2024;

public class Day12
{
    public record struct Coordinate(int X, int Y)
    {
        public static implicit operator Coordinate((int, int) c) => new Coordinate(c.Item1, c.Item2);
    }

    private readonly ITestOutputHelper testOutput;

    public Day12(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }

    [Theory]
    [InlineData(Inputs.Sample, 140)]
    [InlineData(Inputs.Sample3, 1930)]
    [InlineData(Inputs.Input, 0)]
    public void Part1(string inputString, long expected)
    {
        char[][] map = inputString.Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();
        (int x, int y)[] directions =
        [
            (-1, 0), //up
            (0, 1), //right
            (1, 0), //down
            (0, -1), //left
        ];
        HashSet<Coordinate> visited = [];
        List<(char symbol, long area, long perimeter)> plots = [];
        while (TryFindUnvisited(out Coordinate startCoordinate))
        {
            char symbol = map[startCoordinate.X][startCoordinate.Y];
            (long area, long perimeter) = FindAreaAndPerimeter(symbol, startCoordinate);
            plots.Add((symbol, area, perimeter));
        }

        long result = plots.Select(x => x.area * x.perimeter).Sum();

        Assert.Equal(expected, result);

        return;

        bool TryFindUnvisited(out Coordinate unvisited)
        {
            for (int x = 0; x < map.Length; x++)
            {
                for (int y = 0; y < map[x].Length; y++)
                {
                    if (!visited.Contains((x, y)))
                    {
                        unvisited = (x, y);
                        return true;
                    }
                }
            }

            unvisited = default;
            return false;
        }

        (long area, long perimeter) FindAreaAndPerimeter(char symbol, Coordinate coordinate)
        {
            if (map[coordinate.X][coordinate.Y] != symbol) return (0, 1);
            if (visited.Contains(coordinate)) return (0, 0);

            long area = 1;
            long perimeter = 0;
            visited.Add(coordinate);

            foreach ((int x, int y) direction in directions)
            {
                Coordinate newCoordinate = (coordinate.X + direction.x, coordinate.Y + direction.y);
                if (newCoordinate.X < 0 || newCoordinate.X >= map.Length || newCoordinate.Y < 0 || newCoordinate.Y >= map[newCoordinate.X].Length)
                {
                    perimeter++;
                    continue;
                };
                (long area, long perimeter) found = FindAreaAndPerimeter(symbol, newCoordinate);
                area += found.area;
                perimeter += found.perimeter;
            }

            return (area, perimeter);
        }
    }
}

file static class Inputs
{
    public const string Sample =
        """
        AAAA
        BBCD
        BBCC
        EEEC
        """;

    public const string Sample2 =
        """
        OOOOO
        OXOXO
        OOOOO
        OXOXO
        OOOOO
        """;

    public const string Sample3 =
        """
        RRRRIICCFF
        RRRRIICCCF
        VVRRRCCFFF
        VVRCCCJFFF
        VVVVCJJCFE
        VVIVCCJJEE
        VVIIICJJEE
        MIIIIIJJEE
        MIIISIJEEE
        MMMISSJEEE
        """;

    public const string Input =
        """
        SSSSSSSSSSSSSSSWWWWWHHHHHHHHHHHHHHHHHHBBBJJJJJJJJJIJUUUUUUUUUOOOOOOOOOHHHHHHHHHGGGGGGGGGGGGGGGGTTTTTTTTTTZZZHHSSHHHEEEEEEEETTTTTTTTTTOOOOOOO
        SSSSSSSSSSSSSSSWWWWWWHHHHHHHHHHHHHHHHHBBJJJJJJJJJJJJUUUUUUUOOOOOOOOOOOOHHHHHHHHHGGGGGGGGGGTTTGTTTTTTTTTTZZHHHSSSHHEEEEEEEEETTTTTTTTTTTTOOOOO
        SSSSSSSSSSSSAAWWWWWWWBBBBHBBBHHHHHHHHHJJJJJJJJJJJJJUUUUUUUPOCOOOOOOOOOOHHHHHHHHHGGGGGGGGGGGTTTTTTTTTTTTTZHHHHHHSHHHHEEEEEEETTTTTTTTTTTOOOOOO
        SSSSSSSSSSSSAAAAAWRWBBBBBBBBBHHHHHHHHHJJJJJJJJJJJJJJUUUPPPPPPSOOOOOOOOOHOOOOHHHGGGGGGGGGGGGTTTTTTTTTTTTTHHHHHHHHHHHHBBEEEEETTTTTTTTTTTTOOOOO
        SSSSSSSSSSAAAAAAAWRWWBBBBBBHHHHHHHHHHHJJJCJJJJJJJJJJUUUUPPPPPOOOOOOOOOOOOOOOHHHGGGGGGGGGGGGTTTTTTTTTTTTTHHHHHHHHHHHBBBBBEEETTTTTTTTOOOOOOOOO
        SSSSSSSSAAAAAAAARRRWBBBBBBBHHHHHHHHHHHJJJJJJJJJJJJJJUUUUPPPPPOOOOOOOOOOOOOOOHGHGGGGGGGGGGGGETTTTTTTTTTTTHHHHHHHHEHHBBBBBEEEEEOHTTTTOOOOOOOOO
        SSSSSSSAAAAAAAARRRRBBAABBBZZHHHHHHHHCCXXJJJJJJJJJJUUUUUUPPPPPPPOOOOOOOOOOOOOGGGBGGGGGGGGGGGETTTTTTTTTTTTHHGGGHHHHBBBBBBBEEEEEOOTOTOOOOOOOOOO
        SSSSSSSSAAAAAAARRRKBBAABBBEZHHHHHHHHHXXXJJJJJJJJJUUUUUUPPPUPPPPOOPOOOOOOOOSGGGGGGGGCGGGFFQFETTTTTTTTTTTPHHGGGGGHGBBBBBBBBEEEOOOOOOOOOOOOOOOO
        SSSSSSSSSAAKKAAARRRAAAAAEEEEEHEHXHHHHXXXJXJXJJJJJUUUUUUUUUUPSPPOOPPOOOOOOOOOGGGGGGGGGLFFFQFTTTTTTTTTTTTPGGGGGGGGGBBBBBBEEEEEEEOUOOOOOOOOOOOO
        CSSSSSSSSSSKAAAARRAAAAAAEEEEEEEXXXHHHXXXXXXXXJJJJJJUUUUUUUUUUUUUUUUUUOOOOOOGGGGGGGGGFFFFFFFTTTTTTTXTTTTLLGGGGGGGGGBBBEEEEEEEEOOUOOOOOOOOOOOO
        CSSSSSSSSSKKKAAAAAAAAAAAEEEEEEEXXXHHXXXXXXXXXJJJJGGUUUUUUUUUUUUUUUUUUOOOOGGGGGGGGGGFFFFFFFFFFTTTTTTTLLTLGGGGGGGGGGBBBEPEEEEEPXXOOOOOOOOXXOOO
        CSSSSSSSSKKKKAAAAAAAAEEEEEEEEPPEXXHXXXXXXXXXJJJJJJUUUUUUUSSUUUUUUUUUUOHHOGGAGGGGGGGFFFFFFFFFFTTGGGHHLLLLLLGGGGGGGGMBBPPEEEEEPPPPSOOOOOOXXXXX
        CMSSSSSSSKKKKKKAAAAAAAEEEEEEPPPEEXXXXXXXXXXHUUJFFJUUUUUUSSSUUUUUUUUUUAAAAAAAGGGGEEEEEEFFFFFFFFFRRRRHHLLLLLLLGGGGGGGBBPPPPPPPPPPFOOOOXXXXXXXX
        CCKSSSSSQQQKKKKAAAAAAAZEEEEEEEEEEEXXXXXXXXXHHHFFFUUUUUUUUUUUUUUUUUUUUAAAAAAAGGGGEEEEEEFFFFFFFFFRRRRRHLLLLLLLGGGGGGGGGGPPPPPPPPPWOOOXXXXXXXXX
        CCKSSSSQQQQKKKKKAAAAZAZEEEEEEEEEEXXBXXXXXXXHLHHUUUUUUUUUUUUUUUUUUUUUUAAAAAGGGEEEEEEEEEEFFFFFFFFRRRRRHLLLLLGGGGGGGGGGGPPPPPPPPPWWOOOOXXXXXXXX
        KCKKKKKKKKKKKKKKAAAAZZZEZEEEEEEEGBBBBXXXXHHHHHUUUUUUUUUUUUUUUUUUUUUUUAAAEEEEEEEEEEEEEEEEEEFFFFFHHHHHHHLLLLLLGGGGGGGPPPPPPPPPPPWWWOOOXXXXXXXX
        KKKKKKKKKKKKKKKKAAAAZZZZZZEEEEGGGBBBBBBBXHHHHHHHUUUUUUUUUUUUUUUUUUUUUANAEEEEEEEEEEEEEEEEEEFFFFFHHHHLLHLLLLLLGGGGGJGTRYPYPPPPPWWVWWWXXXXXXXXX
        KKKKKKKKKKKKKKKAAAAZZZZZZZEEEEGGBBBBBBBHHHHHHHHUUURCUUUUUUUUUUUUUUUUUAAJEEEEEEEEEEEEEEEEEEFFFHHHHHHLLLLLLLLLGGGGGJRRRYYYPPPWWWWWWWWXXXXXXXXN
        KKKKKKKKKKKKKVKAAAAZZZZZZNEEEEGGBNBBBBBBBHHHHHHUURRCUUUUUUUUUUUUUUUUUJJJEEEEEEEEEEEEEEEEEEFHHHHHRHRILLLLLLLLGGGGGRRRRYYYYPWWWWWWWWWXUXXXPXNN
        KKKKKKKKKEKEKKKAAAAZZZZZZEEEEGGBBBBBBBBBGGHHHHHHFCCCUUUUUUUUUUUUUUAAJJJJEEEEEEEEEEEEEEEEEEXHHHHHRRRRLLLLLLLLGGGGIRRRRYYYYPWWSWWWWWWWUUUUUUUN
        KKKKYYKEEEEEKKKKAAAAZZZZZEEENNGBBLBSBBBBBGGBBHHBFFCCCVNNUUUUUUUNNAAJJJJJEEEEEEEEEEEEEEEFFFXXXHXXXRRRLLLLLLLLLLLYYYRRRYYYYPWYYWWWWWWUUUUUUUUN
        KKKYYYKEEXEXXRKKKKIZZZZZZPEENNBBLLLBBBBBBGGBBBHBFCCCCCNNUUUUUUUNNAJJJJJJEEEEEEEEEEEEEEERXXXXXXXXXRRRLLLLVRLLYYYYYYRRRYYYYYYYYWWWWWWUUUUUUUUN
        KRRYYYXXXXXXXXKKKKZZZZZZZZZZHHHHHLBBBBBBBBBBBBBBBFNNNNNNUUUUUUUNNJJJJJJJEEEEEEEEEEEEEEERRXXXXXXXXRRRRRLRRRRLLYYYYYYYYYYYYYYYYYWWWCCUUUUUUUUU
        RRYYYXXXXXXXKKKKKKKZZJZZZZZZHHHHHBBBBBBBBBBBBBBFFFNNNNNNNNNUUUNNNJJJJJJJEEEEEEEEEEEEEEERRXXXXXXXXRRRRRRRRRRRLKYYYYYYYYYYYCYYYWWWWCKUUUUTTTTT
        RRRYYXXXXXXXXXKKKKKZZJZZZZZZZHHHHBBBBBBBBBBBBFFFFFNNNNNNNNNNNNNNNJJJJJJJJCCCCDDEEEDDXXXXXXXXXXXXRRRRRRRRRRRRLKYYYYYYYYYYYCCCCCCWCCKUUTTTTTTT
        YYYYLLXXXXXXXXKKKKKKZZZZZZZZHHHHBBBBBBBBBBBBBBFFFFNNNNNNNNNNNNNNNJJJJJJCCCCCDDDEEEDBBBYXXXXXXXXRRRRRRRRRRRRRLRYYYYYYYYYYYCCCCCCCCCCCUTTTTTTT
        LLYLLLXXXXXXXKKKKKKKZZZZZZZZHHHBBBBBBBBBBBBBFFFFFFNNNNNNNNNNTTTTJJJJJJJCCCCDDDDDDDDDDBYYXXXXXXXRRRRRRRRRRRRRRRYYYYYYYYYYYYCCCCCCCCCCCTTTTTTT
        LLLCLLLXXXXXXKKKKKKKZZZZZZZHHHHBBBBBBBBBBBBBFFFFFFNNNNNNNNNNTTTTDDDDDJJJJJCDDDDDDDDDDYYYXXXXXWRRRRRRRRRRRRRPPPYYDYYYYYYYYYYCCTCCCCCCCTTTTTTT
        LLLLLLXXXXXXXKKKKKKKZZZZZZZZHHHBBBBBBBBBBBBFFFFFFLLLLLLLLLNNTTTTDDDDDJJJJJCDDDDDDDDDYYYYYYYYYRRRRRRRRRRRRRRRRPDDDDDDYYYYYYYYTTCCCCCCCTTTTTTT
        LLLLLYYYXXXXXKKKKKHKZZZHHHHHHHBBBBBBBBBBBBBFFFFFLLLLLLLLLLNNTTTDDDDDDAJJJCCDDDDDDDDDDDYYYYYYYRPYWWRRRRRRRRCCRRDDDDDDYYYYYYYYTCCCCCCCCTTTTTTT
        LLLLYYYYYYXXXKKKKKHHZZZHHHHHHHHHHBBBBBBBBBBZZFFFLLLLLLLLLLETETTDDDDDDAAAADDDDDDDDDDDDDYYYYYYYYPYWRRRRRRRRRCCCCCDDDDDDYYDYYYYTTCCCCCCCTTTTTTT
        LLLYYYYYYYKXXKKJJHHHHZZHHHHHHHHHHHHBBBBYBBBZWHFFLLLLLLLLLLEEEEEEEDDDDDDADDDDDDDDDDDDDDYYYYYYYYYYWRRRRRRRRCCCCCCDDDDDDDDDYYDDCCCCCCCCGTTTTTTT
        LLLLYYYYYYKKKKKJJHTHHHHHHHHHHHHHHHHOBBBYYBBZWHHHLLLLLLLLLLEEEEEEDDDDDDAADDDDDDDDDDDDDDYYKYYYYYYYYYRRRRRCCCCCCCCCDDDDDDDYYYDDDCCCCCCCGTTTTTTT
        LLLLLYYYYYKKKKJJNHHHHHHHHHHHHHHHHHHONNNNNNNNWHAALLLLEEEEEEEEEEEEDDDDDAAAAAADDDDDDFFDDDDFFYYYYYYYYYRRRRQCCCCCCCPPPPPDDDDDYYDTTTTTTTTGGGTTTTTT
        LLLLYYYYYYYZZJJJHHHHHHHIHHHHHHHHHOOONNNNNNNNWWAALLLLEAEEEEEEEEEEDDDDDDDTTTTDTDDDDFFDFFFFFNNYYYYYYLLLRCCCCCCCCCPPPPPPPDDDDDDTTTTTTTTTTTTTTTTT
        LLLYYYYYYBZZZVZJJJJJJHHIIIHHHOHHOOOONNNNNNNNWAAALLLLAAEEEEEEEJEEDDDDDDDTTTTTTTDDDFFFFFFFFFNYYYYYYLLLLLCCCCCCCPPPPPPPPDDDDDDTTTTTTTTTTTTTTTTT
        LLLYYYYYYBBZZZZJJJJHHHHIIIHHHOOOOOOONNNNNNNNWWAALLLLAAAEAEEEEEEEDYDDDTTTTTTTTPPDZFFFFFFFFFFUUUUYYLLLLLLCCCCCPPPPPPPPPDDDNNNNNNNTTTTTTTTTTTTT
        LLYYYYYYYBBZZZZZJJJJJIHIIHHHHOOOOOOONNNNNNNNWWWWLLLLAAAAAAEYEEEEEYYYDTTTTTTTTPPAZFFFFFFFFFFUUUUYYYLLLCCCCPPPPPPPPPPPPPDXNNNNNNNTTTTTTTTTGGGX
        LLLYYYYYBBZZZZZZJJJJIIIIIHHHHHHOOOOONNNNNNNNWWWWAAAAAAAAAAEEEHEEEYYYTTYTTTAAAPAAZFFFFFFFFFFFUUUYYYLLLCLLPPPPPPPPPPPPPPDXNNNNNNNNNTTTTTTTGGGX
        LLLLLYYYYYZZZZZZJJJIIIIIIIIIHFHODOOONNNNNNNNAWWWAAAAAAAAAHHHEHHHEYYYYYYTTSAAAAAWFFFFFFFFFFFFUUUYYYLLLLLLPPPPPPPPPPPPYPDXNNNNNNNNNTTTTTTTGGXX
        LLLLLLLYJJJJZZJJJJJIIIIIIIIIIFFFOOOOOONNNAAAWWWWWAAAAAAAAHHHHHHHEEYYYYYYYYAAAAAAAFFFFFFFFFFUUUURRLLLLLLLLPPPPPPPPPPPPPDPNNNNNNNNNTTTTTTTGGBB
        LLLLLLLLLJJJZZJJJJJIIIIIIIIIIFFFOOOOOONNNOATTWWWWAAAAAAFFHHHHHHHYYYYYYYZYAAAAAAAAAFKFFFFFFFFFRRRRLLLLLLLLPPPPPPPPPPPNNNNNNNNNNNNNTTTTTTTGNBB
        LLLLLLLBLJJJJJJJJJJIIIIIIIIIIFFFOOOOOONNNTTTTWWWWWAAAHHHHHHHHHHHYYYYYXZZZZZZAAAAAAFFFFFFFFRFRRRRRLLLLLLLLLLPPPPPPPPPNNNNNNNNNNNNNNBBBBBBGBBB
        LLLLLLLLHHHJJJJJJCCCIIIIIIIIIIIFFFOOHHNNNOTTTWWWWWWLLSHHHHHHHHHYYYYYYXZZZZZZAAUAAAAFFFFFBFRRRRRRRRLYLLLLLPLPPPPPPPPPNNNNNNNNNNNNNNBBBBBBBBBB
        LLLLLLLHHHHJJJJJJJCCIIIIIIIXIIIIFFFOFHNNNTTTTTTTTWWLLHHHHHHHHHHYYYYYYXZZZZZZZAAAAAAAAFFBBFRRRRRRRRLYYLLLLPPPPPPPPPPPNNNNNNNNNNNNNNBBBBBBBBBB
        HLLLLLHHHHHHJHJJCCCCCCIIIIIXIIIFFFFFFFNNNTTTTTTTTTTLLHLHHHHHHHHVVVYXXXXXZZZZZJJJAJDDDFBBBFRRRRRRRRYYYLLLLPPPPPPPCPPPNNNNNNNNNNNNNNBBBBBBBBBB
        HHHHLHHHHHHHHHJJCCCCCCTIXXXXXFFFFFFFFFTTTTTTTTTTTLLLLLLHHVHHVHVVVVXXSSSXSSZZZZZJJJJDFFDDBFRRRRYYYYYYYLLLPPPPPPPCCPCCAAAAANNNNNNNNNBBBBBBMHHH
        HHHHHHHHHHHHHHHHBCCCCFCIIXXXXXFFFFFFFFTTTTTTTTTTLLLLLLXHHVVVVVVVVVVVSSSSSSSZZZZZJDDDFDDDDDDRRDAYLLLLLLLLPPPPPPPCCCCAAAAAANNNNNNNNNBBBMMBMHHH
        HHHHHHHHHHHHHHHBCCCCCCCCCCXXXIIIIIIIIITTTTTTTTTTLLLLLLLVHVVVVVVVVVVDSSSSSSSZZZZZZZDDDDDDDDDDDDDLLLLLLLPPPPPPPPPPCCCAAAAAAAAAAAABBBBBBBMMMHHH
        HHHHHHHHHHHHHHBBBCCCCIIIIIIIIIIIIIIIIITTTTTTLLLLLLLLLLLVVVVVVVVVVSSSSSSSSSSVSZZZZZDDDDDDDDDDDDLLLLLZLLPPPPPPPPPPPAAAAAAAAAAAAAABMMMMMMMMMMHH
        HHHHHFHHHHHHHBBBBBCCCIIIIIIIIIIIIIIIIITTLLLLLLLLLLLLLLLVMVVVVVVVVSSSSSSSSSSSSZZZZZDDDDDDDDDDDDDLLLLZZLPPPPPPPPPPPAAAAAAAAAAAAAAAMMMMMMMMMMMM
        CCFFFFHHHHHBBBPBBBBCCIIIIIIIIIIIIIIIIITTTTLLLLLLLLLLLVVVMVVVVVVSSSJSSSSSSSSSSUZZIZDDDDDDDDDDDKKKLLZZLLLPPPPPPPPPOAAAAAAAAAAAAAAAMMMMMMMMMMMM
        CCFFFFFHHHHJBBBBBBCCCIIIIIIIIIIIIIIIIITVVVVLLLLLLLLLLLLVVVVVVVVVVSSSSSSSSSSSZUUZIIDDDDDDDDDDKKKKLZZZZKKKKPPPPZPPOOOAAAAAAAAAAAAAMMMMMMMMMMMM
        CFFFFFFHHHHHBBBBBBBCCIIIIIIIIIIIIIIIIIZVVVVLVLLLLLLBLLLVIVVVVVVVSSSSSSSSSSSSUUUUUDDDDDDDDDDKKKKKKKKKZZAKKPPZZZZOOOOOAAAAAAAAAMMMMMMMMMMMMMMM
        CFFFFFFHHHHHBBBBBXBBCIIIIIIIIIIIIZZZZZZVVVVVVLLLLLLBBIIIIVVVVVVSSSSSSSSSSSSSUUUUUUUDDDDDDDDKKKKKKKKKKKKKZZZZZZZZOOOOAAAAAAAAAAMAMMMMMMMMMMMM
        FFFFFFFFFHHFFBBBXXBBIIIIIIIIIIIIIZZZZVVVVVVVLLLLLLLLBBIIIIVVPVVSSSSSSSSSSUUUUUUUUUUDDDDDDDDDDKKKKKKKKKKKZZZZZZZZOOOOOOAAAAAAAAAAMMMMMMMMMMMM
        FFFFFFFFUFFFBBBBBBBBIIIIIIIIIIIIIZZZZVVVVVVVLLLLLLLBBBBIIIPUPVPKSSSSSSSSSSUUUUUUUUUDDDDDDDDDPKKKKKKKKKKKKZZZZZZOOOOOOOAAAAAAVVAAMMMMMMMMMMMM
        FFFFFFFFFFBBBBBBBBJJIIIIIIIIIIIIIZZZZZVVVVVVVLLLLLLLBBIIIPPPPPPSSSSSSSSSUUUUUUUUUUUDDDDDDDDDPPPKKKKKKKKKKZZZZZZOOOOOOOTTHAAVVVVVMCMMMMWMMMMM
        FFFFFFFFFBBBHBBBBJJJIIIIIIIIIIIIIZZZZZYVVYYVVVVLLLLLBBIIIIPPPPPPPZSSSSSSUUUUUUUUUUUUDDDDDDDPPPPKKKKKKKZZKZZZZZZOOOOOOTTTHHHVHHVMMMMMMMMMLMMM
        FFFFFFFFXBBBBBBBBBJJIIIIIIIIIIIIIZZZZNYVYYYYYYVVLLLLIIIIIIIPPPPPPZSSSSESUUUUUUUUUUUUUDDDDPPPPPPKKKKKKKKZZZZZZZZOOOOOOOTTHHHHHHVHCCCCMMMMMMMM
        FFFFFFFFFBBBBBBBBBBBIIIIIIIIIIIIIZZZZYYYYYYYYYYVLLILIIIIIIPPPPPPPPPSSSUUUUUUUUUUUUUUUDDDDPPPPPPKKKKKKZZZZZZZOOOOOOTTTTTTAAHHHHHHCCCCCCMMMMMM
        FFFFFFFFFBBBBBBBBBBBIIIIIIIIIIIIIZZZZZYYYYYYYYYYYLIIIIIIIIPPPPPPPPPPASUUUUUUBUGUUUUBUJJJJJJJJJJPPKKBZZZZZZZOOOOOTOTTTTTTAAHHHHCCCCCCCCMCMMMM
        FFFFFFFFFFBBBBBBBBBBIIIIIIKKZZZZZZZQQYYYYYYYYYYYYIIIIIIIIPPPPPPPPPPPAAUUUUBBBBUUULBBUJJJJJJJJJJPPKKKZZOZZZOOOOOOTTTTTTTTTHHHHHCCCCCCCCCCMMCC
        FFFFFFFCWWBBBBBBSBBBIIIIKYKKZZZZZZQQQYYYYYYPYYYIIIIIIIIIIPPPPPPPPPAAAUUUUUVVBBBUUUBBBJJJJJJJJJJPPPKAZZOOOOOOOOOOOTTTTTTLLLHHHHHCCCCCCCCCCMCC
        FFFVVVWWBWNWBSSSSBSSIIIIKKKKZZZZZZQQQYYYYYYPPPYIIIIIIIIIIIPPPPPPPPPPAUUAAAVVVVBBBBBBBJJJJJJJJJJPPPOOOOOOOOOOTTOOTTTTTTTLLLLHHHHCCCCCCCCCCCCC
        QQFFVVWWWWWWBSSSSSSSSKKKKKKKZZZZZZZZQZZYYYYPPPPIIIIIIIIIIIPPPPPPPPPPAAUAAAVVVBBBBBBBBJJJJJJJJJJPYPOOOOOOOOOOOTTTTTTTTTTTTLLHLHHCCCCCCCCCSSCC
        QWFVVVWWWWWWBBSSSSSSSKKKKKKKOOZZZZZZZZZIYYPPPPPVVIIIIIIIIIIPPAPPPAAAAAAAAVVVVBBBBBBBBJJJJJJJJJJPOOOOOOOOOOOOTTTTTTTTTTTTLLLLLLLLLLCCCCCCSSSS
        QWVVWWWWWWWWWOSSSSSSKKKKKKKKKKZZZZZZZZZIIPPPPPPVVIIIIIIIIIINPPPAAAAVVAAAAVVVVJJJJBBBBJJJJJJJJJJMMOOOOOOOOOOOTTTTTTTTTTTLLLLLLLLLLLLCCCCCSSSS
        WWWWWWWWWWWWWOOSSSSSKKKKKKKKKZZZZZZZZQIIIIPPPPVVDIIIIIIIIIIISSSAAAVVVVVVVVVVVJJJJBBBBJJJJJJJJJJMMNOOOOOOOOOETTTTMTTTTTTLLLLLLLLLLLLLLCSSSSSS
        WWWWWWWWWWWWWWSXSSSSKKKKKKKKKKZZZZZZZZIIIIIGGGVVDDIIIIIIIIISSSSASAAVVVVVVVVVVJJJJJJJJJJJJJJJJJJNNNNONNNOOEEETTTTMTTTTTTLLLLLLLLLLLLLASSSSSSS
        WWWWWWWWWWWWWWSSSSSSSKKKKKKKZZZZZZZZIIIIIGGGGGGVVVVVVIIVTTTSSSSSSAAAVMSVVVVVVJJJJJJJJJJJJJJJJJJNNNNNNNNOEEEEBBTTMTTTTTTLLLLLLLLLLLLLLLSSSSSS
        WWWWWWWWWWWWWWSSSSSSSSKKKZKZZZZZZZIIIIIIIGGGGGVVVVVVVVVVTTTSSSSSSSSSSSSVVVVVVJJJJJJJJJJIIIIINNNNNNNNNNNNNEEEBBTMMTTTTTTTTLLLLLLLLLLLLLMMSSSN
        WWKWWWWWWWWWWWWSSSSSHHKKKZZZZZZZZZIIIIIIIGGUGGVVVVVVVVVVVVTTSSSSSSSSSSSVLVVVVJJJJJJJJJJIIIIINNNNNNNNNNNNNEEEEEEMMMTTAAAALLLTTLLLLLLLLLMMMXXN
        WWWWWWWWWWWWWWTSJJJSJHKKKYZZZZZZZIIIIIIIGGGVVVVVVVVVVVVVVVVTTSSSSSSSSSLLLVVVVJJJJJJJJJJIIIUINNNNNNNNNNNNNEEEEMMMMTTTAAAALTTUTTTLLLTXXMMMMXXX
        WWWWWWWWWWWWWWTJJJJJJHKKYYYZOZZZZZIIIIIGGGVVVVVVVVVVVVVVWVDDDSSSWSSLLSLLLLVVVJJJJJJJJJJLIINNNNNNNNNNNNNNNEEEEEEMMMTMTAAAATTUTTTTLLTTXMMXXXXX
        WWWFWWWWWMMMWMJJJJJJJJJYYYYZZZOZIIIIIIIIGGVVVVVVVVVVVVVVWWDDDDDWWSAALLLLLLLLLJJJJJJJJJJLIDNNNNNNNNNNNNSNNEEEEEMMMMMTTAAAATTTTTTTTTTTXXMXXXXX
        WFFFMMMMMMMMMMJJJJJJJLJJJYYYYZZIIIIIIIIIIVVVVVVVVVVVMVVDDDDDDDDDDDDAAALLLLLLLLLLLLLLLLLLNNNNNNNNNNNNNNSNNNEFFPTTMTMTTTATATTTTTTTTTTXXXXXXXXF
        WFFFMMMMMMMMMMJJJJJJJJJJJYYYYIZIIIIIIIIIZVVVVVVVVVMMMMDDDDDDDDDDDAAAALLLLLLLLLLTTGGLLLNNNNNNNNNNNNSNSJSSTTEFFPTTTTTTTTATTTTTTTTTTTTXXXXXCXFF
        WFFFFRRRRRRRJJJJJJJJJJJJJJYYIIIIIIIIIIIVVVVVVVVVVVMMMMDDDDDDDDDDDAATLLLLLLTLLLTTPGGLLLLNNNNNNNSNSSSSSSSSTSFFFPTTTTTTTTTTTTTTTTTTTTXXXXXXXXFF
        FFFFFRRRRRRRGJJJJJJJJJJJJJIIIIIIIIIIIIIDVVVVVVVVMMMMMMDDDTTTTDDDTAATTLLLLLTLTTTTGGGLLNNNNGGNSSSSSSSSSSSSSSFFPPTPMTTTTTTTTTTTTTTTTTXXXXXXXXFF
        FFFFFFRRRRRRRRUJJJJJJJJJJJIYYYIIYIIIIIIVVVVVVVVMMMMMMDDDDTTTTDDDTTTTTLLLLLTTTTTTTGGGGGGGGGGGSSSSSSSSSSSSSPPPPPPPPPXTTTTTTTTTTTTTTTTXXXEXXXFF
        FFFFFFFRRRRRRRRJJJJJJJJYJYYYYYYIIIIIIIMMVMVVVVMMMMMMMDDDDTTTTDDTTTTTTTTLTTTTTTTTTGGGGGGGGGGGSSSSSSSSSSSSPPPPPPPPPPTTTTHHHHHTTYTTWWWXWEEEEEEE
        FFFFFFFFFRRRRRRJJJJJJJJYYYYYYYIIIIIJIMMMMMMMVVMMMMMMMDDDDTTTDDRTTTTTTTTTTTTKTTTWWWGGGGGGGGGGGGGSSSSSSSSSPPPPPPPPPPTTTHHHHHHTTYYIYWWWWWWWEEEE
        FFFFFRRRFRRRRRRRJJJJJJYYYYYYYYIIIIIJIUUMMMMMMMMMMMEDDDDDDTTTDDDTTTTTTTTTTKKKTTTKKKGGGGGGGGGVGGGSSSSSSSSSPPPPPPPPPPPHHHHHHHHYYYYYYWWWWWWWWEEE
        QFFFMRRRRRRRRRRRRRQQQJJYYYYYYYYIIIIUUUUUUUUUMMMMMEEEEEEDDTTTDDDDTTTTTTTTTKKKKKTKKKKKGGGGGGVVVVSSSSSSSSSSPPPPPPPPPPPHHHHHYYYYYYYYYWWWWWWWWWED
        QFMMMRRRRRRRRRRRRRRQYYYYYYYYYYYYYYYUUUUUUUUUMMEEEEEEEEEETTTTDDDDDTTTTTTTKKKKKSKKKKKKGGGVVVVVVVVWSSSSSSSSSSPPPPPPPPHHHHHHYYYYYYYYYWWWWWWWWWWW
        QQQQQRRRRRRRRRRRRRRRYYYYYYYYYYYYYYUUUUUUUYUUUMEEEEEEEEEETTTTDDDKJJJJTTTTKKKKKKKKKKKKGGGVVVVVVVVWWSSSSSSSSSPPPPPPPPPHHHYYYYYYYYYYYYWWWWWWWWWW
        QVVVQCRRRRRRRRRRRRRRYYYYYYYYYYYYYUUUUUUUUUUMMMEEEEEEEEEETTTTDRDKJJJJTTTTKKKKKKKKKKKKGGGGZVVVVVVWWWSMMSSSSSPPPPPPPPHHHHYYYYYYYYYYYYYWWWWWWWWW
        VVVVVRRRRRDRRRRRRRRRYYYYYYYYYYYYUUUUUUUUUUUMMMEEEEEEEEEETTTRRRRRJJJJJJJTTKKKKKKKKKKKHGGZZVVQUXWWWWWMMMMMSSPPPPPPPPHHHYYYYYYYYYYYYYYWWWWWFWWP
        VVVVRRRRRDDDRRRVRYYYYYYYYYYYYYYYYYYUUUUUUMMMMEEEEEEEEEEETTTRRRRVJJJJJJJTTQQKKKKKKKFKHHQQQQQQXXWWWWWWUMMMSPPPPPPPPPHHHHYYYYYYYYYYYYWWWWWWFWWP
        VVVVVNNDDDDRRRRVVYYYYYYYYYYYYYYYYYYEUUUUUMMMMEEEEEEEEEEEERRRRRRVJJJJJJJQQQQQKKKKKKKZKHHQQQQXXXWXWWWUUMMMMPPPPPLLPPLLHHYYYYYYYYYYYJWWWWFFFWFS
        VVVVVNNNDDVYRVVVVYYYYYYYYYYYYYYEEEEEEUUUUUMMMMEEEEEEEEEEEEERRRRRJJJJJJJQQQQQKKKKKKKKKKQQQQQXXXXXWWUUUPPPPPPPPPLLLLLLHLYYYYYYYYYYYWWWSGGFFFFS
        VVVVVVNNDNVVVVVVVVVVYYYYYXYYYYYYEEEEEEUUMMMMMMEEEEEEEEEEEEQQQQRRJJJJJJJQQQQQKKKKKKKKMQQQQQQXXXXXXXUUUPPPPPPPPPPLLLLLLLLLLYYYYKYYYWISSGGGFFFS
        VVVVVVNNNNVVVVVVVVVVVYYCXXYYYXYYEEEEEEEEEMMMMMEEEEEXEEREQQQQQQQVJJJJJJJQQQQPPKKKKKKKKQQQQQQXXXXXXXXXUXXPPPPPPPPLLLLLLLLLYYYYYKYYYYYSGGSSSSFS
        VVVVVNNNNNFFVVVVVVVVVJJXXXXXXXXYYEEEEEEEMMMMMMMMMEEXXQQQQQQQQQQVJJJJJJJQQQQQKKKKKKKXXQQQQQXXXXXXXXXUUXXQQPPPPPPPLLLLLLLLKKKKKKKKYVSSSSSSSSSS
        VVEVVINNNNFFFVVVVVVFFXXXXXXXXXYYYEEEEEEEEDDDDMDCMKKXXXQQQQQQQQQQJJJJJJJQQQQQKKKKKKXXXQQQXXXXXXXXXBBIXXXQQQPXSPPLLLLLLLLLKKKKKKKKSSSSSSSSSSSS
        IIIIIIINNNFFFFFFFFFFXXXXXXXXXXXXYEEEEEEEDDDDDDDCKKKXXXQQQQQQQQQQJJJJJJJQQQQQKKKKKKXXXXXXXXXXXXXXBBBXXXXQQQSSSPPLLLLLLLLLKKKKKKKSSSSSSSSSSSSS
        IIIIIINNNNFFFFFFFFFXXXXXXXXXXXXXEEEEEEEDDDDDDDDCKKKKXXQQQQQQQQQQQQQDQQQQQQXQKKKKXXXXXXXXXXXXXXXXBBBVXXXXQQSSSSYYLLLLLLLLKKKKKKKSSSSSSSSSSSSS
        IIIIIIIIIFFFFFFFFFFFXXXXXXXXXXXXEEEEEEEEDDDDDDDDDKKQQQQQQQQQQQQQQQQDQQQQNQXQXXKXXXXXXXXXXXXXXXXXXXBXXXXXSSSSSSSYSSLLLILLKIIIKKKKKSSSSSSSSSSS
        IIIIIIIIIZFFFFFFFFFXXXXXXXXXXXXXEEMEEEEDDDDDDDDKKKKQKQQQQQQQQQQQQQQQQQQQQQXXXXXXXXXXXXXXXXXXXXXXXXGXXXXXSSSSSSSSSIIIIIIIIIIIKKKKKKKKSSSSSSSS
        IIIIIIIIIZZZOOOOFFFXXXXXXXXXXXXXMEMEEEEHDDDDDDDDDKKKKQQQQQQQQQQQQQQCQYYYYQMXXXXXXXXXXXXXXXXXFFXXXXXXXXXXSSSSSSSIIIIIIIIIIIIIKKKKKKKKSSSSSSSS
        IIIIIIIIIZZZOOMMFFFFXXXXXXXXXMMMMMMREEEHHHDDDDDDDKKKKKQQQQQQQQQQQQDDDYDDDLMXXXXMXXXXXXXXXXXXFFXXXXXXXXXSSSSSSSUIIIIIIIIIIIIIKKKKKKKSSSSSSSSS
        IIIIIIIIIZZZOOOOOOFFXXXXXXXXXMMMMMMMMEHHHDDDDDDDDKKKKIQQQQQQQQQQQQDDDDJDLLMMMMMMMMMMXXXXXXXXFFFXXXXXXXXSSSSSSSSIIIIIIIIIIIIIIIKKKKKKSSSSSSSD
        IIIIIIIIZZZOOOOOOOFFXXXXXXXXXMMMMMMMHHHHHDDDDDDDDDKKKIQQQQQQQQQQQQDDDDDDDDMMMMMMMMMMXXYXXXXXFFFXXXXXXSSSSSSSSSSSSIIIIIIIIIIIIIKKKKKKBSSSKSDD
        IIIIIIIIIZZOOOOOOOOFFXXXXXXXMMMMMMMMHHHKHKDDDDDKKKKKIIIIKICIQQAQQADDDDDDDMMMMMMMMMMMMYYXXXXFFFXXXXXXXXXSSBSSSSSSSIIIIIIIIIIIIKKKKKKKKZSSSDDD
        IIIIIHIIZZZOOOOOOFOFFFFXXXXXMMMMMMMMMHHKKKDDDDDDKKKKIIIIIIIIQIILIIDDDDDMMMMMMMMMMMMMFYYYYXXXXXXXXXXXXXXSSSSSSSSSSIIIIIIIIIIIIIKKKKKKZZSSSDDD
        SSIIIHHHHZHOOOOOFFFFFFXXXXXMMMMMMMMMMHHKKKDDDDDKKKKIIIIIIIIIIIIIIIDDDDDDMMMMMMMMMMMFFYYYYVVVVXXXXXXXXXXXSSSSSSSSSIIIIIIVVIITIIIIKKZZZZSSXDDD
        SIISSHHHHHHOOOOOOOFFFFXXXXXXLMMMMMMMMHHKKKDDKDKKKKKKIIIIIIIIIIIIDDDDDDDMMMMMMMMRMFFFFYYVVVVVVVXXXXXXXXXMSSSSSSSSSSIIIIIVIITTITTIEZZFFZBBXDDD
        SSISSSSHHHOOOOOOOFFFFFFXXXXLLMMMMMMHHHHKKKKKKKKKKKKIIIIIIIIIIIIDDDDDDDDMMMMMMMRRMFFFFYYVVVVVVVXXXXXXAAASSSSSCSSSSVVIVVVVQVTTITTTEEZZFFBXXDDX
        SSSSDDDHHHDDDOOOOOXFFFFFXXXXLLMMMMMHSHHKKKKKCKKKKKKKIIIIIIIIIIIIDDDDDDMMMMMMMMMRMMFFFFVVVVVVVVVXXXXXXAASSSSSSFFSAVVVVVVVVVVTTTTEEEEFFFBXXDXX
        SSSSDDDDHHDOOOOODDXFFFFFXXXXXXXMQSHHSSHCCDCCCKKKKKKKIIIIIIIIIIIIDDDDBBMMMMMMMMMMMMFFFFVVVVVVVVTTXXXDAAAAASSSSFASAVVVVVVVVVVVVVTEOEEBFBBXXXXX
        SSSSDDDDHHDDDDDDDDXXXXXXXXXXXXXSSSSSSSHCCDDCCKKKKKKKIIIIIIIIIIBDDBBBBBMMMBBMMMMMMFFFFFVVVVVVVTTTTXGTTAAAAAAASAAAAVVVVVVVVVVVVEEEEEEBBBXXXXXX
        SSSWWWDDDDDDDDDDDXXXXXXXXXXXXXSSSSSSSSSSCCCCCCCKKKKKKIIIIIIIAABBBBBBBBBMMBBBMMMMMMFFFVVVVVVTTTTTTTTTTTAAAAAAAAAAAAVVVVVVVVVVVVEEEEEEBBXXXXXX
        SSSWWWWDDDSDDDDDDDXXXXXXXXXXXXSSSSSSSSSSCCCCCCCCKKKOOOXIIIOAAAAAABBBBBBMMBBBMMMFFFFFFFVVVVHTTTTTTTTTTTTAAAAAAAAAAZVVVVVVVVVVVVBEEEBBBBBXXXXX
        SSSWWLLSSSSDDDSSDMMMMXXXXXXXXXSSSSSSSSSSSCCCCCCCCKKOOOOIIIOAAAAAABBBBBBBBBBBMBFFFFFFFFVVVVVFFTTTTTTTTTTTAAAAAAAAAAVVVVVVVVVVVVBBBBBBBBBXXXXX
        SSSSWSSSSSDDDSSSMMMMXXXXXXXXXXJSJSSSSSSSSCCCCCCCKKKOOOOOOOOOOAAAABBBBBBBBBBBBBFFFFFFFFFFFFFFTTTTTTTTTTTTFFAAAAAAAAVVVVVVVVVVVVVBBBBBBBBXXXXX
        SSSSSSSSSSSSSSSMMMMMMMXXXXXXXJJJJSSSSSSSSCCCCCCCKKOOOOOOOOOOOAAAABBBBBBBBBBBBBFFFFFFFFFFFFFFTTTTTTTTTTTFFFFAAAAAAAAAVVVVVVVVVVBBBBBBBBBXXXXX
        SSSSSSSSSSSSSSSMMMMMMMXXXVXXXJJJJSSSSSSSSCCCCCCKKKKOOOOOOOOOOAAAABBBBBBBBBBBBBFFFFFFFFFFFFTFFTTTTTTTTTFFFFFAAAAAAAAAVVVVVVVVBBBBBBBBBBBBBXXX
        SSSSSSSSSSSSSSSMMMMMMMXXXVVXJJJJJSSSSSSSCCCCCCCKKKOOOOOOOOOOOAABBBBBBBBBBBBBBBFFFFFFFFFFFTTTTTTTTTTTTTFFFFFFKAAAAAAAVVVVVVVVVBBBBBBBBBBBBBBX
        SSSSSSSSSSSSSSKMMMMMMMMMMVVXXPPJSSSSSSSSSSSCCCCCCKOOOOOOOOOOAAAABBBBBBBBBBBBBBFFFSFFFFFFFTTTTTTTTTTTTTFFFFFAAAAAAAAAUVVVVVVVBBBBABBBBBBBBBBB
        SSSSSSSSSSSSSSSSMMMMMMMMMMMKXPPPSSSSSSSSSSSSKCOCKKKKKZOOOOOOOAAAAABBBBBBBBBBQBFFSSSSFFFFFFTTTTTTTTTTTTFFFFFFASSSAAUUUVVVVBBBBBBAABBABBBBBBBB
        IISSSSSSSSSSSSSSMMMMMCMMMWMKKPPPPPPSAASSSSSSKCKKKKKKKZOOOOOAAAAAAABBBBBBBBBBBBFSSSSSFFFFFFTFTTTTTTTTTTFFFFFFFFFFUUUUUUVUUBBBBBBAAAAAAAAABBBB
        IISISSSSSSSSSSSMMMMMMCNCWWWNNNPPPPAAAASSSSSCKCCKKKKKAZOLOAAAAAAAAAAABBBCCBBSFFFSSSSSFFFFFFFFFFFTTTTTTTFFFFFFFFFFFUUUUUVVUUUBBBROAAAAAABBBBBB
        IIIIISSSSUUSSOSMMMMCCCNCWWWNNNPPNPNNAASSSSSCCCCCHHKHAAAAOAAAAAAAAAABBBBCCCSSSSSSSSSSSFFFFFFFFFFFKTTTTTFFFFFFFFFFFUUUUUUUUUUUURRRRAAAAABBBTTT
        IIISSSSUUUUUSUMMMMMCCCCCCWWNNNNPNXNAASSSSSSCCCCCCHKHAAAAAAAAAAAAAAAAABBBSSSSSSSSSSSSSFFFFFFFFFFKKKFFFFFFFFFFFFFFFUUUUUUUUUUURRRRBAAAAAAATTTT
        IIISISSUUUUUUUMMMMCCCCCCCCCCNNNNNNNNNSSSSCCCCCCCCHHHHAAAAAAAAAAAAAAAAAGBISSVVSSSSSSSFFFFFFFFFFFFFFFFFFFFFFFFFFFFFUUUUUUUURRRRRRRRRRRAAAAATTT
        IIIIIIUUUUUUUUMUMMCCCCCCCCYNNNNNNNNSSSSSSSCCCCCHHHHHHHHAAAUAAAAAAAAOAABBBVVVVVSSSSSSSFFFFFFFFFFFFFFFZFFFFFFFFFFFFUUUUUUUUURRRRRRRRRRAATTTTTT
        PPPIPPIUUUUUUUUUMCCCCCCCCCCNNNNNNNNSSSSSSSSCCCHHHHHHHHUUUUUUUAAAAAGOGABBBBDDDVSYFFSFFFFFFFFFFFFFFFFFZFFFFFFFFFFFUUUUUUUURRRRRRRRRRRAATTTTTTT
        PPPPPPUUUUUUUMMMMMMCCCCCCCCCNNNNNNNNMMSSSSSCCCTHHUUHHHUUUUUUUUAAAAGOGBBBBDDDDDFYYFFFFFFFFFFFFFFFFFZZZZZZFFFFFFFFFUUUUUUURRRRRRRRRRRAAAATTTTT
        PPPPPPUUUUUUUUMMMMCCCCCCCCCCCNNNNNMMMMMSSSSCQTTTTUUUUUUUQUUUUUAAAGGGGBBBBDDDDFFFFFFFFFFFFFHHHFFFZZZZZZZZZFFFFFFFFUUUUUUUURRRRRRRRRRAAAAATTTT
        PPPPPPPUPUUUUAMMMMCCCCCCCCCNNNNNNNMMMMMMSSSSTTTTUUUUUUUUUUUUUUAAGGGGBDDDDDDDDFFFFFFHHFFHHHHHZZZFZZZZZZZYYOOOFFUUUUUXXXXURRRRRRRRRRRRCCAATTTT
        PPPPPPPPPUPUUPPMMMCCCCCCCCCCNNNNNNMMMMMMMSSSTTTTUUUUUUUUUUUUUUGGGGGGDDDDDDDDDDDDDDDHHHFFHHHHHHZZZZZZZYYYYOYOFFUUUUUXXXXXXRRRRRRRRRCCCCAAAAAA
        PPPPPPPPPPPPPPTMMMGCCCCCCCCCNNNNNNMMMMMMSSTTTTTTTFUUUUUUUUUUUGGGGGGGDDDDDDDDDDDDDDHHHHHFHHHHHZZZZZZZYYYYYYYYUUUUUUXXXXXXXXRRRRRRRRCCCCCAAAAA
        PPPPPPPPPPPPPPPGGGGGCCCCCCCCNNNNNNNMMMSMSSTTTTTTTTTUUUUUUUUUUVGGGGGGDDDDDDDDDDDDDDHHUHHHHHHHHHZZZZZZYYYYYYYUUUUUUUXXXXXXXRRRRRRRRRCCCCCCAAAA
        GPPPPPPPPPPPPPPGGGGGGCCCCCHNNNNNHHHMMSSSSTTTTTTTTTUUUUUUUUUGGGGGGGGGGDDSDDDDDDDDDDDHHHHHHHHHHHZZZZZYYYYYYYYUUUUUXXXXXXXXXRXXXRRRCCCCCCCAAAAA
        PPPPPPPPPPPPPPPGGGGGGCCCHHHNNNNNHMMMMMMSSTTTTTTTTTTTUUUUUVVGGGGGGGGGGDDDDDDDDDDDDDHFHHHHHHHHHHZZZYZYYYYYYYYYYUXXXXXXXXXXXXXXXXRRRRBCCCCAAAKA
        PPPPPPPPPPPPPPGGGGGGGCGHHHHNNNNNHMMMMMMTTTTTTTTTTTTTTUUUUUUGGGGGGGGGGGDDDDDDDDDDDDHHHHHHHHHHZZZZZYYYYYYYYYYYYUXXXXXXXXXXXXXXXXRRRRCCCCCAAAAA
        PPPPPPPPPPPPGGGGGGGGGGGHHHHHHHNNHHMMMMMSTTTTTTTTTTTTTTUUTWUGGGGGGGGGGGDDDDDDDDDDDDHHHHHHHHHHHHZZZYYYYYYYYYYYYYZXXXXXXXXXXXXXXOORORCCCCCCOOOO
        PPPPPPPPPPPPPGGGGGGGGGGGGHHHHHNHHMMMMMMSSTTTTTTTTTTTTTTUTWWGGGGGGGGGDDDDDDDDDDDDDDDHHHHHHHHHHZZZZYYYYYYYYYYYMYXXXXXXXXXXXXXXOOOROOOOOOOOOOOO
        RRPYPRPPPPPPPGGGGGGGGGGGGGHHHHHHHHHMFMSSSTTTTTTTTTTTTTTTTTTYYGGGGGGGDDDDDDDDDDDDDHHHHHHHHHHHHZZZZZYYYYYYYWYMMXXXXXXXXXXXXXXXOOOOOOOOOOOOOOOO
        """;
}