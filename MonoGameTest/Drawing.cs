using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest;

/// <summary>
/// 描画を任せるクラス
/// </summary>
public class Drawing(SpriteBatch batch, float drawRate)
{
    /// <summary>
    /// 描画に必要なもの
    /// </summary>
    private readonly SpriteBatch _sb = batch;

    /// <summary>
    /// 描画倍率　全体にかかる
    /// </summary>
    private readonly float _drawRate = drawRate;

    public void Draw(Vector pos, Texture2D tex, Depth depth, DrawOption option)
    {
        _sb.Draw(
            tex, pos * _drawRate, option.SourceRectangle, option.Color, option.Angle, option.Origin,
            option.Scale * _drawRate, option.Flip,
            depth.Value);
    }

    /// <summary>
    /// 文字を描画
    /// </summary>
    public void DrawText(Vector2 pos, SpriteFont font, string text, Depth depth, DrawOption? drawOption = null)
    {
        var option = drawOption ?? DrawOption.Default;
        _sb.DrawString(font, text, pos * _drawRate, option.Color, option.Angle, option.Origin, option.Scale * _drawRate,
            option.Flip, depth.Value);
    }

    public void DrawBegin()
    {
        Depth.DepthReset();
         _sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
    }

    public void DrawEnd()
    {
        _sb.End();
    }
}

public readonly struct DrawOption
{
    public DrawOption()
    {
    }

    public static DrawOption Default { get; } = new();

    public Vector Origin { get; init; } = Vector.Zero;
    public Rectangle? SourceRectangle { get; init; } = null;
    public Color Color { get; init; } = Color.White;
    public float Angle { get; init; } = 0;
    public SpriteEffects Flip { get; init; } = SpriteEffects.None;
    public Vector Scale { get; init; } = Vector.One;
}