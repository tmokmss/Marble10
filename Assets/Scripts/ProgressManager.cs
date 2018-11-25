using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] List<MeshRenderer> bars;
    int maxCount;
    int currentProgress;

    // Use this for initialization
    void Start()
    {
        maxCount = bars.Count;
        SetProgress(Result.Wrong);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns>規定の成功回数に達したならば真</returns>
    public bool SetProgress(Result result)
    {
        currentProgress += result == Result.Correct ? 1 : -currentProgress;
        if (currentProgress < 0) currentProgress = 0;
        bars.Select((bar, i) => new { bar, i }).ToList().ForEach(p => ChangeBarColor(p.bar, GetBarColor(p.i < currentProgress)));
        return currentProgress >= maxCount;
    }

    public void ResetProgress()
    {
        currentProgress = 0;
        bars.ForEach(bar => ChangeBarColor(bar, GetBarColor(false)));
    }

    void ChangeBarColor(Renderer bar, Color color)
    {
        bar.material.SetColor("_Color", color);
    }

    Color GetBarColor(bool isReached)
    {
        return isReached ? Color.green : Color.red;
    }
}
