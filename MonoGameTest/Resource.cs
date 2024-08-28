using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest;

public static class Resource
{
    private static readonly Texture2D[] Textures;
    private static readonly string[] TexturePath;

    private const string UserInterfacePath = "UI/";

    static Resource()
    {
        Textures = new Texture2D[(int)TextureId.Size];
        TexturePath = new string[(int)TextureId.Size];
        SetPath(TextureId.Test, "test");
        SetPath(TextureId.Window, "window");
    }

    public static void Init(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        for (int i = 0; i < (int)TextureId.Size; i++)
            Textures[i] = content.Load<Texture2D>(TexturePath[i]);
    }


    private static void SetPath(TextureId id, string path) => TexturePath[(int)id] = path;
    public static Texture2D GetTexture(TextureId id) => Textures[(int)id];
}