using MustHaveUtils.Helpers;
using System;
using System.Runtime.CompilerServices;

namespace MustHaveUtils.Extensions
{
    public static class StringExtensions
    {
        public unsafe static void Rotate(this string str, int from)
        {
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
            if (start > str.Length || end < start)
                throw new InvalidOperationException();

            if (string.IsNullOrEmpty(str))
                return false;

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
            return PrevPermutation(str, start, str.Length - 1);
        }

        public static bool PrevPermutation(this string str)
        {
            return PrevPermutation(str, 0, str.Length - 1);
        }

        public unsafe static bool NextPermutation(this string str, int start, int end)
        {
            if (start > str.Length || end < start)
                throw new InvalidOperationException();

            if (string.IsNullOrEmpty(str))
                return false;

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

        public static bool NextPermutation(this string str, int start)
        {
            return NextPermutation(str, start, str.Length - 1);
        }

        public static bool NextPermutation(this string str)
        {
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
