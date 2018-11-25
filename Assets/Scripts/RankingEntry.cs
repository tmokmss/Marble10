using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static LeaderboardManager;

public class RankingEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI timeText;

    public void SetRecord(int rank, ScoreData data)
    {
        var score = Scorer.Decode(data.score);
        rankText.text = $"{rank}";
        nameText.text = data.playerName;
        levelText.text = $"{score.ReachedLevel}";
        timeText.text = $"{score.TimeSum:f2}";
    }

    public void ChangeTextColor()
    {
        var color = Color.yellow;
        rankText.color = color;
        nameText.color = color;
        levelText.color = color;
        timeText.color = color;
    }
}
