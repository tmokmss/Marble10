using UnityEngine;

public static class Beat
{
    public static int Bpm { set; get; }
    public static float Offset { set; get; }
    public static float UnitTime => Bpm / 60f / 8;

    public static float Sine(float center, float amplitude)
    {
        if (Bpm == 0) return 0;

        var frequency = Bpm / 60f;
        var period = 1 / frequency;
        var omega = 2 * Mathf.PI * frequency;
        return Mathf.Sin(omega * (Time.time + period * Offset)) * amplitude + center;
    }
}
