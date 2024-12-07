﻿namespace AOC2024;

public class Day07
{
    [Theory]
    [InlineData(Inputs.Sample, 3749)]
    [InlineData(Inputs.Input, 465126289353)]
    public void Part1(string inputString, ulong expected)
    {
        List<(ulong result, List<ulong> values)> equations = [];
        ReadOnlySpan<char> input = inputString.AsSpan();
        Span<Range> splits = stackalloc Range[2];

        foreach (Range lineRange in input.Split(Environment.NewLine))
        {
            ReadOnlySpan<char> row = input[lineRange];
            row.Split(splits, ':');
            ulong r = ulong.Parse(row[splits[0]]);
            List<ulong> values = [];
            foreach (var valueRange in row[splits[1]].Split(' '))
            {
                if (row[splits[1]][valueRange].Trim().Length is 0) continue;
                values.Add(ulong.Parse(row[splits[1]][valueRange]));
            }
            equations.Add((r, values));
        }

        Func<ulong, ulong, ulong>[] operators = [(a, b) => a + b, (a, b) => a * b];

        ulong totalResult = 0;
        foreach ((ulong result, List<ulong> values) equation in equations)
        {
            List<ulong> results = [];
            Calculate(equation.values[0], equation.values.ToArray().AsSpan()[1..], operators, results);
            if (results.Contains(equation.result)) totalResult += equation.result;
        }

        Assert.Equal(expected, totalResult);
    }

    void Calculate(ulong acc, Span<ulong> remaining, Func<ulong, ulong, ulong>[] operators, List<ulong> results)
    {
        if (remaining.Length > 0)
        {
            foreach (var op in operators)
            {
                ulong newSoFar = op(acc, remaining[0]);
                Calculate(newSoFar, remaining[1..], operators, results);
            }
        }
        else
        {
            results.Add(acc);
        }
    }

    [Theory]
    [InlineData(Inputs.Sample, 11387)]
    [InlineData(Inputs.Input, 70597497486371ul)]
    public void Part2(string inputString, ulong expected)
    {
        List<(ulong result, List<ulong> values)> equations = [];
        ReadOnlySpan<char> input = inputString.AsSpan();
        Span<Range> splits = stackalloc Range[2];

        foreach (Range lineRange in input.Split(Environment.NewLine))
        {
            ReadOnlySpan<char> row = input[lineRange];
            row.Split(splits, ':');
            ulong r = ulong.Parse(row[splits[0]]);
            List<ulong> values = [];
            foreach (var valueRange in row[splits[1]].Split(' '))
            {
                if (row[splits[1]][valueRange].Trim().Length is 0) continue;
                values.Add(ulong.Parse(row[splits[1]][valueRange]));
            }
            equations.Add((r, values));
        }

        Func<ulong, ulong, ulong>[] operators = [
            (a, b) => a + b,
            (a, b) => a * b,
            //(a, b) => ulong.Parse($"{a}{b}"), duh slow
            //(a, b) => a * (ulong)Math.Pow(10, (ulong)Math.Log10(b)+1) + b, // Log10 is slow as string concat
            (a, b) =>
            {
                ulong bl = 10;
                while (bl <= b) bl *= 10;

                return a * bl + b;
            },
        ];

        ulong totalResult = 0;
        foreach ((ulong result, List<ulong> values) equation in equations)
        {
            List<ulong> results = [];
            Calculate(equation.values[0], equation.values.ToArray().AsSpan()[1..], operators, results);
            if (results.Contains(equation.result)) totalResult += equation.result;
        }

        Assert.Equal(expected, totalResult);
    }

}

