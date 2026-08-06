using System;
using System.Collections.Generic;

public static class ListExtentions
{
    private static Random rng = new();

    public static void Shuffle<T>(this List<T> list)
    {
        var i = list.Count;
        while(i > 1)
        {
            i--;
            var j = rng.Next(i + 1);
            T value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }
}
