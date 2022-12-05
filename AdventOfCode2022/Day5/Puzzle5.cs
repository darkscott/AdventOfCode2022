using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public sealed class Puzzle5 : Puzzle
{

    (Crane9000, Crane9001, List<MoveCargoOp>) ParseInput()
    {
        Regex rx = new Regex(@"move (\d+) from (\d) to (\d)");
        const string INPUT_FILE = @".\Day5\Puzzle5Input.txt";

        string[] input = File.ReadAllLines(INPUT_FILE);

        Crane9000 crane9000 = new();
        Crane9001 crane9001 = new();
        List<MoveCargoOp> moves = new();

        // Parse crate diagram, first 8 lines
        // + 1 for column number
        // + 1 blank line before moves
        // Crate letters starts at index 1 (second position) and every 4 characters
        for (int i = 7; i >= 0; i--)
        {
            string line = input[i].PadRight(40); // pad right so we can ensure indices are accessible

            for (int pos = 0; pos < 9; pos++)
            {
                char c = line[(pos * 4) + 1];
                if (char.IsLetter(c))
                {
                    crane9000.Add(pos, c);
                    crane9001.Add(pos, c);
                }
            }
        }

        // Move data starts on line 11 (index 10)
        // Parse moves
        for (int i = 10; i < input.Length; i++)
        {
            var match = rx.Match(input[i]);
            if (!match.Success) { throw new InvalidOperationException(); }

            int num = int.Parse(match.Groups[1].Value);
            int from = int.Parse(match.Groups[2].Value);
            int to = int.Parse(match.Groups[3].Value);

            MoveCargoOp mco = new MoveCargoOp() { NumberOfCrates = num, FromPos = from, ToPos = to };
            moves.Add(mco);
        }

        return (crane9000, crane9001, moves);
    }

    public override void Run()
    {
        (var crane9000, var crane9001, var moves) = ParseInput();

        crane9000.ApplyMoves(moves); // VJSFHWGFT

        WriteLine(crane9000.GetTopCrates());

        crane9001.ApplyMoves(moves); //LCTQFBVZV

        WriteLine(crane9001.GetTopCrates());
    }

    class MoveCargoOp
    {
        public int NumberOfCrates { get; init; }
        public int FromPos { get; init; }
        public int ToPos { get; init; }
    }

    abstract class CraneBase
    {
        public List<List<char>> Cargo { get; init;} = new();

        public CraneBase()
        {
            for (int i = 0; i < 9; i++)
            {
                Cargo.Add(new List<char>());
            }
        }

        public void Add(int pos, char c)
        {
            Cargo[pos].Add(c);
        }

        public void Move(int x, int y)
        {
            char c = Cargo[x - 1].Pop();
            Cargo[y - 1].Add(c);
        }


        public string GetTopCrates()
        {
            return string.Join("", Cargo.Select(c => c.Last()));
        }
        abstract public void ApplyMoves(List<MoveCargoOp> moves);
    }

    class Crane9000 : CraneBase
    {
        public override void ApplyMoves(List<MoveCargoOp> moves)
        {
            foreach (var move in moves)
            {
                for (int i = 0; i < move.NumberOfCrates; i++)
                {
                    Move(move.FromPos, move.ToPos);
                }
            }
        }
    }

    class Crane9001 : CraneBase
    {
        public override void ApplyMoves(List<MoveCargoOp> moves)
        {
            foreach (var move in moves)
            {
                var crates = Cargo[move.FromPos - 1].PopTimes(move.NumberOfCrates);
                Cargo[move.ToPos - 1].AddRange(crates);
            }
        }
    }
}

public static class ListUtil
{
    public static T Pop<T>(this List<T> list)
    {
        T item = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return item;
    }

    public static IEnumerable<T> PopTimes<T>(this List<T> list, int times)
    {
        var items = list.TakeLast(times).ToArray();
        list.RemoveRange(list.Count - times, times);
        return items;
    }
}