file class Inputs
{
    public const string Sample =
        """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """;

    public const string Input =
        """
        20261572812: 98 138 31 2 666
        45327: 8 9 335 97 87
        829287: 99 13 816 7 1 744 9
        231630061: 460 2 33 93 500 1 52 8
        651017: 713 52 851
        30996182: 4 107 24 60 50 60 182
        16545464: 810 5 1 85 3 6 4 6 19
        146327: 7 4 8 7 7 1 1 3 73 466 7
        326: 6 4 5 7 91 4 62 8 85 7 3 9
        14687269: 7 63 8 5 6 337 1 857 1 3
        1730: 82 7 613 522 21
        6511850: 626 2 36 4 1 8 9 9 1 665
        13883777: 43 3 923 109 3 1 1 11
        19083747060: 546 3 33 44 5 5 38 3 6 7
        1568: 84 9 71 662 6 63 9 1
        203: 72 48 4 70 9
        157: 70 83 4
        1494863426: 249 6 2 4 2 748 9 4 3 4
        416767: 392 19 52 90 9 55
        60088: 8 1 2 723 12 67
        3169: 25 98 3 685 31
        11301912270: 2 680 336 4 5 6 9 5 274
        2292: 29 418 11 5 2
        48651: 40 304 4 5 6
        33745743: 49 3 9 890 9 1 846 9
        867714120: 374 4 6 5 327 234
        26733890742: 8 95 2 8 4 729 45 6 1 6
        4511655425: 11 59 4 94 613 3 64
        44441: 92 524 72 81 7 1
        7991: 6 5 9 1 764 7 7 5 733 1
        278337: 8 7 248 8 4 5 375 6 30 6
        476229876539: 94 3 1 9 8 9 29 9 1 5 7 39
        1733207525: 13 2 525 3 19 6 301 25
        1466117: 8 17 9 509 846
        1189: 4 5 5 3 889
        7817084617971: 781 708 4 570 47 9 72
        151614: 16 4 3 5 92
        8682: 2 8 86 3 24
        7446697: 35 71 38 7 97
        8668844903: 2 5 276 5 3 8 1 2 6 71 8 3
        22949069796: 2 815 9 8 9 7 6 73 6 8 9 4
        73257040: 4 54 1 3 9 7 7 6 21 3 7
        2434621: 629 289 34 1 78 85
        135: 20 7 5
        213408: 1 369 86 234 2
        106817943544: 2 74 29 852 61 50 14 4
        50897732: 53 4 3 2 8 98 66 5 15 74
        481687: 63 545 18 44 151
        866107837: 68 962 5 662 4
        21651: 1 7 4 3 6 5 2 5
        1867497: 33 5 629 3 784 6
        658564: 2 6 4 6 53 42 4 4 4 2 2
        1286: 6 327 878 70 5
        150654: 7 53 20 2 52
        32009509009: 619 108 3 6 29 95 7 4 9
        128298588: 8 2 5 77 4 8 9 8 65 2 8 12
        106971480: 1 6 7 23 5 88 6 7 4 85 99
        1298: 838 4 118 5 333
        109869: 30 95 499 68 4 3
        9534854: 9 47 9 1 5 628 449 36 1
        16008: 295 7 1 948 105 884 4
        7800300697: 60 9 37 8 450 231 3 4
        19656: 93 6 4 53 42 3
        3331320: 17 7 6 124 80 744 85 3
        2023999391: 681 118 4 7 7 4 514
        239993784: 7 41 2 5 527 191 7 948
        769876: 4 6 6 285 8 5 9 1 1 5 9 1
        730784: 545 6 6 41 32
        94876096665: 24 2 7 20 1 6 1 7 4 2 665
        459334762: 2 2 9 93 2 3 8 9 412 8 2
        6027067800: 3 27 695 221 120
        42749: 4 7 399 44 5
        137175: 84 9 59 25
        16830002448: 337 6 2 9 3 7 8 17 6 477
        473075680: 453 64 172 76 70
        950398: 63 8 119 5 398
        3374757296819: 721 4 4 9 71 514 130
        8906411904: 43 834 398 20 624
        89494: 6 2 451 8 9 634 3 1 6 7 6
        385612430: 10 7 677 103 79
        226663: 1 7 5 11 280 2 2 1 5 69
        7019457032: 2 83 3 809 1 41 826
        117829733: 332 846 297 22 6 4 1
        4293: 47 5 61 8 4 576 92 9
        27956: 4 3 50 13 6
        1698430: 2 4 737 4 72 226 156
        904: 46 9 2 654 52 20 68
        592947912: 1 219 8 5 903 8 3 83 8 3
        594519: 59 1 4 51 9
        82300: 7 2 113 64 50
        18655560: 3 365 66 1 84
        183864: 4 9 80 95 978
        4549914: 4 6 46 9 1 582 8 4 952 9
        627883: 6 278 8 3
        18928824: 3 5 6 262 81 4 2 24
        57994: 1 4 95 2 15 73 2 1 31
        1759022: 39 1 8 5 42 8 493
        5033908: 23 706 3 31 35
        556387631: 7 7 54 90 3 6 67 6 3 32
        1458452: 2 3 1 4 90 158 678 9 4
        746735089: 45 775 519 2 67 91 3 9
        96495380: 4 85 303 17 6 1 4 149
        275290785: 1 5 3 8 42 4 5 505 4 6 5 9
        9455765: 424 946 4 69 4
        603870572: 903 86 6 6 6 9 491 2 2
        2110: 51 114 91 8 62
        2463: 3 303 756 2 3 1 791 1
        65915810695: 65 827 88 810 696
        185882150: 1 8 58 821 47
        95795337: 86 254 617 953 37
        159378123309: 8 9 7 6 1 520 82 20 5 3 3
        480856880: 9 7 73 6 670 3 8 1 6 4 8 7
        2592: 75 176 8 5
        123664181: 1 8 391 8 4 644 6 2 723
        29321568: 834 366 189 96 1
        195388: 903 36 6 277 63
        4824530231: 15 75 7 497 36 30 231
        33868: 5 3 78 4 5 6 1 53 1
        734046: 81 65 44 15 114
        14871045810: 619 6 4 2 1 148 45 81 1
        3448940: 774 88 4 931 9
        48501274: 61 5 67 9 5 6 9 45 88 6
        7122124803: 6 105 8 92 16 480 2 3
        56015: 62 14 9 9 7
        63596840304: 3 3 91 27 92 5 15 2 2
        17858156412: 7 3 49 450 9 7 540 9 3
        390165: 6 3 8 22 316 6 2 4 9 37
        1276750: 50 6 51 1 8 28 9 98 4 6
        9471132: 816 67 2 3 633 3 132
        5998115: 51 4 4 9 6 816 515
        405042823: 54 75 42 823
        270329888710: 6 3 38 9 5 8 8 72 4 7 10
        9011938: 5 3 5 1 7 36 36 3 7 9 3 20
        660: 4 75 581
        1341831680: 4 7 258 141 88 56 560
        22831300800: 7 4 876 5 591 80 7 5 9
        6298914: 5 113 64 8 914
        29294: 3 9 76 876 2 11
        237239168: 35 7 1 8 2 828 85 7 67 1
        1802251877126: 15 2 267 25 2 9 77 129
        11008: 4 865 554 2 96 74
        642251445: 64 22 5 144 5
        2064: 3 2 78 543 7 9 5 2
        13773890668: 71 833 4 7 7 9 528 7 78
        28935437: 12 51 7 219 353 6 7 7
        2400515: 480 5 4 55 60
        761: 12 8 84 53 528
        3640: 20 6 480 7 20 78
        60299935213: 472 589 964 225 6 4 3
        224408: 8 8 9 1 8 29 2 95 8 2 5 3
        2046614307: 7 55 3 12 9 25 14 307
        31922850999: 6 9 1 19 1 118 7 9 4 5 6 8
        83457941113: 84 300 2 5 1 69 99 6 13
        608409710: 58 4 6 1 4 52 7 41 55 2
        819044459: 7 975 3 705 2 6
        10019018: 93 43 97 430 18
        2726736: 664 958 82 21 2 8
        849157: 996 2 9 5 1 2 1 1 1 7 421
        7278520: 1 2 981 6 3 2 1 9 4 8 5 61
        118060806: 373 11 5 143 43 6
        20902: 70 81 2 69 32 5 26 1
        639450: 359 70 6 435 735
        41696: 5 212 8
        7392: 3 765 3 453 7 1 6
        4249867874: 62 1 9 1 19 7 58 69
        32408375: 32 40 832 5 5
        1609379: 38 605 7 8 1
        1045: 9 67 6 7 956
        3060188: 8 606 1 818 9 2 6 5 8
        288224602: 1 942 3 773 2 32 730
        119: 4 3 93 8 6
        26606: 8 49 7 671 3 9 6 4 5 633
        70567960: 42 210 9 95 8
        11162592: 89 9 9 493 12 98
        1312192163295: 79 25 1 193 83 8 6 8 95
        23027: 876 2 542 8 7
        92299: 201 457 443
        73140250: 102 581 713
        134246144: 61 8 87 93 8 34
        44: 9 3 8 22 3
        66755594563: 55 62 9 1 1 4 42 3 6 4 4 3
        1235240: 9 8 90 2 69
        1167032: 9 40 2 7 38 4 7 9 7 9 73 2
        2620881661: 8 7 362 3 9 6 6 656 5
        187793: 8 179 692 98 1
        47794: 3 69 3 43 44 65 8 76
        284165701644: 99 706 353 701 644
        335162944: 57 6 98 2 944
        169260: 6 70 403
        178035795: 38 34 9 7 9 40 6 3 755
        66948420: 21 7 6 5 43 924
        2479370: 247 93 50 3 16
        6967886707: 696 7 88 6 705
        31062240000: 49 1 6 5 53 8 4 5 74 33
        8117959754763: 2 701 4 98 6 584 9 2 1 3
        14179652599: 3 70 50 492 2 94 84 7
        8834: 29 3 69 4 2
        211195608: 67 686 1 48 8 39
        9141370: 9 9 358 7 6 1 5 41 9 1 7 7
        398636: 12 5 60 4 4 6 1 322
        45629: 427 4 18 86 588 42 4 6
        209140108: 701 1 16 49 7 38
        54854749: 3 8 8 31 1 36 3 8 2 2 9 7
        36782309: 3 59 8 80 2 307
        5143959332411: 1 57 15 5 9 9 332 402 8
        1602608: 3 91 8 5 349 9 2 3 688 5
        37788: 6 8 7 687 3
        283577: 1 9 13 263 995 2
        115730: 9 3 2 124 26 2 56 8 9 2
        20700: 48 2 2 7 49 3 6 7 991 53
        276460: 25 6 6 415 3 6 70
        3271850: 322 4 392 793 10
        188703367670: 5 46 9 1 6 9 8 8 766 9
        126394406: 141 169 6 649 7 880 6
        43849535: 2 1 58 36 94 7 52 5
        7113117119: 702 9 31 137 34 11 8
        512831269: 9 29 572 229 883 15 4
        886: 4 8 15 700 6
        290874054503317: 5 9 986 8 10 900 663 5
        1150293: 4 185 21 424 9 8 885
        16808387: 45 747 23 5 8 764
        338100965: 7 1 333 5 28 35 8 74 91
        5743: 42 591 84 1 8 7
        1017379449: 233 3 95 9 18 7 6 5 2 6 9
        312579238: 665 2 442 1 89 2 2 991
        436338339683: 55 5 392 436 7 1 1 46 4
        47035605: 55 4 63 37 1 2 342 3
        85619743907: 594 144 83 743 907
        924237690: 1 8 1 37 6 9 34 11 7 688
        43212856321: 65 367 1 2 856 276 45
        5352: 506 7 39 7 9 10 1 8 303
        244908: 27 62 917 23 3
        136335660: 8 865 3 8 549 37 6 41
        30147022: 597 8 8 789 910
        267424: 1 932 3 2 6 1 5 68 1 137
        651: 6 46 5
        87604475: 8 1 725 4 236 79 2 8 9 2
        11278205567: 765 7 7 490 8 724 36
        30585804: 367 926 90 1 24
        19147: 9 69 245 34
        10826820: 520 7 522 33 68 15 5
        12150: 81 2 75
        4889: 80 6 1 5 3 9 9 1 9 380 18
        120193530: 39 642 6 2 34 4 6 9 6
        207977: 10 5 6 117 977
        1324: 4 66 5 977
        129600605: 2 5 4 331 720 7 5 3 252
        33300996: 925 36 93 6 5
        1551665414: 888 8 1 5 7 569 8 3 1 4
        1281554136: 7 62 90 6 837 487
        121034377174: 6 71 2 103 953 6 763 5
        7706354167554: 7 539 475 3 878 43 9 8
        17506596: 29 6 1 59 31 8 327 5 4 4
        413531: 9 1 19 33 2 86 9 8 899
        20895: 3 77 9 7 8 3 4 75 8 6 1
        408404: 44 7 1 327 6 3 62 9 4 6 6
        10722: 97 6 5 6 6 3 3 4 4 1 23 8
        996: 1 7 5 965 1 6 7 5
        507325: 6 83 5 88 25
        29347686: 4 2 89 9 424 8 4 1 25 8 3
        1356080: 3 129 876 8 4
        518639: 3 9 4 5 3 7 168 7 7 9 2 5
        68790276: 4 74 115 2 1 6 7 9 71 5 4
        27608836421: 74 23 112 303 478 2
        1866792852: 9 2 6 5 2 6 7 291 57 5 4
        3227072784: 34 858 526 57 912
        408841: 8 6 833 2 5 72 2 3 7 56 6
        625: 82 77 6 5 453
        710281278: 493 635 6 8 6 707 9 6 7
        7947543334: 52 6 6 7 4 908 4 5 32 2
        13287971: 8 1 3 4 4 797 2
        920304107: 4 44 9 581 107
        145: 2 6 4 95 2
        1221276: 628 4 23 3 84
        3152042325: 8 44 183 5 895
        9004631: 878 46 64 31 294
        32755464: 83 7 9 62 4 426 63
        465434: 763 61 3
        117023125: 95 9 11 871 949 3 9 4 1
        5489531364874: 871 3 526 8 7 84 9 7 9 4
        227647935510: 573 82 158 3 17 95
        987686437: 289 89 64 6 37
        489723554: 580 3 84 3 554
        2980101: 348 83 916 94 9
        932: 38 4 82 1 698
        10720145: 9 9 2 1 3 913 6 4 31 9 38
        1582199043: 331 239 95 2 44
        4939116: 488 5 845 66 6
        12960: 533 16 6 791 274 8
        1131: 6 5 4 7 893
        3956427580: 63 628 275 83
        579603: 5 13 72 45 920
        120: 2 44 31 1 1
        707973546: 698 9 969 3 6 50 7 889
        82171: 81 407 764
        59078: 146 444 74 1 4
        372416: 43 78 364 6 14
        283157070382: 1 5 1 73 6 547 4 4 7 9 8 4
        127665181: 753 6 892 46 513
        3931604: 48 840 233 19 428
        2922: 2 2 6 42 9 304 8 2
        1628538200: 2 833 6 97 7 7 7 40 251
        18933: 3 5 4 7 2 6 7 5 3 1 870
        1983470022326: 85 1 1 3 8 5 72 2 5 6 97 6
        1518804: 47 883 650 38 9 4
        206771: 533 223 932 896 6 8 4
        14944382: 55 3 94 1 15 936 9 323
        420277: 87 5 9 133 987 7 3 1
        3451113: 75 46 3 35 778
        24826110: 973 81 9 5 7 15
        2273099665: 5 7 2 5 690 397 735
        2236642944: 64 6 9 46 835 96 79 68
        184786375236: 229 83 38 804 36
        3354043148: 7 93 846 609 8
        45799: 59 10 64 3 7
        29664054396: 886 310 6 45 72 6 2 4 9
        22212: 60 256 7 8 56 28
        320339168: 20 8 161 14 94 750 2 8
        207560639: 8 597 883 85 31 2 70
        5549006: 384 76 94 900 6
        638281767: 97 5 96 6 654 3
        14219040: 7 351 2 6 54 2 1 6 32 5 3
        80115: 11 581 67 366 6 1 2 43
        12344046: 78 4 2 8 21 2 117 74 2 2
        25789271222525: 5 9 9 70 73 86 67 5 863
        6248074: 37 584 3 80 75
        69253: 9 3 1 33 8 9 3 24 6 3 616
        5420650221: 45 6 1 20 6 491 4 108 1
        82962: 5 2 69 7 7 729 3 3 3 6 1 3
        28999: 604 13 47
        1065075928: 80 2 3 99 4 664 1 6
        127772: 77 823 401 75 2 78
        130939215: 691 9 6 84 21 1 9 8
        7660: 935 6 2 8 115
        451212: 230 7 42 6 19
        2247886690: 51 1 4 8 452 6 6 667 70
        1811035556: 9 9 82 5 899 51 79 56
        4109365120: 7 62 932 476 20 316
        17089560: 4 5 8 43 3 43 6 512 10 9
        22982: 8 55 52 93 9
        186: 2 8 3 28 145
        1377312127: 5 655 33 24 8 531 70 9
        24621407: 548 52 2 432 863
        57879912: 2 24 63 5 41 1 1 3 8 4
        9319523: 224 416 8 3 23
        210304: 4 162 9 9 74 14 4
        87226: 96 3 3 2 80 7
        2148130634: 20 77 642 3 22 34
        112797844: 60 1 687 151 8 44
        34875: 44 9 2 634 5
        704382941: 3 9 462 6 9 3 2 8 1 533 1
        514339: 5 1 4 33 7
        91172: 3 34 7 7 779 805 3
        10679126102: 7 7 410 3 2 99 8 671 62
        917320: 32 3 4 710 1
        55748: 151 5 7 51 5 1 9 9 4 2 6 7
        982512: 13 3 221 459 9 376 9
        434145: 3 3 8 97 8 99 5 6 8 7 61 8
        1472458: 20 5 4 1 64 88 6 244 66
        152: 78 4 8 2 60
        1241: 22 2 83 80 6
        2740059: 48 8 6 2 41 61 63
        528: 494 32 2
        4800: 57 138 26 79 16
        1322689: 9 4 96 268 9
        987420: 4 89 693 26 34 84 4 6 6
        6243825: 8 615 1 3 825
        2466861957: 881 7 4 42 7 183 9 54
        4914: 3 2 819
        11451: 94 96 3 20 51
        23964156: 78 38 3 396 54 1 5 7 1
        2163758589: 478 21 710 4 29 433
        5237550860: 7 828 6 229 5 9 43 20
        338665: 32 6 7 78 6 7 6 6 4 9 664
        178: 3 7 9 80 8
        2147171: 69 1 75 7 7 7 9 4 3 83 4 5
        1605002: 27 9 71 5 93 8
        256854564: 20 38 803 59 6 918
        291006063: 8 6 4 6 2 505 20 9 6 5 4
        3639475198: 42 13 9 129 862
        77155930: 76 88 27 59 30
        3946048: 95 472 44 33 4 2 9 7 38
        2160667: 523 1 5 96 797
        165008: 8 456 5 5 89 9
        18151435: 8 37 9 53 395
        5369239218: 8 59 8 9 238 1 215
        5049390643: 7 651 7 4 52 591 8 9 43
        56719056: 9 568 33 7 4 1 762 8 48
        451791000: 33 46 19 8 58 30 9
        2603596800: 7 779 8 629 668 640
        1242803851704: 891 9 58 20 5 69 1 703
        19728171: 543 5 4 9 17 1
        726115: 7 1 2 3 336 5 20 5 2 6 9
        197421: 4 7 7 4 80 345 2 594
        342622733: 48 235 81 630 7 733
        611736748: 972 247 91 97 28
        5709688746: 6 8 20 6 8 8 7 9 84 59 93
        26935: 8 7 13 267 9 3 1 1 51 8 9
        1110319: 3 408 25 6 2 9 619
        1729: 728 8 984 9
        1516073592: 24 477 4 3 578 64 3
        10192: 20 38 5 2
        4199874129612: 658 8 64 668 724 9 7
        4463235120291: 949 8 57 973 5 106 91
        469476: 2 6 2 158 61 2 683 6 6 7
        5635257: 9 6 2 29 3 6 52 2 3 5 1 6
        139207: 1 870 2 8 7
        2768307332: 477 5 4 7 9 2 49 1 94
        115373519: 7 279 65 8 6 98 4 4 3 4 9
        13997490486: 5 7 7 1 5 7 570 30 9 3 91
        1680: 20 461 2 718
        1270: 135 484 80 563 9
        2821947: 4 6 5 6 56 2 9 6 895 7 3 2
        825506: 1 77 28 3 382 6 98 3 4 8
        393493259: 577 828 3 33 28 5 5 9
        7301408: 145 67 35 41 6 4 128
        63195971622: 1 51 6 633 5 3 9 9 4 3 8 3
        2782104: 662 229 7 52 6
        569: 8 17 537 3 4
        146904291: 20 7 785 707 297 1 1
        775581: 692 42 51 26 38
        72170: 8 28 8 22 248 35
        15580: 32 7 60 91 82
        15137565: 8 2 1 3 37 267 667
        4932150: 9 2 9 4 39 56 3 171
        25927855: 81 4 8 765 20 2
        79854: 7 1 3 7 9 94 3 3 20 8 9 6
        22979158: 476 9 3 6 8 327 70 3 4
        127781982: 47 80 5 268 28 30 3 43
        2102: 5 4 83 9 295 2
        105687: 32 33 13 4 70
        36448653989: 36 44 865 321 3 776
        170271265: 27 6 8 270 36 2 903
        14111307: 2 1 5 2 2 21 5 265 64 7 9
        34356195344: 4 966 68 794 9 656
        213840: 55 8 9 6 9
        919113216: 74 6 6 4 6 7 1 9 3 9 888 7
        4412885624: 7 500 5 17 9 7 9 85 27 2
        109: 1 4 98
        9371772: 2 2 4 4 2 485 26 6 1 8 3
        816: 3 41 43 9 6 8
        372878004: 997 85 440 4
        1080585: 731 342 5 1 6 9 2 96 1 2
        200215: 77 2 4 90 63
        618205474: 3 9 21 3 245 70 472
        814900: 76 13 49 87 725
        19021857936: 13 2 5 2 8 7 2 9 4 977 9 7
        7647520449: 4 79 5 9 3 5 3 83 7 8 3 97
        27673593644: 7 611 2 6 7 59 308 4
        5303585976395: 86 3 46 67 6 988 388 9
        4761: 2 4 8 7 2
        47095296: 4 969 5 5 2 3 2 3 8 3 54 6
        540397305: 4 55 983 204 3 734 81
        29687553: 92 4 537 579 6 81
        1323: 41 295 988
        7119955: 2 637 9 3 57
        25443072: 96 704 18 576 1 54
        5959: 9 725 10 8 7
        4114: 3 5 257 131 35 93
        82044355016: 92 332 3 93 95 4 7 76 4
        294366037: 4 56 11 247 63 33 5 89
        214663955220: 2 5 29 7 7 901 429 30 6
        3262: 50 9 16 7
        11457938: 416 5 486 56 2
        906440: 19 1 75 60 34
        2191342561199: 330 8 5 444 4 153 975
        437950713: 145 7 6 9 8 50 713
        246758: 92 9 297 781 61
        82810: 12 975 58 72 7
        7891: 73 11 578
        1255272: 125 42 9 81 91
        27973: 3 23 4 79 11 282
        68667: 678 3 6 77 7
        136624821: 673 29 831 7 1
        41550483: 74 9 55 6 58 687 853
        309186514: 2 3 6 98 6 545 447 7 91
        8249343115: 94 70 12 87 3 109 6
        399336: 62 8 11 22 226 8 7
        795786: 4 89 439 7 89
        298384: 83 7 1 7 9 42 8 7 6 2 6 4
        11562407: 6 9 8 4 16 5 1 5 7 6 240 5
        186661952: 9 7 274 698 61
        34885431: 387 508 56 40 3 917
        2067520: 89 16 841 7 910
        128753: 45 4 3 93 7
        511005: 8 8 43 857 7 5
        28633297: 78 29 892 3 97
        63180: 46 596 44 91 3 81
        990793851: 7 5 9 2 4 8 3 39 3 6 3 51
        16421139602: 9 67 3 6 4 27 6 1 69 6 1 2
        139: 6 8 91
        1649833: 3 3 738 902 833
        2671786336: 26 717 7 7 75 858 6
        10141397: 55 790 4 3 44 9 951
        2464139376: 734 563 5 9 31 9 586 4
        424715: 9 4 501 9 869
        81979110546: 910 879 90 54 5
        2986635261: 1 70 96 4 61 88 599 3
        2152893601: 6 9 1 6 2 97 85 492 8 5 3
        61426224: 4 3 85 8 8 978
        40496968: 3 1 5 5 9 847 49 9 7 84 2
        30897645282: 8 6 55 921 933 33 7
        22188546846: 9 8 2 40 9 3 136 7 4 47
        18083286: 2 5 4 78 1 522 44 24 1 2
        33589182: 5 36 96 16 43 533 9 46
        538: 4 91 7 80 4 77 6
        2803584: 9 97 9 7 6 3 73 8 2 9 5 56
        8723: 9 78 1 2 3
        52823506: 528 23 494 9 2
        53738020: 99 68 53 110 72 820
        205872: 2 71 5 5 2 2 9 8 7 1 855 8
        676868645553: 6 685 83 686 1 45 550
        193662: 5 6 4 8 1 18 8 5 929 5 1 2
        80168990: 2 8 1 1 20 8 7 4 87 8 55 1
        39364: 7 656 1 49 8 260 8 84
        5957812: 52 5 9 4 7 791
        10356: 79 4 63 6 12
        34261: 4 54 9 5 6 4 5 491
        749180: 23 9 137 676 95 797
        4663718: 6 78 1 474 3 1 743 7 38
        1925011: 40 59 4 814 715
        52506566150: 6 2 7 5 6 3 2 9 6 661 50
        8945777399: 6 7 90 5 3 15 3 15 2 39 9
        3028272604: 484 8 638 552 604
        188437635: 5 436 5 4 9 72 8 12 2 1 1
        181156501: 6 48 31 50 567 1
        325638624: 8 3 800 44 4 841 60
        8032850: 79 52 7 73 767 19 64
        433839131280: 2 1 3 3 8 7 6 5 454 10 2 4
        86385975: 69 36 1 1 535 75 65
        13686: 46 2 280 1 245
        1712014581552: 341 2 621 4 44 11 57 4
        1489310550: 78 449 9 9 525
        255151481: 54 51 1 27 5 18 69 78 8
        63232569: 23 47 98 94 16 4 569
        15505: 8 35 30 1 5
        489767: 5 960 102 89 78
        325626915: 395 6 8 914 68 2 9 5 1
        14129837307: 28 90 7 239 89 652 9
        764: 721 5 33 6
        4223: 7 463 56 919 7
        166163510506: 16 531 6 847 510 506
        587419833297: 3 37 6 882 9 69 33 2 97
        399277947: 31 8 927 7 664 2 281
        170242241098: 49 39 425 38 8 13 907
        5668108: 9 227 545 6 58
        203298244380: 6 69 493 315 964
        33496949455: 809 49 35 1 845 6 249
        501967675: 7 9 1 784 5 3 6 6 7 62 5 5
        1876910: 5 4 6 2 8 4 389 5 6 708 2
        2965650: 3 707 8 50
        118205: 95 51 42 2 99 4 2 261
        100089048: 8 4 2 70 3 4 4 7 825 2 3 8
        46834203: 9 4 41 9 6 135 4
        88282419: 44 2 282 4 17
        1181936466: 1 6 3 9 72 22 94 7 1 6 7 5
        16346: 8 7 4 86 6
        79124454: 6 4 6 3 2 4 20 310 3 9 2 3
        8135081494: 8 1 1 338 7 6 938 3 8 6 8
        333717154: 3 918 472 3 530 3 65 4
        58432: 3 900 2 8 64
        26363: 9 7 82 269 1
        2099820378: 443 5 948 375 3
        12057: 632 2 19 3 8
        147361111: 917 7 7 49 78 3 4 6 3 34
        1045885343: 7 249 6 853 46
        170369280: 87 4 7 6 24 2 46 120
        669944977: 331 46 44 97 7
        257509: 263 583 5 61 60 49
        1757606033: 258 4 7 17 2 3 67 4 562
        4072900: 6 65 918 216 4 845
        984: 2 4 77 866 17
        1530342088: 128 6 17 140 28 389 5
        171554880: 128 480 8 346 56 588
        1018072: 2 4 8 9 1 87 598 9 88 1
        2349979998225: 78 332 66 57 9 3 223
        3024234657: 64 2 95 6 17 41 1 48 55
        1515192: 621 1 29 7 4 3
        305325282: 272 33 31 91 61 82
        54685: 7 18 9 3 1 2 4 9 7 82 38
        71892: 3 9 69 79 98 9 5 7 93 4 9
        221909351484: 78 8 219 359 2 547 6
        799880400: 6 9 4 9 747 250 4 3 396
        26946: 2 2 3 8 1 9 4 221 8 3 4 6
        280535: 7 811 3 341 565 11
        7694: 11 699 3
        77108114084: 2 50 4 320 3 6 4 8 5 21 4
        2144050592168: 1 4 97 6 6 9 3 3 8 402 1 8
        514177: 4 54 6 3 47 2 5 4 4 333 5
        5795806: 49 117 118
        255481250: 4 121 82 997 25
        53713731064: 7 89 7 8 9 4 9 6 3 1 674 1
        3827: 2 64 6 38 7 789
        12069: 302 37 9 9 875
        26340: 5 40 49 633 86 5
        39004: 9 1 66 3 49 40 7 77 98
        199466019: 88 909 2 1 3 5 4 5 5 2 4 4
        94346225618: 548 1 5 879 559 5 7 7 1
        411053: 97 5 6 31 3 3 9 5
        15666: 54 4 72 2 19 3 84 6
        7685786934: 768 5 713 73 919 14
        2639: 565 1 804 487 783
        116657452: 5 5 29 9 8 4 9 7 2 3 5 2
        391689: 1 3 4 1 240 96 9
        20629030: 3 3 685 4 96 49 30
        92190575: 9 85 162 357 798 57 5
        200187465: 5 3 7 2 7 1 7 470 7 298 6
        841: 7 2 5 5 491
        2124: 3 593 2 343
        36610063992: 2 43 93 9 4 918 554
        114822786: 181 97 654 9
        10260285: 5 1 371 137 200 2 85
        16025920: 7 6 188 21 1 9 638
        17613507: 6 15 3 45 9 807 3 8 6 3
        239580: 1 83 310 2 484
        2027025: 779 40 55 3 15
        7115144510: 89 890 45 945 283 95
        3388245182269: 9 89 846 679 7 489 54
        1595992599927: 2 85 10 6 3 933 92 6 1
        4531547: 93 5 6 1 1 6 118 8 7 20
        789698706: 635 9 658 6 35 6
        1433136093: 4 9 64 27 5 818 750 93
        424116828954: 580 43 483 918 741
        139600188: 516 991 3 91
        120808840: 9 9 9 7 851 564 6 7 1 7 5
        10581: 48 19 1 673 786 4
        82117538: 5 1 79 33 5 9 94 63
        27964789: 21 257 656 987 4 86 5
        956: 9 35 21
        3163734: 554 79 833 6 1
        583: 3 1 9 571
        1161556284: 3 427 5 9 8 1 4 6 9 73 1 8
        605316855: 867 9 37 65 29
        7787780235: 7 9 6 382 7 41 5 303 3
        1267497: 2 13 7 3 400 6 4 2 3 2 4 5
        21273569: 96 695 55 78 5 4 337
        5218526406: 700 35 710 88 30 8
        6422539915: 8 896 896 8 109 2 18
        7471037503: 965 9 959 897 2 746
        1760697625: 5 2 7 60 697 623
        104951442: 954 22 4 8 3 7 2 1 6 59
        796965533: 29 3 4 34 4 7 2 84 5 612
        2637180: 90 25 49 6 78
        65441892692: 6 544 18 92 692
        2055416: 63 303 62 48 3 96 2 9
        3726049: 8 88 3 84 154 481
        192424: 9 71 8 248 3 8 11 21 8
        114575369: 387 925 73 32 9
        730: 4 4 2 66 8 58
        894217550: 565 989 8 801 6 8 6 5 5
        126551425: 2 371 131 652 97 385
        4561: 4 8 5 116 8 9 3 31 5 1 9 2
        85412348: 773 3 9 3 7 7 1 79 6 8 7 4
        8653448: 6 989 27 31 1 6 9 2
        38808424190: 10 485 93 8 4 302 4 8
        8051: 3 80 97
        5219: 974 114 4 8 15 6 830 8
        15324: 90 2 82 4 560
        6746536845: 70 878 8 8 711 45
        1191528: 1 6 289 260 72
        177897089474: 8 7 68 8 555 43 75 51
        2678692: 267 869 2
        651553: 2 2 6 2 18 1 9 352 69 5 8
        143: 92 7 30 8 6
        233481: 3 602 438 3 900 4 44
        230845835: 5 225 796 1 45 4 829 5
        616135338477: 13 868 46 1 7 6 91 479
        19341080: 48 35 4 50 1 3 567
        8588906: 478 3 6 4 93 2 8 147 7
        885519043205: 790 76 118 4 9 40 71 3
        3872: 5 3 226 482
        3514395790: 5 2 3 9 3 3 5 8 95 741 49
        53662: 55 97 1 5 97 8 42 149
        5088: 26 4 48 89 7
        359229: 1 437 12 8 29
        36437462: 7 3 49 13 936 1 4 4
        86296: 3 1 6 5 5 2 2 2 553 7 21
        131253470: 201 653 47 2 5
        5445324: 2 10 5 508 9 9 5 958 24
        60870634648: 5 806 8 72 8 4 9 472
        16988: 2 75 261 41 137
        53595: 45 936 8 6 12 2 7 9
        922311575: 184 462 315 5
        6210: 161 3 43 3 2
        23761847403: 8 1 238 9 33 7 23 6 6 7 9
        69625: 9 10 6 7 47 2 458 7 2
        27763: 3 23 9 41 1 747 8 4 3 8 8
        3052647: 5 65 438 4 647
        3381704: 59 5 1 164 9 4 206 1 3 2
        4970: 5 21 47 6 31
        118: 98 2 4 8 6
        8316014: 1 971 19 84 13
        3465: 138 5 10 32 2 1 2 528 7
        3530332260: 8 7 2 767 910 9
        12063293616: 6 4 5 62 1 2 86 1 7 1 5 16
        5536: 7 37 87 16
        1179238531713: 815 95 78 651 222
        27708: 2 7 175 5 528
        3879901: 3 746 69 51 75 1
        5067983: 5 3 3 4 154 1 12 5 3 608
        42455989: 4 9 9 30 5 7 1 794 7 8
        359560160: 5 425 56 10 41 16
        5120: 493 7 13 776 880
        87496046313: 910 9 1 1 95 5 5 4 6 315
        60686: 319 5 2 38
        106937025061: 9 276 5 827 3 250 61 1
        29347: 111 257 503 2 315
        614534: 6 8 6 3 947 4 878
        24659: 465 7 51 585 2
        2296436: 635 55 832 29 4
        6149400: 80 25 85 641 50 148
        1924040510520: 80 6 72 5 6 5 713 93
        220087571: 55 40 8 757 1
        610462400: 6 6 1 7 40 934 76 5
        3531963: 3 957 5 41 6 3
        698721408: 113 4 6 6 9 4 4 8 8 9 4 4
        5148269: 45 880 6 13 191
        628: 141 1 78 192 199 18
        103117571: 91 12 994 95 11
        1484996971: 716 768 99 697 1
        20880: 4 3 58 5 6
        7404612: 79 7 1 86 12
        30842169828: 6 168 3 1 5 1 697 4 1 87
        59759: 79 5 6 47 19 3 7 7 1 8 7
        38429118000: 91 49 91 6 4 435 809
        318576716: 3 44 1 9 815 98 47 7 94
        5897: 7 8 397 29 4 2 4 8 3 8 5 4
        4722: 46 4 82 8 614
        193939725: 6 390 296 280 528
        7908668418: 5 9 3 75 98 4 352 781 2
        1080: 6 9 96 5 525
        78879: 33 42 3 45 640 2 157
        16555109276: 8 3 2 1 6 24 9 2 9 1 346 6
        46600957875: 2 3 7 38 5 61 3 784 7 28
        619697449: 19 1 2 6 86 5 77 1 8 8 9
        17149600: 8 67 267 992 50
        90: 2 2 1 2 9
        567900: 2 59 815 6 2 6 4 9 8 7 9
        179229819: 2 56 7 298 22
        5464512: 1 411 62 80 6 6 7 9 2 8 9
        318552570: 7 325 37 14 7 67
        7281: 1 904 8 35 6
        36284: 36 263 21
        146: 86 1 60
        960991455: 9 60 17 3 4 787 442 4 9
        1636416607: 85 559 205 6 4 7 7
        7640592: 1 6 3 493 738 6 7 1 7 57
        61572395640: 6 1 85 2 1 5 5 43 9 345 8
        316803329: 70 7 6 45 1 1 974 4 6 5 3
        119040: 5 46 43 4 3 1 7 273 2 6 4
        102523960284: 854 366 3 1 3 6 8 8 6 5 4
        1796198: 914 5 3 7 791 92 89
        91670: 66 824 103
        4758: 7 59 9 4 10 498
        2932560: 12 2 1 8 7 6 64 8 1 99 60
        2466642: 24 66 642
        2516: 4 49 3 3 9 37
        39268: 8 49 60 8
        388603: 476 815 639 16 8
        286225867: 21 6 106 25 867
        111798479: 4 31 144 6 3 831 8 4 95
        4432: 1 4 17 4 85 2 4 8
        76038195: 99 3 77 68 7 42 5 527 3
        75741848: 5 40 5 8 2 9 9 1 3 8 7 91
        3223727766: 5 7 1 3 9 7 85 4 1 9 301 4
        259448: 5 147 5 85 2 198
        11318777470897: 4 8 262 41 9 412 9 95
        50547: 4 1 95 21 8 7 97 975 5
        125447831: 32 60 7 651 1 32
        3164824079999: 580 8 12 8 4 860 33 4 6
        621945: 497 283 33 9 85 1
        126774605416: 74 573 297 3 17 6
        12802698: 73 40 81 474 48 9
        6289: 5 1 57 37 49
        22886763: 68 840 67 29 9 5 8
        45506381: 408 531 49 3 70 2 9
        1086077699545: 15 29 686 9 71 545
        96330: 9 7 8 569 735 1
        26524275: 873 51 28 652 17 9 7 8
        17287261563680: 882 49 4 615 63 677
        488039: 4 8 6 3 59 69 7 4 8 8 9 95
        2937672: 82 1 2 4 6 243 394 18
        2991816576: 6 751 2 6 31 6 56 984
        442230660: 1 60 226 6 19 276
        93051972: 368 1 280 71 9 73
        1160: 7 5 131 740 211
        4941982155: 81 61 9 82 14 3 12
        36760: 5 4 5 64 655 9 4 280 4 5
        5418: 3 3 25 68 7 3 1 2 6
        86247005055: 35 1 8 636 8 6 737 29 5
        190642: 5 346 9 8 3 118 3 52 8 2
        434695950: 2 846 36 7 4 4 7 51 3 93
        1911065788: 195 980 6 578 8
        25555915: 57 3 8 73 7 11 524 38 3
        95100: 6 5 8 39 2 4 5 987 4 7 3 6
        28837000: 1 8 97 1 8 584 9 9 4 250
        505569315: 543 931 35 519 1 796
        41223: 6 687 2
        901: 21 6 4 870
        8637229: 991 585 548 75 1
        163576: 162 96 287 60 5 75 1
        962371163023: 213 860 25 83 1 5 3 15
        109350192: 7 57 1 7 9 1 405 258 4 6
        16613281652: 7 383 8 1 85 624 9 5 2
        9596305077: 6 70 5 8 7 401 2 2 6 6 5
        17875933357: 644 63 9 881 5 7 5 8
        89783: 9 6 6 2 15 96 503
        27300689: 3 5 64 1 38 5 2 7 838 9
        764541238248: 9 4 6 9 6 7 1 654 78 1 4 2
        658436: 86 66 2 270 395 4 8 3 6
        8419432: 84 1 94 18 4 8
        859023: 1 9 7 7 3 50 858 8 5 8 7 9
        14951976: 266 30 48 344 39
        59478: 38 2 212 80
        83264: 76 8 3 957 5
        239182: 8 191 31 1 2 1 6
        1108645374204: 8 6 8 4 2 388 3 2 93 20 4
        12300944: 61 5 4 1 83 5 2 5 8 8 2 5
        7204006: 2 90 4 249 145 3 65
        258603731420: 71 834 6 2 3 2 1 3 1 420
        3464130: 86 4 2 41 30
        2125224: 5 812 18 2 9 96 72
        15015078406: 175 858 78 358 49
        17380390: 49 358 9 7 9 41 4 8 8
        9550084: 18 829 8 8 4
        20665420: 9 9 7 3 92 1 2 1 580 6 3 8
        7411523885: 223 3 68 4 152 38 88
        1106464: 389 45 53 32 71
        1180722080: 478 1 245 6 5 2 20 503
        3184109157: 87 602 2 6 770 51 50 7
        113071859: 7 540 22 3 2 29 7 9 2 6 5
        868: 3 7 8 4 35 4 3 7 7 5 283 6
        2153328173529: 951 5 9 452 173 529
        6349117323: 4 62 3 81 5 6 455 174 3
        2309765691: 35 37 18 40 709 2 8 3
        7943: 87 81 26 869 1
        9289401: 7 6 581 2 342
        1562470: 3 4 252 17 182
        3575975392: 3 3 903 216 8 44
        2171436: 21 3 1 20 4 2 5 2 9 8 1 24
        46506582: 95 679 6 5 6 491 88
        136519099056: 8 975 9 571 92 81 3
        2390175: 94 611 15 48 45 5
        8295222: 768 5 8 45 6 73 8 741
        20445495: 61 85 20 785 7
        27369402341: 999 9 6 6 1 8 7 4 2 1 4 95
        15647742: 99 92 174 30 9
        19937073: 36 6 1 2 6 7 20 3 77
        217602: 6 2 1 4 5 730 2 2 552 4 2
        1077224: 78 2 408 33 7 97
        """;
}