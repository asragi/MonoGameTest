using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest;

//Key types
public enum Key
{
    Left,
    Up,
    Right,
    Down,
    A,
    B,
    C,
    D,
    Start,
    TotalKeys,
}

internal enum Controller //操縦者
{
    KeyBoard = 0,
    Computer,
    Training,
}

internal static class KeyInput
{
    private const int PlayerCount = 1;
    private static int[,] _input;
    private static Controller[] _controller;
    private static KeyboardState _keyState;
    private static Keys[,] _keyPosition;

    //Current stick input direction, Previous stick input direction
    private static Direction[] _currentDirection, _preDirection;
    private const int TotalDirections = 8;

    //Converter of Directions
    private static readonly int[] ConvertToTenKey = [5, 4, 7, 8, 9, 6, 3, 2, 1];

    private static readonly Key[] ArrowKey = [Key.Left, Key.Up, Key.Right, Key.Down];

    private static readonly Key[][] DirectionToKey =
    [
        [], //0
        [Key.Left], //1
        [Key.Left, Key.Up], //2
        [Key.Up], //3
        [Key.Up, Key.Right], //4
        [Key.Right], //5
        [Key.Right, Key.Down], //6
        [Key.Down], //7
        [Key.Down, Key.Left] //8
    ];

    //Direction when ArrowKey is not down.
    private const Direction DefaultDirection = Direction.Left;


    public static void Init()
    {
        //input is the time button is pushing [keyType, playerCount]
        _input = new int[(int)Key.TotalKeys, PlayerCount];

        _controller = new Controller[PlayerCount];

        //key-config will be preserved in config file.
        //keyPosition will read from this.
        _keyPosition = new[,] // [playerCount, keyType]
        {
            {
                Keys.Left,
                Keys.Up,
                Keys.Right,
                Keys.Down,
                Keys.Z,
                Keys.X,
                Keys.C,
                Keys.V,
                Keys.Enter,
            },
            {
                Keys.OemPeriod,
                Keys.OemPlus,
                Keys.OemBackslash,
                Keys.OemQuestion,
                Keys.B,
                Keys.N,
                Keys.M,
                Keys.OemComma,
                Keys.Escape,
            }
        };

        _currentDirection = new Direction[PlayerCount];
        _preDirection = new Direction[PlayerCount];

        InitGamePad();
    }

    private static bool[] _useGamepad;
    private static List<PlayerIndex> _gamepad;
    private static List<GamePadState> _gamepadState;
    private static Buttons[,] _gamepadButtons;

    private static readonly List<Buttons> AllButtons =
    [
        Buttons.A,
        Buttons.B,
        Buttons.X,
        Buttons.Y,
        Buttons.Start,
        Buttons.DPadLeft,
        Buttons.DPadUp,
        Buttons.DPadRight,
        Buttons.DPadDown,
        Buttons.LeftShoulder,
        Buttons.RightShoulder,
        Buttons.LeftTrigger,
        Buttons.RightTrigger,
        Buttons.Back,
        Buttons.BigButton,
        Buttons.LeftStick,
        Buttons.RightStick,
        Buttons.LeftThumbstickLeft,
        Buttons.LeftThumbstickUp,
        Buttons.LeftThumbstickRight,
        Buttons.LeftThumbstickDown,
        Buttons.RightThumbstickLeft,
        Buttons.RightThumbstickUp,
        Buttons.RightThumbstickRight,
        Buttons.RightThumbstickDown
    ];

    //static GamePadThumbSticks[,] gamepadStick;
    private const float StickIncliningDistance = 0.6f;

    static IEnumerable<PlayerIndex> GetConnectedGamePad()
        => Enum.GetValues(typeof(PlayerIndex)).Cast<PlayerIndex>()
            .Where(p => GamePad.GetCapabilities(p).IsConnected);

    public static void InitGamePad()
    {
        var pads = GetConnectedGamePad().ToList();

        for (int i = 0; i < pads.Count - 2; i++)
            pads.RemoveAt(pads.Count - 1);

        _gamepad = new List<PlayerIndex>();
        if (pads.Count == 0)
        {
            _gamepad.Add(0);
            _gamepad.Add(0);
        }
        else if (pads.Count == 1)
        {
            _gamepad.Add(0);
            _gamepad.Add(pads[0]);
        }
        else
        {
            _gamepad.Add(pads[0]);
            _gamepad.Add(pads[1]);
        }


        _useGamepad = new bool[PlayerCount];
        for (int i = 0; i < pads.Count; i++)
        {
            //Gamepadは2P優先
            _useGamepad[PlayerCount - 1 - i] = true;
        }


        _gamepadState = new List<GamePadState>(2);

        _gamepadButtons = new Buttons[,]
        {
            {
                Buttons.DPadLeft,
                Buttons.DPadUp,
                Buttons.DPadRight,
                Buttons.DPadDown,
                Buttons.A,
                Buttons.B,
                Buttons.X,
                Buttons.Y,
                Buttons.Start,
            },
            {
                Buttons.DPadLeft,
                Buttons.DPadUp,
                Buttons.DPadRight,
                Buttons.DPadDown,
                Buttons.A,
                Buttons.B,
                Buttons.X,
                Buttons.Y,
                Buttons.Start,
            }
        };
    }

