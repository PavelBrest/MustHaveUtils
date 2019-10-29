using System;
using System.Collections.Generic;
using System.Text;

namespace MustHaveUtils.Extensions
{
    public static class StringExtensions
    {
        public unsafe static bool NextPermutation(this string str)
        {
            var length = str.Length;

            fixed (char* ptr = str)
            {
                char* first = &ptr[0];
                char* last = &ptr[length - 1];

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
                        ReverseIter(ref i1, ref last);

                        return true;
                    }
                    if (i == first)
                    {
                        ReverseIter(ref first, ref last);

                        return false;
                    }
                }
            }
        }

        private static unsafe void SwapIter(ref char* first, ref char* second)
        {
            char buffer = *first;
            *first = *second;
            *second = buffer;
        }

        private static unsafe void ReverseIter(ref char* first, ref char* last)
        {
            long idx = (last - first + 1) / 2;

            while (idx != 0)
            {
                SwapIter(ref first, ref last);
                first--;
                last--;
                idx--;
            }
        }
    }
}
