using System.Text;

namespace AdventOfCode2022;

public interface IPuzzle
{
    void Run();
}

public abstract class Puzzle : IPuzzle
{
    public Puzzle() { }

    public abstract void Run();

#if DEBUG
    protected void WriteLine(string text) => Console.WriteLine(text);
    protected void WriteLine(object text) => Console.WriteLine(text.ToString());
#else
    protected void WriteLine(string text) {}
    protected void WriteLine(object text) {}
#endif
}