    public static void Update()
    {
        _gamepadState = _gamepad.Select(p => GamePad.GetState(p)).ToList();
        _keyState = Keyboard.GetState();

        for (int i = 0; i < PlayerCount; i++)
            switch (_controller[i])
            {
                case Controller.KeyBoard:
                    if (_useGamepad[i])
                    {
                        //When Controller is Gamepad
                        UpdateInputFromGamePad(i);
                    }
                    else
                    {
                        //When Controller is Keyboard
                        UpdateInputFromKeyBoard(i);
                    }

                    break;
                case Controller.Training:
                    break;
            }

        /*if (Game1.frame > 600)
        {
            input[(int)Key.A, 1] = Game1.frame % 120;
            input[(int)Key.B, 1] = Game1.frame % 120 + 1;
        }*/

        for (int i = 0; i < PlayerCount; i++)
        {
            _preDirection[i] = _currentDirection[i];
            _currentDirection[i] = GetStickInput(i + 1);
        }
    }


    static void UpdateInputFromGamePad(int player)
    {
        var inclined = GetInputFromGamePadStick(player);
        for (int i = 0; i < (int)Key.TotalKeys; i++)
            if (_gamepadState[player].IsButtonDown(_gamepadButtons[player, i])
                || (i < ArrowKey.Count() && inclined[i])) //スティックの傾きでも十字入力ができる
                _input[i, player]++;
            else
                _input[i, player] = 0;
    }

    static bool[] GetInputFromGamePadStick(int player)
    {
        var vec = _gamepadState[player].ThumbSticks.Left;
        bool[] inclined = new bool[4];
        if (vec.X < -StickIncliningDistance)
            inclined[(int)Key.Left] = true;
        if (vec.Y > StickIncliningDistance)
            inclined[(int)Key.Up] = true;
        if (vec.X > StickIncliningDistance)
            inclined[(int)Key.Right] = true;
        if (vec.Y < -StickIncliningDistance)
            inclined[(int)Key.Down] = true;
        return inclined;
    }

    static void UpdateInputFromKeyBoard(int player)
    {
        for (int i = 0; i < (int)Key.TotalKeys; i++)
            if (_keyState.IsKeyDown(_keyPosition[player, i]))
                _input[i, player]++;
            else
                _input[i, player] = 0;
    }

    public static void UpdateInput(bool[] down, int player)
    {
        for (int i = 0; i < (int)Key.TotalKeys; i++)
            if (down[i])
                _input[i, player]++;
            else
                _input[i, player] = 0;

        _preDirection[player] = _currentDirection[player];
        _currentDirection[player] = GetStickInput(player + 1);
    }

    public static bool IsKeysDown(Keys keys) => _keyState.IsKeyDown(keys);

    public static int GetKeyState(Key key, int player)
    {
        return _input[(int)key, player - 1];
    }

    //If One of Players Push
    public static bool GetKeyPush(Key key)
    {
        bool flag = false;
        for (int i = 0; i < PlayerCount; i++)
            flag |= _input[(int)key, i] == 1;
        return flag;
    }

    //Is Key Pushed
    public static bool GetKeyPush(Key key, int player)
    {
        return _input[(int)key, player - 1] == 1;
    }

    public static bool GetKeyDown(Key key)
    {
        bool flag = false;
        for (int i = 0; i < PlayerCount; i++)
            flag |= _input[(int)key, i] > 0;
        return flag;
    }

    //Is Key Down
    public static bool GetKeyDown(Key key, int player)
    {
        return _input[(int)key, player - 1] > 0;
    }


    /// <summary>
    /// 何かしらキーが押されていれば、それをひとつだけ返す
    /// </summary>
    public static Nullable<Keys> GetAnyKeyPush()
    {
        var keys = _keyState.GetPressedKeys();
        if (keys.Any()) return keys.First();
        else return null;
    }

    /// <summary>
    /// 押されているボタンをすべて取得する
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static IEnumerable<Buttons> GetAllButtonDown(GamePadState state)
    {
        var buttons = AllButtons.Where(e => state.IsButtonDown(e));
        return buttons;
    }

    /// <summary>
    /// 押されているボタンのうち一つを返す
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static Nullable<Buttons> GetAnyButtonPush(int player)
    {
        var buttons = GetAllButtonDown(_gamepadState[player]);
        if (buttons.Any()) return buttons.First();
        else return null;
    }

