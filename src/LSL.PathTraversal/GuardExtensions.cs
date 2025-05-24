using System;

namespace LSL.PathTraversal;

internal static class GuardExtensions
{
    public static T GuardAgainstNull<T>(this T source, string parameterName)
    {
        if (source == null) throw new ArgumentNullException(parameterName);

        return source;
    }
}
