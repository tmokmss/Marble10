using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitManager : MonoBehaviour
{
    [SerializeField] Director director;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button homeButton;
    [SerializeField] Button replayButton;
    [SerializeField] Button rankingButton;

    // Use this for initialization
    void Start()
    {
        inputField.onEndEdit.AddListener(SubmitScore);
    }
    // Update is called once per frame
    void Update()
    {

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

    }

    void HideModal()
    {
        canvas.gameObject.SetActive(false);
    }

    public void ShowSubmitModal(Scorer score)
    {
        titleText.text = $@"You Reached
Level <color=#ffff00><size=80>{score.ReachedLevel}</size></color> in
<color=#ffff00><size=80>{score.TimeSum:f2}</size></color> seconds";
    }

    void SubmitScore(string name)
    {
        throw new NotImplementedException();
    }
}
