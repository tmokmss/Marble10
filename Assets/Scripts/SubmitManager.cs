using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitManager : MonoBehaviour
{
    public bool Enabled => canvas.gameObject.activeSelf;

    [SerializeField] Director director;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI fieldLabel;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject inputItem;
    [SerializeField] Button homeButton;
    [SerializeField] Button replayButton;
    [SerializeField] Button rankingButton;

    const string EnterName = "ENTER\nNAME:";
    const string PleaseWait = "Please\nWait";

    bool isSubmitting;
    bool isHighScore;

    // Use this for initialization
    void Start()
    {
        inputField.onEndEdit.AddListener(StartToSubmit);

        replayButton.onClick.AddListener(OnReplayButtonPressed);
        homeButton.onClick.AddListener(OnHomeButtonPressed);
        rankingButton.onClick.AddListener(OnRankingButtonPressed);
    }

    void Update()
    {
        if (!Enabled) return;

        // スコア入力中は他のキー入力受け付けない
        if (isHighScore) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnReplayButtonPressed();
        }
    }

    void OnHomeButtonPressed()
    {
        HideModal();
        director.GoToHome();
    }

    void OnReplayButtonPressed()
    {
        HideModal();
        director.StartPlaying();
    }

    void OnRankingButtonPressed()
    {
        HideModal();
        director.GoToRanking();
    }

    void HideModal()
    {
        canvas.gameObject.SetActive(false);
    }

    public void ShowSubmitModal(Scorer score, int bestScore)
    {
        Initialize();
        titleText.text = $@"You Reached
Level <color=#ffff00><size=70>{score.ReachedLevel}</size></color> in
<color=#ffff00><size=70>{score.TimeSum:f2}</size></color> seconds";
        canvas.gameObject.SetActive(true);
        isHighScore = score.Encode() < bestScore;
        if (isHighScore)
        {
            inputItem.SetActive(false);
        }
    }

    void StartToSubmit(string name)
    {
        if (isSubmitting) return;
        isSubmitting = true;
        SubmitScore(name).WrapErrors();
    }

    async Task SubmitScore(string name)
    {
        fieldLabel.text = PleaseWait;
        await director.SubmitToRanking(name);
        canvas.gameObject.SetActive(false);
        director.GoToRanking();
    }

    void Initialize()
    {
        isSubmitting = false;
        fieldLabel.text = EnterName;
    }
}
