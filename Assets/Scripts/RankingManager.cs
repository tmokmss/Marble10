using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static LeaderboardManager;

public class RankingManager : MonoBehaviour
{
    public bool RankingDisplayed => canvas?.activeSelf ?? false;

    [SerializeField] Director director;
    [SerializeField] LeaderboardManager slave;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject entries;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] Button homeButton;
    [SerializeField] Button replayButton;
    [SerializeField] Button twitterButton;

    // Use this for initialization
    void Start()
    {
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
        
    }

    public async Task SendScore(string playerName, Scorer scorer)
    {
        await slave.SendScore(playerName, scorer.Encode());
    }

    public async Task ShowRanking()
    {
        canvas.SetActive(true);
        await slave.GetScoreList(100, RegisterEntries);
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
