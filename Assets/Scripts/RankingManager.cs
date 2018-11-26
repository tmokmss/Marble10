using naichilab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static LeaderboardManager;

public class RankingManager : MonoBehaviour
{
    public bool RankingDisplayed => canvas.activeSelf;
    public int MyHighScore => slave.MyHighScore;
    public string MyName => slave.MyName;

    [SerializeField] Director director;
    [SerializeField] LeaderboardManager slave;
    [SerializeField] PleaseWait pleaseWait;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject entries;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] Button homeButton;
    [SerializeField] Button replayButton;
    [SerializeField] Button twitterButton;

    bool isFetching;

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        slave.ClearLocalData();
#endif
        replayButton.onClick.AddListener(OnReplayButtonPressed);
        homeButton.onClick.AddListener(OnHomeButtonPressed);
        twitterButton.onClick.AddListener(OnTwitterButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!RankingDisplayed) return;
    }

    void OnHomeButtonPressed()
    {
        HideRanking();
        director.GoToHome();
    }
    
    void OnReplayButtonPressed()
    {
        HideRanking();
        director.StartPlaying();
    }

    void OnTwitterButtonPressed()
    {
        director.ToTwitter();
    }

    public async Task SendScore(string playerName, Scorer scorer)
    {
        await slave.SendScore(playerName, scorer.Encode());
    }

    public async Task ShowRanking()
    {
        if (isFetching) return;
        isFetching = true;
        canvas.SetActive(true);
        pleaseWait.StartView();
        await slave.GetScoreList(100, RegisterEntries);
        pleaseWait.Hide();
        isFetching = false;
    }

    public void HideRanking()
    {
        entries.GetComponentsInChildren<RankingEntry>().ToList().ForEach(entry => Destroy(entry.gameObject));
        canvas.SetActive(false);
    }

    void RegisterEntries(ScoreDatas scores)
    {
        for (var i = 0; i < scores.results.Count; i++)
        {
            var data = scores.results[i];
            var entryObj = Instantiate(entryPrefab, entries.transform);
            var entry = entryObj.GetComponent<RankingEntry>();
            entry.SetRecord(i + 1, data);
            if (data.objectId == slave.MyObjectId)
            {
                entry.ChangeTextColor();
            }
        }
    }
}