    /// <summary>
    /// すべてのゲームパッドのボタン入力状態を取得
    /// 最大接続数4の配列を返す
    /// </summary>
    /// <returns></returns>
    public static List<IEnumerable<Buttons>> GetAllGamePadState()
    {
        var playerIndex = GetConnectedGamePad();
        var list = new List<IEnumerable<Buttons>>();
        for (int i = 0; i < Enum.GetValues(typeof(PlayerIndex)).Length; i++)
        {
            if (!playerIndex.Contains((PlayerIndex)i))
            {
                list.Add(null);
            }
            else
            {
                var state = GamePad.GetState((PlayerIndex)i);
                list.Add(GetAllButtonDown(state));
            }
        }

        return list;
    }


    public static void SetKey(int player, Key key, Keys keys)
    {
        _keyPosition[player, (int)key] = keys;
        _input[(int)key, player]++;
    }

    public static Keys GetKey(int player, Key key) => _keyPosition[player, (int)key];

    public static void SetButton(int player, Key key, Buttons keys)
    {
        _gamepadButtons[player, (int)key] = keys;
        _input[(int)key, player]++;
    }

    public static Buttons GetButton(int player, Key key) => _gamepadButtons[player, (int)key];

    /// <summary>
    /// 1Pと2PのゲームパッドのPlayerIndexを設定する
    /// 長さ2のリストを渡す
    /// </summary>
    /// <param name="p"></param>
    public static void SetGamepad(List<PlayerIndex?> p)
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            if (p[i].HasValue)
            {
                _gamepad[i] = p[i].Value;
                _useGamepad[i] = true;
            }
            else
            {
                _useGamepad[i] = false;
            }
        }
    }

    public static bool IsGamePad(int player) => _useGamepad[player];
    public static PlayerIndex GetGamePadIndex(int player) => _gamepad[player];

    public static string GetKeyName(int player, Key key)
    {
        if (IsGamePad(player))
        {
            return _gamepadButtons[player, (int)key].ToString();
        }
        else
        {
            return _keyPosition[player, (int)key].ToString();
        }
    }


    //Degree Measure Of Stick Direction
    // * If stick is neutral or left, return 0
    public static double GetDegreeDirection(int player)
    {
        return MathEx.DegreeNormalize(
            Math.Atan2((GetKeyDown(Key.Up, player) ? 1 : 0) - (GetKeyDown(Key.Down, player) ? 1 : 0),
                (GetKeyDown(Key.Left, player) ? 1 : 0) - (GetKeyDown(Key.Right, player) ? 1 : 0)) /
            (Math.PI / (MathEx.FullCircle / 2 /*180 degrees*/))
        );
    }

    //360 Degrees To 8 Directions
    static Direction ConvertDegreeToDirection(double degree)
    {
        return (Direction)((int)((degree + MathEx.FullCircle / TotalDirections / 2 /*22.5 degrees*/ + MathEx.Eps)
                                 / (MathEx.FullCircle / TotalDirections) /*45 degrees*/)
            % TotalDirections + 1);
    }

    //If direction is 1 and Key.Left is not down, direction should be 0
    static Direction GetStickInput(int player)
    {
        var direction = ConvertDegreeToDirection(GetDegreeDirection(player));

        var noPush = from e in DirectionToKey[(int)DefaultDirection]
            select GetKeyDown(e, player);

        if (direction == DefaultDirection && !noPush.Aggregate((i, j) => i || j))
            return Direction.N;
        else
            return direction;
    }

    //<Direction, time>
    public static Tuple<Direction, int> GetStickState(int player)
    {
        var direction = GetStickInput(player);
        var isDown = new List<int>();
        foreach (var e in DirectionToKey[(int)direction])
            isDown.Add(GetKeyState(e, player));
        var minTime = direction == 0 ? 0 : isDown.Aggregate((i, j) => j == 0 ? i : (i == 0 ? j : Math.Min(i, j)));

        return Tuple.Create(minTime > 0 ? direction : Direction.N, minTime);

        //
        // 
        // 234
        // 105
        // 876
        //
    }

    public static Direction GetStickInclineDirection(int player)
    {
        return _currentDirection[player - 1];

        //
        // 
        // 234
        // 105
        // 876
        //
    }

    public static Direction GetStickFlickDirection(int player)
    {
        return _currentDirection[player - 1] != _preDirection[player - 1] ? _currentDirection[player - 1] : Direction.N;

        /*var isPushed = new List<bool>();
        foreach (var e in DirectionToKey[(int)direction])
            isPushed.Add(GetKeyPush(e, player));

        //レバーの方向に倒した瞬間であるか
        //斜めに入力されたとき、どちらかのキーが押された瞬間だったらtrueを返してほしいよね

        return isPushed.Aggregate(false, (i, j) => i || j) ? direction : Direction.N;*/

        //
        // 
        // 234
        // 105
        // 876
        //
    }

    public static int ConvertToTenKeyDirection(Direction direction)
    {
        return ConvertToTenKey[(int)direction];

        //TenKey
        // 
        // 789
        // 456
        // 123
        //
    }
}