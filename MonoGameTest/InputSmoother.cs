namespace MonoGameTest;

internal class InputSmoother
{
    private const int InputWait = 6;
    private const int HeadWait = 20;
    private int _waitFrame;
    private int _headWaitCount;
    private bool _inputCheck;

    /// <summary>
    /// 入力押しっぱなしでも毎フレーム移動しないようにする関数
    /// </summary>
    public Direction SmoothInput(Direction dir)
    {
        _waitFrame--;
        if (dir == Direction.N)
        {
            _inputCheck = false;
            _waitFrame = 0;
            _headWaitCount = HeadWait;
            return Direction.N;
        }

        _headWaitCount--;
        if (_headWaitCount > 0 && _inputCheck) return Direction.N;
        if (_waitFrame > 0) return Direction.N;
        _waitFrame = InputWait;
        _inputCheck = true;
        return dir;
    }
}