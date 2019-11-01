using System;
using System.Runtime.CompilerServices;

namespace MustHaveUtils.Helpers
{
    internal static class SpanHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref Span<T> span, ref int first, ref int second)
        {
            var buffer = span[first];
            span[first] = span[second];
            span[second] = buffer;
        }
    }
}
