using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;
class Util
{
    public static T[][] Create2DArray<T>(int width, int height, Func<int, int, T> initialValue) where T : new()
    {
        T[][] arr = new T[height][];
        for (int i = 0; i < height; i++)
        {
            arr[i] = new T[width];
            for (int j = 0; j < width; j++)
            {
                arr[i][j] = initialValue(i, j);
            }
        }
        return arr;
    }
}
