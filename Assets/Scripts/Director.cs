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
    [SerializeField] AudioManager audioManager;
    [SerializeField] ProgressManager progressManager;
    [SerializeField] RankingManager rankingManager;

    readonly float MaxTime = 10f;
    readonly int InitialLevel = 4;

    GameState state;
    Scorer score;
    Examiner examiner;
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
                break;
            case GameState.JustStarted:
                InitializeGame();
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
            case GameState.LevelCleared:
                LevelUp().WrapErrors();
                break;
            case GameState.LevelUp:
                // LevelUp!の表示などを行っている
                break;
            case GameState.GameOver:
                GameOver().WrapErrors();
                state = GameState.Result;
                break;
            case GameState.Result:
                break;
        }
    }

    public void GoToHome()
    {
        state = GameState.Waiting;
    }

    public void StartPlaying()
    {
        state = GameState.JustStarted;
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

    void InitializeGame()
    {
        score = new Scorer();
        examiner = new Examiner(InitialLevel);
        spawner.Reset();
        StartLevel();
    }

    async Task LevelUp()
    {
        state = GameState.LevelUp;

        // 到達したレベルを記録として用いる (not クリアしたレベル)
        // You reached Level x in t seconds! 的な
        score.AddScore(MaxTime - timeLeft, examiner.CurrentLevel + 1);

        audioManager.PlayLevelUp();
        await textManager.ShowLevelUp(0.8f);

        examiner.GoToNextLevel();
        StartLevel();
    }

    void StartLevel()
    {
        textManager.SetLevel(examiner.CurrentLevel);
        var next = examiner.NextQuestion();

        progressManager.ResetProgress();
        timeLeft = MaxTime;
        spawner.Spawn(next).WrapErrors();
        state = GameState.InTenSeconds;
    }

    async Task GameOver()
    {
        Debug.Log($"{score.TimeSum} sec, {score.ReachedLevel} reached");
        await rankingManager.SendScore("aaa", score);
        await rankingManager.ShowRanking();
    }

    public async Task Input(Direction direction)
    {
        if (state == GameState.Waiting)
        {
            StartPlaying();
            return;
        }
        if (state != GameState.InTenSeconds) return;

        var relation = directionMap[direction];
        var result = examiner.Answer(relation);
        var nextMarbleNum = examiner.NextQuestion();
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
            state = GameState.LevelCleared;
        }

        return next;
    }
}

enum GameState
{
    Waiting,
    JustStarted,
    InTenSeconds,
    LevelCleared,
    GameOver,
    Result,
    Ranking,
    LevelUp,
}