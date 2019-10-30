using MustHaveUtils.Helpers;
using System;
using System.Runtime.CompilerServices;

namespace MustHaveUtils.Extensions.String
{
    public static class StringExtensions
    {
        /// <summary>
        /// Performs a left rotation on a range of elements
        /// </summary>
        /// <param name="str">String for rotate.</param>
        /// <param name="from">The element's index that should appear at the beginning of the rotated range</param>
        public unsafe static void Rotate(this string str, int from)
        {
            if (string.IsNullOrEmpty(str))
                throw new NullReferenceException();

            fixed (char* ptr = str)
            {
                char* first = &ptr[0];
                char* f = &ptr[from] + 1;
                char* last = &ptr[str.Length - 1] + 1;
                Rotate(ref first, ref f, ref last);
            }
        }


        public unsafe static bool PrevPermutation(this string str, int start, int end)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            if (start > str.Length || end < start)
                throw new InvalidOperationException();

            fixed (char* ptr = str)
            {
                char* first = &ptr[start];
                char* last = &ptr[end];

                if (first == last) return false;

                char* i = last;

                while (true)
                {
                    char* i1, i2;

                    i1 = i;
                    if (*i1 < *--i)
                    {
                        i2 = last + 1;
                        while (!(*--i2 < *i))
                            ;
                        PtrHelper.SwapIter(ref i, ref i2);
                        PtrHelper.ReverseIter(i1, last);
                        return true;
                    }
                    if (i == first)
                    {
                        PtrHelper.ReverseIter(first, last);
                        return false;
                    }
                }
            }
        }

        public static bool PrevPermutation(this string str, int start)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return PrevPermutation(str, start, str.Length - 1);
        }

        public static bool PrevPermutation(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return PrevPermutation(str, 0, str.Length - 1);
        }

        /// <summary>
        /// Transforms the string into the next permutation from the set of all permutations that are lexicographically ordered with respect to operator or comp. 
        /// Returns true if such permutation exists, otherwise transforms the range into the first permutation and returns false.
        /// </summary>
        /// <param name="str">String to permutate</param>
        /// <param name="start">Start element's index to permute</param>
        /// <param name="end">Last element's index to permute</param>
        /// <returns>
        /// true if the new permutation is lexicographically greater than the old. 
        /// false if the last permutation was reached and the range was reset to the first permutation.
        /// </returns>
        public unsafe static bool NextPermutation(this string str, int start, int end)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            if (start > str.Length || end < start)
                throw new InvalidOperationException();

            fixed (char* ptr = str)
            {
                char* first = &ptr[start];
                char* last = &ptr[end];

                if (first == last) return false;

                char* i = last;

                while (true)
                {
                    char* i1, i2;

                    i1 = i;
                    if (*--i < *i1)
                    {
                        i2 = last + 1;
                        while (!(*i < *--i2))
                            ;

                        PtrHelper.SwapIter(ref i, ref i2);
                        PtrHelper.ReverseIter(i1, last);

                        return true;
                    }
                    if (i == first)
                    {
                        PtrHelper.ReverseIter(first, last);

                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Transforms the string into the next permutation from the set of all permutations that are lexicographically ordered with respect to operator or comp. 
        /// Returns true if such permutation exists, otherwise transforms the range into the first permutation and returns false.
        /// </summary>
        /// <param name="str">String to permutate</param>
        /// <param name="start">Start element's index to permute</param>
        /// <returns>
        /// true if the new permutation is lexicographically greater than the old. 
        /// false if the last permutation was reached and the range was reset to the first permutation.
        /// </returns>
        public static bool NextPermutation(this string str, int start)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return NextPermutation(str, start, str.Length - 1);
        }


        /// <summary>
        /// Transforms the string into the next permutation from the set of all permutations that are lexicographically ordered with respect to operator or comp. 
        /// Returns true if such permutation exists, otherwise transforms the range into the first permutation and returns false.
        /// </summary>
        /// <param name="str">String to permutate</param>
        /// <returns>
        /// true if the new permutation is lexicographically greater than the old. 
        /// false if the last permutation was reached and the range was reset to the first permutation.
        /// </returns>
        public static bool NextPermutation(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return NextPermutation(str, 0, str.Length - 1);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static char* Rotate(ref char* first, ref char* from, ref char* last)
        {
            if (first == from) return last;
            if (from == last) return first;

            char* read = from;
            char* write = first;
            char* nextRead = first;

            while (read != last)
            {
                if (write == nextRead) nextRead = read;

                PtrHelper.SwapIter(ref write, ref read);
                ++write;
                ++read;
            }

            Rotate(ref write, ref nextRead, ref last);
            return write;
        }
    }
}
