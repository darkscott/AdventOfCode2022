using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public sealed class Puzzle6 : Puzzle
{

    char[] ParseInput()
    {
        const string INPUT_FILE = @".\Day6\Puzzle6Input.txt";
        return File.ReadAllText(INPUT_FILE).ToCharArray();
    }

    public override void Run()
    {
        var input = ParseInput();

        int start = SignalProcessor.FindStartOfPacket(input, 4);

        WriteLine("Start: " + start);

        start = SignalProcessor.FindStartOfPacket(input, 14);

        WriteLine("Start: " + start);
    }

    class SignalProcessor
    {
        public static int FindStartOfPacket(char[] input, int numDistinctChars)
        {
            PacketTracker tracker = new();

            int i = 0;
            for (; i < numDistinctChars; i++)
            {
                tracker.Set(input[i]);
            }

            if (tracker.Count == numDistinctChars) { return i + 1; }

            int s = i - numDistinctChars;
            for (; i < input.Length; i++)
            {
                tracker.Clear(input[s++]);
                tracker.Set(input[i]);
                // Console.WriteLine($"{tracker.Count} | {s + 1} - {i + 1}");
                if (tracker.Count == numDistinctChars) { break; }
            }

            return i + 1;
        }
    }

    class PacketTracker
    {
        public byte [] Tracker { get; private set; } = new byte[26];

        public void Set(char c)
        {
            Tracker[(c - 'a')]++;
        }

        public void Clear(char c)
        {
            Tracker[(c - 'a')]--;
        }

        public int Count => Tracker.Count(x => x != 0);
    }
}
