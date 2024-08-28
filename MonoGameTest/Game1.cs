using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest;

public class Game1 : Game
{
    private SpriteBatch _spriteBatch;
    private Drawing _drawing;
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
        Resource.Init(Content);
        _image = new Image(Vector.Zero, Resource.GetTexture(TextureId.Window), DepthId.Debug, Pivot.BottomRight, null);
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
        _image.Draw(_drawing, new Vector(320, 180));
        _drawing.DrawEnd();
        base.Draw(gameTime);
    }
}