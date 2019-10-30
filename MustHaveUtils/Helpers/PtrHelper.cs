using System.Runtime.CompilerServices;

namespace MustHaveUtils.Helpers
{
    internal unsafe static class PtrHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapIter(ref char* first, ref char* second)
        {
            char buffer = *first;
            *first = *second;
            *second = buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseIter(char* first, char* last)
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
