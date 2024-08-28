using System;

namespace MonoGameTest;

public readonly struct Pivot
{
    public static readonly Pivot TopLeft = new(Vector.Zero);
    public static readonly Pivot TopRight = new(new Vector(1, 0));
    public static readonly Pivot Center = new(new Vector(0.5f, 0.5f));
    public static readonly Pivot BottomRight = new(Vector.One);
    private readonly Vector _pivot;

    private Pivot(Vector pivot)
    {
        if (pivot.X < 0 || pivot.X > 1 || pivot.Y < 0 || pivot.Y > 1)
            throw new ArgumentException("Pivot must be between 0 and 1");
        _pivot = pivot;
    }
    
    public Vector Apply(Size size) => new(size.X * _pivot.X, size.Y * _pivot.Y);
}