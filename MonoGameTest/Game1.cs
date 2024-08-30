using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest;

public class Game1 : Game
{
    private const int Width = 384;
    private const int Height = 288;
    private const int DrawRate = 2;
    private SpriteBatch _spriteBatch;
    private Drawing _drawing;
    private SelectionWindow _selectionWindow;

    public Game1()
    {
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = Width * DrawRate;
        graphics.PreferredBackBufferHeight = Height * DrawRate;
        graphics.IsFullScreen = false;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _drawing = new Drawing(_spriteBatch, DrawRate);
        Resource.Init(Content);
        KeyInput.Init();
        _selectionWindow = new SelectionWindow(
            TextureId.Window,
            6,
            TextureId.Cursor,
            new(30, 30),
            new Size(new Vector(100, 100)),
            Pivot.TopLeft,
            DepthId.Debug,
            new Vector(8, 8),
            16,
            ["Option1", "Option2", "Option3"],
            index => { },
            () => { });
        _selectionWindow.Open();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        KeyInput.Update();
        
        if (KeyInput.GetKeyPush(Key.Up))
            _selectionWindow.OnInputUp();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.AliceBlue);
        _drawing.DrawBegin();
        _selectionWindow.Draw(_drawing, Vector.Zero);
        _drawing.DrawEnd();
        base.Draw(gameTime);
    }
}