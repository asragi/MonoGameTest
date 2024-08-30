namespace MonoGameTest;

internal class SelectionWindowView
{
    private readonly Window _window;
    private readonly Image _cursor;
    private readonly Vector _cursorPosition;
    private readonly int _cursorLineHeight;
    private bool _isOpen;

    internal SelectionWindowView(
        TextureId windowTexture,
        int cornerSize,
        TextureId cursorTexture,
        Vector relativePosition,
        Size size,
        Pivot pivot,
        DepthId depth,
        Vector cursorPosition,
        int cursorLineHeight
    )
    {
        _window = new Window(windowTexture, cornerSize, relativePosition, size, pivot, depth);
        _cursor = new Image(cursorPosition + relativePosition, Resource.GetTexture(cursorTexture), depth, Pivot.TopLeft, null);
        _cursorLineHeight = cursorLineHeight;
        _cursorPosition = cursorPosition;
        _isOpen = false;
    }

    public void OnUpdateIndex(int index)
        => _cursor.SetY(_window.GetRelativePosition().Y + _cursorPosition.Y + _cursorLineHeight * index);

    public void Open() => _isOpen = true;

    public void Close() => _isOpen = false;

    public void Draw(Drawing d, Vector parentPosition)
    {
        if (!_isOpen) return;
        _window.Draw(d, parentPosition);
        _cursor.Draw(d, parentPosition);
    }
}