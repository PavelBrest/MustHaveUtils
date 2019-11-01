using MustHaveUtils.Helpers;
using System;
using System.Runtime.CompilerServices;

namespace MustHaveUtils.Extensions.Array
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Transforms the range into the previous permutation from the set of all permutations that are lexicographically ordered with respect to IComparable. 
        /// Returns true if such permutation exists, otherwise transforms the range into the last permutation and returns false.
        /// </summary>
        /// <typeparam name="T">T must meet implement IComparable<T></typeparam>
        /// <returns>
        /// true if the new permutation precedes the old in lexicographical order. 
        /// false if the first permutation was reached and the range was reset to the last permutation.
        /// </returns>
        public static bool PrevPermutation<T>(this T[] array)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            return PrevPermutation(array, 0, array.Length - 1);
        }

        /// <summary>
        /// Transforms the range into the previous permutation from the set of all permutations that are lexicographically ordered with respect to IComparable. 
        /// Returns true if such permutation exists, otherwise transforms the range into the last permutation and returns false.
        /// </summary>
        /// <typeparam name="T">T must meet implement IComparable<T></typeparam>
        /// <returns>
        /// true if the new permutation precedes the old in lexicographical order. 
        /// false if the first permutation was reached and the range was reset to the last permutation.
        /// </returns>
        public static bool PrevPermutation<T>(this T[] array, int start)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            return PrevPermutation(array, start, array.Length - 1);
        }

        /// <summary>
        /// Transforms the range into the previous permutation from the set of all permutations that are lexicographically ordered with respect to IComparable. 
        /// Returns true if such permutation exists, otherwise transforms the range into the last permutation and returns false.
        /// </summary>
        /// <typeparam name="T">T must meet implement IComparable<T></typeparam>
        /// <returns>
        /// true if the new permutation precedes the old in lexicographical order. 
        /// false if the first permutation was reached and the range was reset to the last permutation.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static bool PrevPermutation<T>(this T[] array, int start, int end)
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

            while (true)
            {
                int i1, i2;

                i1 = i;
                if (span[i1].CompareTo(span[--i]) < 0)
                {
                    i2 = last + 1;
                    while (!(span[--i2].CompareTo(span[i]) < 0))
                        ;

                    SpanHelper.Swap(ref span, ref i, ref i2);
                    MemoryExtensions.Reverse(span.Slice(i1, last - i1 + 1));

                    return true;
                }
                if (i == first)
                {
                    MemoryExtensions.Reverse(span.Slice(first, last - first + 1));

                    return false;
                }
            }
        }

        /// <summary>
        /// Transforms the range into the next permutation from the set of all permutations that are lexicographically ordered with respect to operator IComparable. 
        /// Returns true if such permutation exists, otherwise transforms the range into the first permutation and returns false.
        /// </summary>
        /// <typeparam name="T">T must meet implement IComparable<T></typeparam>
        /// <returns>
        /// true if the new permutation is lexicographically greater than the old. 
        /// false if the last permutation was reached and the range was reset to the first permutation.
        /// </returns>
        public static bool NextPermutation<T>(this T[] array)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            return NextPermutation(array, 0, array.Length - 1);
        }

        /// <summary>
        /// Transforms the range into the next permutation from the set of all permutations that are lexicographically ordered with respect to operator IComparable. 
        /// Returns true if such permutation exists, otherwise transforms the range into the first permutation and returns false.
        /// </summary>
        /// <typeparam name="T">T must meet implement IComparable<T></typeparam>
        /// <returns>
        /// true if the new permutation is lexicographically greater than the old. 
        /// false if the last permutation was reached and the range was reset to the first permutation.
        /// </returns>
        public static bool NextPermutation<T>(this T[] array, int start)
            where T : IComparable<T>
        {
            if (array == null) throw new NullReferenceException();

            return NextPermutation(array, start, array.Length - 1);
        }

        /// <summary>
        /// Transforms the range into the next permutation from the set of all permutations that are lexicographically ordered with respect to operator IComparable. 
        /// Returns true if such permutation exists, otherwise transforms the range into the first permutation and returns false.
        /// </summary>
        /// <typeparam name="T">T must meet implement IComparable<T></typeparam>
        /// <returns>
        /// true if the new permutation is lexicographically greater than the old. 
        /// false if the last permutation was reached and the range was reset to the first permutation.
        /// </returns>
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
                    MemoryExtensions.Reverse(span.Slice(first, last - first + 1));

                    return false;
                }
            }
        }
    }
}
