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
    private Image _image;
    private Window _window, _secondWindow;

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
        _image = new Image(new(100, 100), Resource.GetTexture(TextureId.Window), DepthId.Debug, Pivot.TopRight, null);
        _window = new Window(TextureId.Window, 5, new(8, 8), new Size(new Vector(120, 80)), Pivot.TopLeft, DepthId.Debug);
        _secondWindow = new Window(TextureId.Window, 5, new(Width - 8, Height - 8), new Size(new Vector(200, 80)), Pivot.BottomRight, DepthId.Debug);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.AliceBlue);
        _drawing.DrawBegin();
        _image.Draw(_drawing, Vector.Zero);
        _window.Draw(_drawing, Vector.Zero);
        _secondWindow.Draw(_drawing, Vector.Zero);
        _drawing.DrawEnd();
        base.Draw(gameTime);
    }
}