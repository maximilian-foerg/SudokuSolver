using System.Collections.Generic;
using System.Linq;

namespace SudokuLibrary
{
    public class Util
    {
        public static bool AllDifferent<T>(IEnumerable<T> array)
        {
            return array.Distinct().Count() == array.Count();
        }

        public static Field IndexToField(int index, int size)
        {
            return new(index / size, index % size);
        }

        public static HashSet<T> Subtract<T>(HashSet<T> set, IEnumerable<T> other)
        {
            HashSet<T> clone = new(set);
            clone.ExceptWith(other);
            return clone;
        }
    }
}