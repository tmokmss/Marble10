using UnityEngine;

public static class GaussianRandom
{
    public static float Next(float mean = 0f, float deviation = 1f)
    {
        var u1 = 1 - Random.value;
        var u2 = 1 - Random.value;
        var randStdNormal = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
        var randNormal = mean + deviation * randStdNormal;
        return randNormal;
    }

    public static int NextInt(float mean = 0f, float deviation = 1f)
    {
        return 0;
    }
}
