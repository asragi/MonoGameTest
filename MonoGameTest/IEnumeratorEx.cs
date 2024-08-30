using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameTest;

public static class EnumerableEx
{
    /// <summary>
    /// 最小値を持つ要素を返します
    /// </summary>
    public static TSource FindMin<TSource, TResult>
    (
        this IEnumerable<TSource> self,
        Func<TSource, TResult> selector
    )
    {
        return self.First(c => selector(c).Equals(self.Min(selector)));
    }

    /// <summary>
    /// 最大値を持つ要素を返します
    /// </summary>
    public static TSource FindMax<TSource, TResult>
    (
        this IEnumerable<TSource> self,
        Func<TSource, TResult> selector
    )
    {
        return self.First(c => selector(c).Equals(self.Max(selector)));
    }

    /// <summary>
    /// ForEachをIEnumeratorに拡張
    /// </summary>
    public static void ForEach2<T>
    (
        this IEnumerable<T> self,
        Action<T> func
    )
    {
        foreach (var e in self) func(e);
    }

    public static TAccumulate Aggregate2<TSource, TAccumulate>
    (
        this IEnumerable<TSource> self,
        TAccumulate seed,
        Func<TAccumulate, TSource, int, TAccumulate> func
    )
    {
        return self.Select((p, i) => (p, i)).Aggregate(seed, (s, e) =>
        {
            var (p, i) = e;
            return func(s, p, i);
        });
    }
}