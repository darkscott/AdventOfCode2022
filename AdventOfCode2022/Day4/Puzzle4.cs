using System.Collections;

namespace AdventOfCode2022;

public sealed class Puzzle4 : Puzzle
{
    List<ElfPair> ParseInput()
    {
        List<ElfPair> pairs = new();

        foreach (var line in File.ReadAllLines(@".\Day4\Puzzle4Input.txt"))
        {
            var split = line.Split(new char[] {'-', ','});

            ElfPair pair = new ElfPair()
            {
                Elf1 = new ElfJob() { StartSection = int.Parse(split[0]), EndSection = int.Parse(split[1]) },
                Elf2 = new ElfJob() { StartSection = int.Parse(split[2]), EndSection = int.Parse(split[3]) },
            };

            pairs.Add(pair);
        }

        return pairs;
    }

    public override void Run()
    {
        List<ElfPair> pairs = ParseInput();

        int count = pairs.Count(p => Util.ContainsTotalOverlap(p));

        WriteLine(count);

        int count2 = pairs.Count(p => Util.ContainsAnyOverlap(p));

        WriteLine(count2);
    }

    class ElfJob
    {
        public int StartSection { get; init; }
        public int EndSection { get; init; }
    }

    class ElfPair
    {
        public ElfJob Elf1 { get; init; }
        public ElfJob Elf2 { get; init; }
    }

    class Util
    {
        public static bool ContainsTotalOverlap(ElfPair pair)
        {
            if ((pair.Elf1.StartSection >= pair.Elf2.StartSection &&
                pair.Elf1.EndSection <= pair.Elf2.EndSection) ||
                (pair.Elf2.StartSection >= pair.Elf1.StartSection &&
                pair.Elf2.EndSection <= pair.Elf1.EndSection))
            {
                return true;
            }

            return false;
        }

        public static bool ContainsAnyOverlap(ElfPair pair)
        {
            if (!(pair.Elf1.StartSection > pair.Elf2.EndSection ||
                pair.Elf1.EndSection < pair.Elf2.StartSection))
            {
                return true;
            }

            return false;
        }
    }
}
