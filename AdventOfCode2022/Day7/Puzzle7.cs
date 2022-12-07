using System.Collections;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public sealed class Puzzle7 : Puzzle
{
    string[] ParseInput()
    {
        const string INPUT_FILE = @".\Day7\Puzzle7Input.txt";

        return File.ReadAllLines(INPUT_FILE);
    }

    public override void Run()
    {
        ElFS elfs = new ElFS();
        elfs.Build(ParseInput());

        Part1();

        Part2();

        void Part1()
        {
            var dirs = elfs.DfsEnumerate().Where(n => n.IsDir && n.RecursiveSize() <= 100000);

            WriteLine("Part 1:");
            WriteLine(dirs.Sum(n => n.RecursiveSize()));
        }

        void Part2()
        {
            int totalSize = elfs.Size;
            int availableSpace = 70000000 - totalSize;

            var dirSizeToDelete = elfs.DfsEnumerate().OrderBy(s => s.RecursiveSize()).ToList();

            WriteLine("Part 2:");
            WriteLine(dirSizeToDelete.First(s => availableSpace + s.RecursiveSize() >= 30000000).ToString());
        }
    }

    class ElFS
    {
        ElFSNode root = new ElFSNode("FS");

        public int Size => root.RecursiveSize();

        public void Build(string[] input)
        {
            Stack<ElFSNode> elFStk = new Stack<ElFSNode>();
            ElFSNode current = root;


            foreach (var line in input)
            {
                var split = line.Split(' ');

                switch (split[0])
                {
                    case "$":
                        ParseCommand(split);
                        break;
                    default:
                        ParseNode(split);
                        break;
                }
            }

            void ParseNode(string[] line)
            {
                int size = line[0] == "dir" ? 0 : int.Parse(line[0]);
                string name = line[1];

                current.Children.Add(name, new ElFSNode(name, size));
            }

            void ParseCommand(string[] command)
            {
                string cmd = command[1];
                string target = command.Length > 2 ? command[2] : "";

                switch (cmd)
                {
                    case "cd":
                        if (target == "..")
                        {
                            elFStk.Pop();
                            current = elFStk.Peek();
                        }
                        else
                        {
                            if (!current.Children.TryGetValue(target, out var child))
                            {
                                child = new ElFSNode(target);
                                current.Children.Add(target, child);
                            }
                            current = child;
                            elFStk.Push(current);
                        }
                        break;
                    case "ls":
                        break;
                }
            }
        }

        public StringBuilder Print()
        {
            StringBuilder sb = new();

            PrintRecursive(root, 0);

            return sb;


            void PrintRecursive(ElFSNode node, int indent)
            {
                sb.Append("".PadRight(indent, ' '));
                sb.AppendLine(node.ToString());

                foreach (var child in node.Children.Values)
                {
                    PrintRecursive(child, indent + 2);
                }
            }
        }

        public IEnumerable<ElFSNode> DfsEnumerate()
        {
            return RecursiveIterate(root.Children.First().Value);

            IEnumerable<ElFSNode> RecursiveIterate(ElFSNode node)
            {
                foreach ((string name, ElFSNode n) in node.Children)
                {
                    if (n.IsDir)
                    {
                        //yield return n;
                        foreach (var node2 in RecursiveIterate(n))
                        {
                            yield return node2;
                        }
                    }
                    else
                    {
                        
                    }

                    yield return n;
                }
            }
        }
    }

    class ElFSNode
    {
        public string Name { get; init; }
        public int Size { get; set; } = 0;

        public bool IsDir => Size == 0;
        public bool IsFile => Size > 0;

        public Dictionary<string, ElFSNode> Children { get; set; } = new();

        public ElFSNode(string name, int size = 0)
        {
            Name = name;
            Size = size;
        }

        public int RecursiveSize()
        {
            return Size + Children.Values.Sum(x => x.RecursiveSize());
        }

        public override string ToString()
        {
            return $"{Name} - {Size} - {RecursiveSize()}";
        }
    }
}
