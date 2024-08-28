namespace MonoGameTest;

public readonly struct Size(Vector size)
{
    public double X => size.X;
    public double Y => size.Y;
}