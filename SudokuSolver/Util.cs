using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class Util
    {
        public static Boolean AllDifferent<T>(IEnumerable<T> array)
        {
            return array.Distinct().Count() == array.Count();
        }

        public static int[] IndexToCoordinates(int index, int size)
        {
            return new int[] {index / size, index % size};
        }

        public static HashSet<T> Subtract<T>(HashSet<T> set, IEnumerable<T> other)
        {
            HashSet<T> clone = new HashSet<T>(set);
            clone.ExceptWith(other);
            return clone;
        }

        public static IEnumerable<T> SliceRow<T>(T[,] array, int row)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                yield return array[row, i];
            }
        }

        public static IEnumerable<T> SliceColumn<T>(T[,] array, int column)
        {
            for (int i = 0; i < array.GetLength(1); i++)
            {
                yield return array[i, column];
            }
        }

        public static IEnumerable<T> SliceRegion<T>(T[,] array, int fromX, int toX, int fromY, int toY)
        {
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    yield return array[x, y];
                }
            }
        }
    }
}
