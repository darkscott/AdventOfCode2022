namespace AdventOfCode2022;
public sealed class Puzzle1 : Puzzle
{
    List<Elf> elves = new List<Elf>();

    public override void Run()
    {
        ParseInput();

        var mostCalsElf = elves.Max(e => e.TotalCalories);

        WriteLine(mostCalsElf);

        var topThreeCalsElvesCals = elves.OrderByDescending(e => e.TotalCalories).Take(3).Sum(e => e.TotalCalories);

        WriteLine(topThreeCalsElvesCals);
    }

    void ParseInput()
    {
        const string INPUT_FILE = @".\Day1\Puzzle1Input.txt";

        var fileLines = File.ReadAllLines(INPUT_FILE);
        Elf elf = GetNextElf();

        foreach (var line in fileLines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                if (int.TryParse(line, out int value))
                {
                    elf.CalorieItems.Add(value);
                }
            }
            else
            {
                elf = GetNextElf();
            }
        }

        Elf GetNextElf()
        {
            Elf elf = new();
            elves.Add(elf);
            return elf;
        }
    }

    class Elf
    {
        public List<int> CalorieItems { get; init; } = new List<int>();
        public int TotalCalories => CalorieItems.Sum(x => x);

        public int ElfId { get; init; }

        private static int ElfIdCounter = 1;

        public Elf()
        {
            ElfId = ElfIdCounter;
            ElfIdCounter++;
        }
    }
}
