using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] float offset;
    [SerializeField, Range(30, 200)] int bpm;

    [SerializeField] Spawner spawner;
    [SerializeField] TextManager textManager;
    [SerializeField] Examiner examiner;
    [SerializeField] AudioManager audioManager;
    [SerializeField] ProgressManager progressManager;

    readonly float MaxTime = 10f;
    readonly int InitialLevel = 3;

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
                audioManager.StartBGM();
                state = GameState.InTenSeconds;
                break;
            case GameState.InTenSeconds:
                var elapsed = UpdateTime();
                if (elapsed)
                {
                    // 規定回数正解することなく10秒経過した
                    state = GameState.GameOver;
                }

                break;
            case GameState.StageFinished:
                var task = LevelUp();
                break;
            case GameState.DuringLevelUp:

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
        currentLevel = InitialLevel;
        StartLevel();
    }

    async Task LevelUp()
    {
        state = GameState.DuringLevelUp;
        await textManager.ShowLevelUp(1.3f);

        currentLevel++;
        StartLevel();
        state = GameState.InTenSeconds;
    }

    void StartLevel()
    {
        textManager.SetLevel(currentLevel);
        var next = examiner.NextQuestion(currentLevel);

        progressManager.ResetProgress();
        timeLeft = MaxTime;
        state = GameState.InTenSeconds;
        var task = spawner.Spawn(next);
    }

    public async Task Input(Direction direction)
    {
        var relation = directionMap[direction];
        var result = examiner.Answer(relation);
        var nextMarbleNum = examiner.NextQuestion(currentLevel);
        var goNextLevel = ProcessResult(result);

        await spawner.Answer(direction, relation);
        if (!goNextLevel)
            await spawner.Spawn(nextMarbleNum);
    }

    bool ProcessResult(Result result)
    {
        if (result == Result.Correct)
        {
            currentCombo++;
        } else
        {
            currentCombo = 0;
        }
        audioManager.PlayAnswer(result);
        var next = progressManager.SetProgress(result);
        if (next)
        {
            state = GameState.StageFinished;
        }

        return next;
    }
}

enum GameState
{
    Waiting,
    JustStarted,
    InTenSeconds,
    StageFinished,
    StageCleared,
    DuringLevelUp,
    GameOver,
    Result,
}