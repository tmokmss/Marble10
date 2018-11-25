using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] float offset;
    [SerializeField, Range(30, 200)] int bpm;

    [SerializeField] TutorialDirector tutorialDirector;
    [SerializeField] Spawner spawner;
    [SerializeField] TextManager textManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] ProgressManager progressManager;
    [SerializeField] RankingManager rankingManager;
    [SerializeField] SubmitManager submitManager;

    readonly float MaxTime = 10f;
    readonly int InitialLevel = 4;

    GameState state;
    Examiner examiner;
    Scorer score;
    Dictionary<Direction, Relation> directionMap;
    float timeLeft;

    // Use this for initialization
    void Start()
    {
        TwitterUtility.director = this;

        Beat.Bpm = bpm;
        Beat.Offset = offset;

        directionMap = new Dictionary<Direction, Relation>
        {
            [Direction.Up] = Relation.Equal,
            [Direction.Left] = Relation.Smaller,
            [Direction.Right] = Relation.Larger
        };
        state = GameState.Tutorial;
    }

    // Update is called once per frame
    void Update()
    {
        Beat.Bpm = bpm;
        Beat.Offset = offset;

        switch (state)
        {
            case GameState.Tutorial:
                BeginTutorial();
                break;
            case GameState.WaitForTutorial:
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
                GameOver();
                break;
            case GameState.Result:
                // 状態遷移はsubmitManagerに任せる
                break;
            case GameState.Ranking:
                // 状態遷移はrankingManagerに任せる
                break;
        }
    }

    public void GoToHome()
    {
        state = GameState.Tutorial;
    }

    public void StartPlaying()
    {
        state = GameState.JustStarted;
    }

    public void GoToRanking()
    {
        state = GameState.Ranking;
        ShowRanking().WrapErrors();
    }

    void BeginTutorial()
    {
        Beat.Bpm = 60;
        tutorialDirector.BeginTutorial();
    }

    async Task EndTutorialAndPlay()
    {
        Beat.Bpm = 0;
        state = GameState.WaitForTutorial;
        await tutorialDirector.TerminateTutorial();
        StartPlaying();
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

    void GameOver()
    {
        submitManager.ShowSubmitModal(score, rankingManager.MyHighScore);
        state = GameState.Result;
    }

    async Task ShowRanking()
    {
        await rankingManager.ShowRanking();
    }

    public void ToTwitter()
    {
        var tweet = $"レベル{score.ReachedLevel}に{score.TimeSum:f2}秒で到達しました！";
        naichilab.UnityRoomTweet.Tweet("marble10", tweet, "unityroom", "unity1week");
    }

    public async Task Input(Direction direction)
    {
        if (state == GameState.Tutorial)
        {
            EndTutorialAndPlay().WrapErrors();
            return;
        }
        if (state != GameState.InTenSeconds) return;

        var relation = directionMap[direction];
        var result = examiner.Answer(relation);
        var nextMarbleNum = examiner.NextQuestion();
        var goNextLevel = ProcessResult(result);

        await spawner.Swipe(direction);
        if (!goNextLevel)
            await spawner.Spawn(nextMarbleNum);
    }

    bool ProcessResult(Result result)
    {
        audioManager.PlayAnswer(result);
        var next = progressManager.SetProgress(result);
        if (next)
        {
            state = GameState.LevelCleared;
        }

        return next;
    }

    public async Task SubmitToRanking(string userName)
    {
        audioManager.PlayAnswer(Result.Correct);
        await rankingManager.SendScore(userName, score);
    }
}

enum GameState
{
    Tutorial,
    WaitForTutorial,
    JustStarted,
    InTenSeconds,
    LevelCleared,
    LevelUp,
    GameOver,
    Result,
    Ranking,
}