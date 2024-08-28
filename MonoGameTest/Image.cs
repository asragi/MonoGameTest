using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest;

public class Image(Vector relativePosition, Texture2D texture, DepthId depth, Pivot pivot, DrawOption? option)
{
    private readonly DrawOption _option = option ?? DrawOption.Default;
    private readonly Size _size = new(new Vector(texture.Width, texture.Height));

    public void Draw(Drawing d, Vector parentPosition)
    {
        var pivotPosition = pivot.Apply(_size);
        d.Draw(parentPosition + relativePosition - pivotPosition, texture, depth, _option);
    }
}
