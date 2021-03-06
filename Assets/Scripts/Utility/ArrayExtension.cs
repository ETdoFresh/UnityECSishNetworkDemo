﻿using System.Collections.Generic;
using System.Linq;

public static class ArrayExtension
{
    public static string ToStringArray(this string[] strings, string seperator = ", ")
    {
        var output = "";
        for (int i = 0; i < strings.Length; i++)
            output += (i == 0 ? "" : seperator) + strings[i];
        return output;
    }

    public static int IndexOf<T>(this IEnumerable<T> haystack, IEnumerable<T> needle)
    {
        if (needle.Count() == 0) return 0;

        var lastI = haystack.Count() - needle.Count() + 1;
        for (int i = 0; i < lastI; i++)
        {
            if (haystack.ElementAt(i).Equals(needle.ElementAt(0)))
            {
                if (needle.Count() == 1) return i;

                var allEquals = true;
                for (int j = 1; j < needle.Count() && allEquals; j++)
                    if (!haystack.ElementAt(i + j).Equals(needle.ElementAt(j)))
                        allEquals = false;

                if (allEquals) return i;
            }
        }
        return -1;
    }
}
