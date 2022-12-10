using System.Text;

namespace AdventOfCode2022;

public sealed class Puzzle9 : Puzzle
{
    List<Move> ParseInput()
    {
        List<Move> result = new();
        //var lines = File.ReadAllLines(@"Day9\PuzzleTestInput.txt");
        var lines = File.ReadAllLines(@"Day9\PuzzleInput.txt");

        foreach (var line in lines)
        {
            Move m = new Move(line[0], int.Parse(line[2..]));
            result.Add(m);
        }

        return result;
    }

    public override void Run()
    {
        Console.Clear();
        PositionTracker tracker = new();
        var moves = ParseInput();

        RopeEnd head = RopeEnd.CreateHead();

        RopeEnd firstTail = null;
        RopeEnd prev = new();
        for (int i = 0; i < 9; i++)
        {
            RopeEnd curTail = RopeEnd.CreateTail();
            if (firstTail == null)
            {
                firstTail = curTail;
            }
            if (prev != null)
            {
                prev.Next = curTail;
            }
            // curTail.OnMoved = (r) => WriteLine($"Tail {r.Id} moved to: {r.X,3}, {r.Y,-3}");

            tracker.Add(curTail);
            prev = curTail;
        }

        RopeEnd tail = prev;
        tail.OnMoved = (r) =>
        {
            tracker.Add(r);
            WriteLine($"Tail {r.Id} moved to: {r.X,3}, {r.Y,-3}");
        };
        tracker.Add(tail);

        foreach (var move in moves)
        {
            // WriteLine($"{move.Dir} {move.Count}");
            for (int i = 0; i < move.Count; i++)
            {
                head.ProcessMove(move);
                //WriteLine($"Head moved to: {head.X,3}, {head.Y,-3}");
                firstTail.Update(head);
            }
        }

        WriteLine(tracker.Positions.Count);
        WriteLine(tracker.TotalMoves);
    }

    class Move
    {
        public char Dir { get; init; }
        public int Count { get; init; }

        public Move(char dir, int count)
        {
            Dir = dir;
            Count = count;
        }
    }

    class RopeEnd
    {
        static int TailCounter = 0;

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public char Id { get; init; }

        public RopeEnd? Next { get; set; } = null;

        public static RopeEnd CreateHead()
        {
            return new RopeEnd() { Id = 'H', X = 11, Y = 15 };
        }

        public static RopeEnd CreateTail()
        {
            return new RopeEnd() { Id = (char)('1' + TailCounter++), X = 11, Y = 15 };
        }

        public void ProcessMove(Move m)
        {
            switch (m.Dir)
            {
                case 'U':
                    Y += 1;
                    break;
                case 'D':
                    Y -= 1;
                    break;
                case 'L':
                    X -= 1;
                    break;
                case 'R':
                    X += 1;
                    break;
            }
        }

        public void MoveToward(RopeEnd r)
        {
            while (!IsAdjacent(r))
            {
                if (r.X < X)
                {
                    X -= 1;
                }
                else if (r.X > X)
                {
                    X += 1;
                }

                if (r.Y < Y)
                {
                    Y -= 1;
                }
                else if (r.Y > Y)
                {
                    Y += 1;
                }

                if (X < 0 || Y < 0)
                {
                    int i = 3;
                }
                OnMoved(this);
            }
        }

        public bool IsAdjacent(RopeEnd r)
        {
            int x = Math.Abs(r.X - this.X);
            int y = Math.Abs(r.Y - this.Y);

            return (x <= 1) && (y <= 1);
        }

        public void Update(RopeEnd prev)
        {
            MoveToward(prev);
            Next?.Update(this);
        }

        public Action<RopeEnd> OnMoved = (e) => { };
    }

    class PositionTracker
    {
        public HashSet<(int, int)> Positions { get; } = new();

        public int TotalMoves { get; private set; }

        public int MinX { get; private set; }
        public int MinY { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }


        public void Add(int x, int y)
        {
            MinX = Math.Min(MinX, x);
            MinY = Math.Min(MinY, y);
            MaxX = Math.Max(MaxX, x);
            MaxY = Math.Max(MaxY, y);
            TotalMoves++;

            Positions.Add((x, y));
        }

        public void Add(RopeEnd r) => Add(r.X, r.Y);
    }
}