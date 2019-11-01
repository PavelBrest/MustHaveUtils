using MustHaveUtils.Helpers;
using System;
using System.Runtime.CompilerServices;

namespace MustHaveUtils.Extensions.Array
{
    public static class ArrayExtensions
    {
        public static bool NextPermutation<T>(this T[] array)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            return NextPermutation(array, 0, array.Length - 1);
        }

        public static bool NextPermutation<T>(this T[] array, int start)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            return NextPermutation(array, start, array.Length - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static bool NextPermutation<T>(this T[] array, int start, int end)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            if (start > array.Length || 
                end > array.Length || 
                end < start) 
                throw new InvalidOperationException();

            var span = array.AsSpan();

            var first = start;
            var last = end;

            if (first == last) return false;

            var i = last;

            while(true)
            {
                int i1, i2;

                i1 = i;
                if (span[--i].CompareTo(span[i1]) < 0)
                {
                    i2 = last + 1;
                    while (!(span[i].CompareTo(span[--i2]) < 0))
                        ;

                    SpanHelper.Swap(ref span, ref i, ref i2);
                    MemoryExtensions.Reverse(span.Slice(i1, last - i1 + 1));

                    return true;
                }
                if (i == first)
                {
                    MemoryExtensions.Reverse(span.Slice(first, last - first));

                    return false;
                }
            }
        }
    }
}
