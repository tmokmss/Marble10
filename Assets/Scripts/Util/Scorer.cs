using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scorer
{
    public float TimeSum { private set; get; }
    public int ReachedLevel { private set; get; }

    const int Mask = 100000;

    public void AddScore(float time, int level)
    {
        TimeSum += time;
        ReachedLevel = level;
    }

    public int Encode()
    {
        // NCMBライブラリに適用するため、レベルと時間を一つのintにパックする
        // レベルはlarger is betterだが時間はsmaller is betterなので、時間だけ引き算している

        if (TimeSum == 0) return 0;
        return ReachedLevel * Mask + (int)(Mask - TimeSum * 100);
    }

    public static Scorer Decode(int number)
    {
        if (number == 0) return new Scorer();

        var timePart = number % Mask;
        var time = (Mask - timePart) / 100f;
        var level = (number - timePart) / Mask;
        var score = new Scorer
        {
            TimeSum = time,
            ReachedLevel = level
        };
        return score;
    }
}
