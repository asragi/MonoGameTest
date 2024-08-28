using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest;

public class Game1 : Game
{
    private SpriteBatch _spriteBatch;
    private Drawing _drawing;
    private Texture2D _texture;
    private Image _image;

    public Game1()
    {
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = 640;
        graphics.PreferredBackBufferHeight = 360;
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
        _drawing = new Drawing(_spriteBatch, 2);
        _texture = Content.Load<Texture2D>("test");
        _image = new Image(Vector.Zero, _texture, DepthId.Debug, Pivot.BottomRight, null);
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
        GraphicsDevice.Clear(Color.Red);
        _drawing.DrawBegin();
        _image.Draw(_drawing, new Vector(320, 180));
        _drawing.Draw(new Vector(100, 0), _texture, DepthId.Debug, new() {Color = new(255, 255, 255, 50)});
        _drawing.Draw(new Vector(100, 50), _texture, DepthId.Debug, new() {Color = new(255, 255, 255, 50)});
        _drawing.DrawEnd();
        base.Draw(gameTime);
    }
}