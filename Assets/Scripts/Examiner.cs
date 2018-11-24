using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examiner : MonoBehaviour
{
    public int CurrentLevel { set; get; }

    int comboCount;
    int currentMarbleCount;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int NextQuestion(int nextLevel)
    {
        CurrentLevel = nextLevel;
        var diff = GaussianRandom.Next(0, 1);
        currentMarbleCount = Mathf.RoundToInt(CurrentLevel + diff);
        if (currentMarbleCount <= 0) currentMarbleCount = 1;
        return currentMarbleCount;
    }

    public Result Answer(Relation answer)
    {
        return GetCurrentRelation() == answer ? Result.Correct : Result.Wrong;
    }

    Relation GetCurrentRelation()
    {
        if (currentMarbleCount < CurrentLevel)
            return Relation.Smaller;
        if (currentMarbleCount > CurrentLevel)
            return Relation.Larger;
        return Relation.Equal;
    }
}
