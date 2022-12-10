using static BenchmarkDotNet.Engines.Engine;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public sealed class Puzzle10 : Puzzle
{
    List<Instruction> ParseInput()
    {
        //var input = File.ReadAllLines(@".\Day10\PuzzleTestInput.txt");
        var input = File.ReadAllLines(@".\Day10\PuzzleInput.txt");

        List<Instruction> result = new();

        foreach (var line in input)
        {
            var split = line.Split(' ');
            if (split.Length == 1)
            {
                result.Add(Instruction.CreateNoop());
            }
            else
            {
                result.Add(Instruction.CreateAddx(int.Parse(split[1])));
            }
        }

        return result;
    }

    public override void Run()
    {
        Part2();
    }

    void Part2()
    {
        int cycleCount = 0;
        int regX = 1;
        int crtX = 0;
        var instructions = ParseInput();

        foreach (var inst in instructions)
        {
            for (int i = 0; i < inst.Clocks; i++)
            {
                cycleCount++;
                if (regX - 1 <= crtX && regX + 1 >= crtX)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write('.');
                }
                crtX++;
                if (crtX % 40 == 0) { crtX = 0; Console.WriteLine(); }
            }
            regX += inst.Value;
        }
    }

    void Part1()
    {
        HashSet<int> SyncCycles = new() { 20, 60, 100, 140, 180, 220 };
        int cycleCount = 0;
        int regX = 1;

        int signalSum = 0;

        var instructions = ParseInput();

        foreach (var inst in instructions)
        {
            for (int i = 0; i < inst.Clocks; i++)
            {
                cycleCount++;
                if (SyncCycles.Contains(cycleCount))
                {
                    signalSum += cycleCount * regX;
                }
            }
            regX += inst.Value;
        }

        WriteLine(signalSum);
    }

    class Instruction
    {
        public int Value { get; set; } = 0;

        public int Clocks { get; set; } = 1;

        public static Instruction CreateAddx(int value) { return new() { Value = value, Clocks = 2 }; }

        public static Instruction CreateNoop() { return new(); }
    }
}
