using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    int currentNumber;
    Dictionary<Direction, Relation> directionMap;

    // Use this for initialization
    void Start()
    {
        directionMap = new Dictionary<Direction, Relation>
        {
            [Direction.Up] = Relation.Equal,
            [Direction.Left] = Relation.Smaller,
            [Direction.Right] = Relation.Larger
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartPlaying()
    {
        currentNumber = 3;
    }

    public void Input(Direction direction)
    {
        var task = spawner.AnswerAndSpawn(direction, directionMap[direction]);
    }

    public Result Answer(Relation answer)
    {
        return Result.Wrong;
    }
}
