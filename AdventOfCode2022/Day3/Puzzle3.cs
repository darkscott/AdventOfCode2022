using System.Collections;

namespace AdventOfCode2022;

public sealed class Puzzle3 : Puzzle
{
    List<Rucksack> ParseInput()
    {
        const string INPUT_FILE = @".\Day3\Puzzle3Input.txt";
        List<Rucksack> rucksacks = new();

        foreach (var line in File.ReadAllLines(INPUT_FILE))
        {
            Rucksack r = new Rucksack(line.ToCharArray());
            rucksacks.Add(r);
        }

        return rucksacks;
    }

    public override void Run()
    {
        var rucksacks = ParseInput();

        Part1(rucksacks);

        //Part2(rucksacks);

        int test = rucksacks.Select(s => s.UniqueContents)
                 .Chunk(3)
                 .Select(c => Util.PriorityFromChar(Util.GetCharFromBitArray(c.Aggregate((g, e) =>
                 {
                     return g.And(e);
                 })))).Sum();

        WriteLine(test);
    }

    void Part1(List<Rucksack> rucksacks)
    {
        // 7691
        int sum = rucksacks.Sum(r => GetPriority(r));

        WriteLine(sum);

        int GetPriority(Rucksack r)
        {
            return Util.PriorityFromChar(r.FindItemInBothCompartments());
        }
    }

    void Part2(List<Rucksack> rucksacks)
    {
        int sum = 0;
        
        for (int i = 0; i < rucksacks.Count; ) 
        {
            var elf1 = rucksacks[i++].UniqueContents;
            var elf2 = rucksacks[i++].UniqueContents;
            var elf3 = rucksacks[i++].UniqueContents;

            var common = elf1.And(elf2).And(elf3);

            char item = Util.GetCharFromBitArray(common);
            sum += Util.PriorityFromChar(item);
        }

        WriteLine(sum);
    }
    
    class Rucksack
    {
        public char[] Contents { get; init; }
        public BitArray UniqueContents { get; init; }

        public Rucksack(char[] contents)
        {
            Contents = contents;
            UniqueContents = GetUniqueContents(contents);
        }

        public char FindItemInBothCompartments()
        {
            BitArray comp1 = GetUniqueContents(new Span<char>(Contents, 0, Contents.Length / 2));
            BitArray comp2 = GetUniqueContents(new Span<char>(Contents, Contents.Length / 2 , Contents.Length / 2));

            var result = comp1.And(comp2);

            return Util.GetCharFromBitArray(result);
        }

        private BitArray GetUniqueContents(Span<char> contents)
        {
            BitArray result = new(128);

            for (int i = 0; i < contents.Length; i++)
            {
                result.Set(contents[i], true);
            }

            return result;
        }
    }

    class Util
    {
        public static int PriorityFromChar(char c)
        {
            return c switch
            {
                (>= 'a') and (<= 'z') => (c - 'a') + 1,
                (>= 'A') and (<='Z') => (c - 'A') + 27,
                _ => throw new InvalidDataException()
            };
        }

        public static char GetCharFromBitArray(BitArray ba)
        {
            int charIndex = 0;
            for (; charIndex < ba.Length; charIndex++)
            {
                if (ba[charIndex]) break;
            }

            return (char)charIndex;
        }
    }
}
