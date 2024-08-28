namespace MonoGameTest;
/// <summary>
/// 描画深度を管理するためのIDです（順番のみ　後ろの方が優先度高）
/// </summary>
public enum DepthId
{
    BackGround,
    Ground,
    HitBox,
    PlayerBack,
    Player,
    PlayerFront,
    Item,
    Attack,
    Effect,
    Status,
    MessageBack,
    Message,

    // Menu
    MenuBack,
    MenuBottom,
    MenuMiddle,
    MenuTop,
    MenuMessageBack,
    MenuMessage,

    Pause,
    Frame,
    Debug
}
