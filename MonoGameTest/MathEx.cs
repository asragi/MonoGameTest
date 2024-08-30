namespace MonoGameTest;

public static class MathEx
{
    public const float Eps = 1.0e-10f;
    public const double FullCircle = 360;

    public static double DegreeNormalize(double degree)
    {
        //degree -> 0~360

        while (degree >= FullCircle) degree -= FullCircle;
        while (degree < 0) degree += FullCircle;
        return degree;
    }
}