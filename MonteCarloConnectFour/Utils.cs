using MonteCarloConnectFour;
using System;
using System.Collections.Generic;
using System.Linq;

static class GlobalRandom
{
    private static Random randomInstance = null;

    public static int NextInt(int maxValue)
    {
        if (randomInstance == null)
            randomInstance = new Random();

        return randomInstance.Next(maxValue);
    }

    public static double NextDouble()
    {
        if (randomInstance == null)
            randomInstance = new Random();

        return randomInstance.NextDouble();
    }
}

static class EnumerableExtensions
{
    public static T MaxObject<T, U>(this IEnumerable<T> source, Func<T, U> selector)
        where U : IComparable<U>
    {
        if (source == null) throw new ArgumentNullException("source");
        bool first = true;
        T maxObj = default(T);
        U maxKey = default(U);
        foreach (var item in source)
        {
            if (first)
            {
                maxObj = item;
                maxKey = selector(maxObj);
                first = false;
            }
            else
            {
                U currentKey = selector(item);
                if (currentKey.CompareTo(maxKey) > 0)
                {
                    maxKey = currentKey;
                    maxObj = item;
                }
            }
        }
        return maxObj;
    }
}
