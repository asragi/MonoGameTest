namespace MonoGameTest;

internal class SelectionWindowPresenter(
    int maxIndex,
    Listen<int> indexListen,
    Listen<int> submitListen,
    Listen cancelListen)
{
    private readonly Listen _cancelListen = cancelListen;
    private int _index;

    public void OnInputUp()
    {
        _index = (_index - 1 + maxIndex) % maxIndex;
        NotifyChangeIndex();
    }
    
    public void OnInputDown()
    {
        _index = (_index + 1) % maxIndex;
        NotifyChangeIndex();
    }

    public void OnSubmit()
    {
        NotifySubmit();
    }

    public void OnCancel()
    {
        NotifyCancel();
    }
    
    private void NotifyChangeIndex()
    {
        indexListen(_index);
    }
    
    private void NotifySubmit()
    {
        submitListen(_index);
    }
    
    private void NotifyCancel()
    {
        indexListen(_index);
    }
}
