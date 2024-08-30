using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest;

public class Window
{
    private readonly Image[] _cornerImages, _sideImages;
    private readonly Image _centerImage;

    public Window(TextureId textureId, int cornerSize, Vector relativePosition, Size size, Pivot pivot, DepthId depth)
    {
        var texture = Resource.GetTexture(textureId);
        var pivotPos = pivot.Apply(size);
        _cornerImages = CreateCorner(cornerSize, relativePosition, size, pivotPos, depth, texture);
        _sideImages = CreateSide(cornerSize, relativePosition, size, depth, texture, pivotPos);
        _centerImage = new Image(relativePosition + new Vector(cornerSize, cornerSize) - pivotPos, texture, depth,
            Pivot.TopLeft, new DrawOption()
            {
                SourceRectangle = new Rectangle(
                    cornerSize,
                    cornerSize,
                    texture.Width - cornerSize * 2,
                    texture.Height - cornerSize * 2
                ),
                Scale = new Vector(
                    (size.X - cornerSize * 2) / (texture.Width - cornerSize * 2),
                    (size.Y - cornerSize * 2) / (texture.Height - cornerSize * 2)
                ),
            });
    }

    private static Image[] CreateSide(int cornerSize, Vector relativePosition, Size size, DepthId depth,
        Texture2D texture,
        Vector pivotPos)
    {
        var images = new Image[4];
        var sourceRectangles = new Rectangle[]
        {
            new(cornerSize, 0, texture.Width - cornerSize * 2, cornerSize),
            new(texture.Width - cornerSize, cornerSize, cornerSize, texture.Height - cornerSize * 2),
            new(cornerSize, texture.Height - cornerSize, texture.Width - cornerSize * 2, cornerSize),
            new(0, cornerSize, cornerSize, texture.Height - cornerSize * 2),
        };
        var positions = new Vector[]
        {
            new(cornerSize, 0),
            new(size.X - cornerSize, cornerSize),
            new(cornerSize, size.Y - cornerSize),
            new(0, cornerSize),
        };
        var sideWidthRate = (size.X - cornerSize * 2) / (texture.Width - cornerSize * 2);
        var sideHeightRate = (size.Y - cornerSize * 2) / (texture.Height - cornerSize * 2);
        for (var i = 0; i < 4; i++)
        {
            var option = new DrawOption()
            {
                SourceRectangle = sourceRectangles[i],
                Scale = i % 2 == 0 ? new Vector(sideWidthRate, 1) : new Vector(1, sideHeightRate),
            };
            images[i] = new Image(relativePosition + positions[i] - pivotPos, texture, depth, Pivot.TopLeft, option);
        }

        return images;
    }

    private static Image[] CreateCorner(int cornerSize, Vector relativePosition, Size size, Vector pivotPos,
        DepthId depth,
        Texture2D texture)
    {
        var images = new Image[4];
        for (var i = 0; i < 4; i++)
        {
            var option = new DrawOption()
            {
                SourceRectangle = new Rectangle(
                    i % 2 == 0 ? 0 : texture.Width - cornerSize,
                    i < 2 ? 0 : texture.Height - cornerSize,
                    cornerSize,
                    cornerSize
                ),
            };
            var pos = new Vector(
                i % 2 == 0 ? 0 : size.X - cornerSize,
                i < 2 ? 0 : size.Y - cornerSize
            );
            images[i] = new Image(relativePosition + pos - pivotPos, texture, depth, Pivot.TopLeft, option);
        }

        return images;
    }

    public Vector GetRelativePosition() => _centerImage.GetRelativePosition();

    public void Draw(Drawing d, Vector parentPosition)
    {
        foreach (var image in _cornerImages)
            image.Draw(d, parentPosition);
        foreach (var image in _sideImages)
            image.Draw(d, parentPosition);
        _centerImage.Draw(d, parentPosition);
    }
}