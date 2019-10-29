using System;

namespace MustHaveUtils.Extensions
{
    public static class StringExtensions
    {
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

                        SwapIter(ref i, ref i2);
                        ReverseIter(i1, last);

                        return true;
                    }
                    if (i == first)
                    {
                        ReverseIter(first, last);

                        return false;
                    }
                }
            }
        }

        public unsafe static bool NextPermutation(this string str, int start)
        {
            return NextPermutation(str, start, str.Length - 1);
        }

        public unsafe static bool NextPermutation(this string str)
        {
            return NextPermutation(str, 0, str.Length - 1);
        }

        private static unsafe void SwapIter(ref char* first, ref char* second)
        {
            char buffer = *first;
            *first = *second;
            *second = buffer;
        }

        private static unsafe void ReverseIter(char* first, char* last)
        {
            long idx = (last - first + 1) / 2;

            while (idx != 0)
            {
                SwapIter(ref first, ref last);
                first++;
                last--;
                idx--;
            }
        }
    }
}
