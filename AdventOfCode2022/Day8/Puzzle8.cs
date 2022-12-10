//#define TEST_DATA
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public sealed class Puzzle8 : Puzzle
{
    byte[][] _map;
    int _width;
    int _height;

    char[][] _dbgMap;

    byte[][] _marked;

    int[][] _score;

    void ParseInput()
    {
#if TEST_DATA
        string[] data = new string[]
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };
#else
        const string INPUT_FILE = @".\Day8\Puzzle8Input.txt";
        string[] data = File.ReadAllLines(INPUT_FILE);
#endif


        _height = data.Length;
        _width = data[0].Length;

        _map = Util.Create2DArray<byte>(_width, _height, (r, c) => (byte)(data[r][c] - '0'));
        _marked = Util.Create2DArray<byte>(_width, _height, (_, _) => 0);
        _dbgMap = Util.Create2DArray<char>(_width, _height, (_, _) => 'o');
        _score = Util.Create2DArray<int>(_width, _height, GetScore);
    }

    public override void Run()
    {
        ParseInput();

        int count = CountEdges();
        for (int c = 1; c < _width - 1; c++)
        {
            count += CountVertical(c);
        }

        for (int r = 1; r < _height - 1; r++)
        {
            count += CountHorizontal(r);
        }

        // Part 1
        WriteLine(count);

        WriteLine("\n\n");

        PrintDbgMap();

        // Part 2
        int max = _score.Select(s => s.Max()).Max();

        WriteLine(max);
    }

    void MarkVisible(int row, int column, char dir)
    {
        _dbgMap[row][column] = dir;
        _marked[row][column] = 1;
        WriteLine($"{dir} {row,3}, {column}");
    }

    int CountHorizontal(int row)
    {
        int count = 0;
        int max = _map[row][0];
        for (int col = 1; col < _width - 1; col++)
        {
            if (IsVisible(max, row, col))
            {
                MarkVisible(row, col, '»');
                count++;
            }
            max = Math.Max(_map[row][col], max);
            if (max == 9) { break; }
        }

        max = _map[row][_width - 1];
        for (int col = _width - 2; col >= 1; col--)
        {
            if (IsVisible(max, row, col))
            {
                MarkVisible(row, col, '«');
                count++;
            }
            max = Math.Max(_map[row][col], max);
            if (max == 9) { break; }
        }

        return count;
    }

    int CountVertical(int col)
    {
        int count = 0;
        int max = _map[0][col];
        for (int row = 1; row < _height - 2; row++)
        {
            if (IsVisible(max, row, col))
            {
                MarkVisible(row, col, '↑');
                count++;
            }
            max = Math.Max(_map[row][col], max);
            if (max == 9) { break; }
        }

        max = _map[_height - 1][col];
        for (int row = _height - 2; row > 2; row--)
        {
            if (IsVisible(max, row, col))
            {
                MarkVisible(row, col, '↓');
                count++;
            }
            max = Math.Max(_map[row][col], max);
            if (max == 9) { break; }
        }

        return count;
    }

    int CountEdges()
    {
        return (2 * _width) + (2 * (_height - 2));
    }

    void PrintDbgMap()
    {
        Console.Write("   ");
        for (int i = 0; i <= _width; i += 5)
        {
            Console.Write($"{i + 1,-5}");
        }
        Console.WriteLine();

        for (int r = 0; r < _height; r++)
        {
            for (int c = 0; c < _width; c++)
            {
                Verify(r, c);
                if (c == 0)
                {
                    Console.Write($"{r + 1,2} ");
                }
                Console.Write($"{_dbgMap[r][c]}");
            }
            Console.WriteLine();
        }
    }

    bool IsVisible(int max, int row, int col)
    {
        return (max < _map[row][col] && _marked[row][col] == 0);
    }

    bool Verify(int row, int col)
    {
        int max = _map[row][col];
        switch (_dbgMap[row][col])
        {
            case '↑':
                for (int r = row - 1; r >= 0; r--)
                {
                    Debug.Assert(_map[r][col] < max);
                }
                break;
            case '↓':
                for (int r = row + 1; r < _height; r++)
                {
                    Debug.Assert(_map[r][col] < max);
                }
                break;
            case '»':
                for (int c = col - 1; c >= 0; c--)
                {
                    Debug.Assert(_map[row][c] < max);
                }
                break;
            case '«':
                for (int c = col + 1; c < _width; c++)
                {
                    Debug.Assert(_map[row][c] < max);
                }
                break;
        }
        return true;
    }

    int GetScore(int row, int col)
    {
        int height = _map[row][col];
        int up = 0;
        int down = 0;
        int left = 0;
        int right = 0;

        for (int r = row - 1; r >= 0; r--)
        {
            up++;
            if (_map[r][col] >= height) { break; }
        }

        for (int r = row + 1; r < _height; r++)
        {
            down++;
            if (_map[r][col] >= height) { break; }
        }

        for (int c = col - 1; c >= 0; c--)
        {
            left++;
            if (_map[row][c] >= height) { break; }
        }

        for (int c = col + 1; c < _width; c++)
        {
            right++;
            if (_map[row][c] >= height) { break; }
        }

        return up * down * left * right;
    }
}
