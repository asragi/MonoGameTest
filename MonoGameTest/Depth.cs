using System;

namespace MonoGameTest;

/// <summary>
/// 描画深度を管理します
/// 同じ描画深度IDでも、描画順によって微妙に値を変えてあとに呼ばれた描画の方を上にすることで、
/// 描画順序を一定化して画面がちらつかないように＋自然に書けるように
/// </summary>
public class Depth
{
    /// <summary>
    /// 同描画深度許容数
    /// </summary>
    //（0x3effffff / sameDepth）> DepthIDの数になるように設定してください
    private const int SameDepth = 0x100000;

    /// <summary>
    /// 利用済みdepthの回数の一覧
    /// </summary>
    private static int[] _depthUsed;

    /// <summary>
    /// DepthIDの長さ
    /// </summary>
    private static int DepthNum => Enum.GetNames(typeof(DepthId)).Length;

    public float Value { get; private set; }

    private Depth(float v) => Value = v;

    /// <summary>
    /// IDをDepthとして利用するための物
    /// </summary>
    public static implicit operator Depth(DepthId id) =>
        new(ConvertInt((int)id * SameDepth + _depthUsed[(int)id]++));

    /// <summary>
    /// Depth利用数のリセット
    /// 描画前に呼ぶ
    /// </summary>
    public static void DepthReset()
    {
        _depthUsed = new int[DepthNum];
    }

    /// <summary>
    /// int（0 ~ 1056964607）から順序を逆にしたfloat(0 ~ 1)へ変換
    /// </summary>
    static float ConvertInt(int d)
    {
        int c = 0x3effffff - d;
        float f = c % 0x800000;
        f += 0x800000;
        int e = c / 0x800000 - 125;
        for (int i = 0; i < -e + 24; i++)
            f /= 2;
        return f;
    }
}

