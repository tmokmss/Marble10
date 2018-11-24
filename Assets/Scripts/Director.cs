using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] float offset;
    [SerializeField, Range(30, 200)] int bpm;

    [SerializeField] Spawner spawner;
    [SerializeField] TextManager textManager;
    [SerializeField] Examiner examiner;

    GameState state;
    int currentLevel;
    int currentCombo;
    Dictionary<Direction, Relation> directionMap;
    float timeLeft;

    // Use this for initialization
    void Start()
    {
        Beat.Bpm = bpm;
        Beat.Offset = offset;

        directionMap = new Dictionary<Direction, Relation>
        {
            [Direction.Up] = Relation.Equal,
            [Direction.Left] = Relation.Smaller,
            [Direction.Right] = Relation.Larger
        };
        state = GameState.Waiting;
    }

    // Update is called once per frame
    void Update()
    {
        Beat.Bpm = bpm;
        Beat.Offset = offset;

        switch (state)
        {
            case GameState.Waiting:
                state = GameState.JustStarted;
                break;
            case GameState.JustStarted:
                StartPlaying();
                state = GameState.InTenSeconds;
                break;
            case GameState.InTenSeconds:
                var elapsed = UpdateTime();
                if (elapsed)
                {
                    state = GameState.StageFinished;
                }

                break;
            case GameState.StageFinished:
                LevelUp();
                state = GameState.InTenSeconds;
                break;
            case GameState.StageCleared:
                break;
            case GameState.GameOver:
                break;
            case GameState.Result:
                break;
        }
    }

    bool UpdateTime()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
        }
        textManager.SetTime(timeLeft);
        return timeLeft <= 0;
    }

    void StartPlaying()
    {
        currentLevel = 2;
        LevelUp();
        timeLeft = 10f;
    }

    void LevelUp()
    {
        currentLevel++;
        textManager.SetLevel(currentLevel);
        examiner.CurrentLevel = currentLevel;
        timeLeft = 10f;
    }

    public void Input(Direction direction)
    {
        var relation = directionMap[direction];
        var result = examiner.Answer(relation);
        var next = examiner.NextQuestion(currentLevel);
        var task = spawner.AnswerAndSpawn(direction, relation, next);
        ProcessResult(result);
    }

    void ProcessResult(Result result)
    {
        if (result == Result.Correct)
        {
            currentCombo++;
        } else
        {
            currentCombo = 0;
        }
        textManager.SetCombo(currentCombo);
    }
}

enum GameState
{
    Waiting,
    JustStarted,
    InTenSeconds,
    StageFinished,
    StageCleared,
    GameOver,
    Result,
}