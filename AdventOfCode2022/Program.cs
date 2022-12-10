using AdventOfCode2022;

using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;


#if DEBUG
IPuzzle puzzle = new Puzzle10();
puzzle.Run();
#else
BenchmarkRunner.Run<PuzzleRunner>();
#endif


[SimpleJob()]
public class PuzzleRunner
{
    //[Benchmark]
    public void RunPuzzle1()
    {
        new Puzzle1().Run();
    }

    [Benchmark]
    public void RunPuzzle2()
    {
        new Puzzle2().Run();
    }

    [Benchmark]
    public void RunPuzzle3()
    {
        new Puzzle3().Run();
    }

    [Benchmark]
    public void RunPuzzle4()
    {
        new Puzzle4().Run();
    }
}