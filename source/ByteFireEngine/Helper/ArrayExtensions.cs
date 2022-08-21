using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteFireEngine.Helper
{
    static class ArrayExtensions
    {

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i]);
            }
        }

        public static void ForEachIndexed<T>(this T[] array, Action<T, int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (int i = 0; i < array.Length; i++)
            {
                action(array[i], i);
            }
        }

    }
}
