using System;

namespace MonoGameTest;

public class SelectionWindow
{
    private readonly SelectionWindowPresenter _presenter;
    private readonly SelectionWindowView _view;

    public SelectionWindow(
        TextureId windowTexture,
        int cornerSize,
        TextureId cursorTexture,
        Vector relativePosition,
        Size size,
        Pivot pivot,
        DepthId depth,
        Vector cursorPosition,
        int cursorLineHeight,
        string[] options,
        Listen<int> onSubmit,
        Listen onCancel
        )
    {
        _view = new SelectionWindowView(
            windowTexture,
            cornerSize,
            cursorTexture,
            relativePosition,
            size,
            pivot,
            depth,
            cursorPosition,
            cursorLineHeight
        );
        
        _presenter = new SelectionWindowPresenter(
            options.Length,
            _view.OnUpdateIndex,
            onSubmit,
            onCancel
        );
    }

    public void OnInputUp()
    {
        _presenter.OnInputUp();
    }
    
    public void OnInputDown()
    {
        _presenter.OnInputDown();
    }

    public void OnSubmit()
    {
        _presenter.OnSubmit();
    }
    
    public void OnCancel()
    {
        _presenter.OnCancel();
    }

    public void Open()
    {
        _view.Open();
    }
    
    public void Close()
    {
        _view.Close();
    }

    public void Draw(Drawing d, Vector parentPosition)
    {
        _view.Draw(d, parentPosition);
    }
}