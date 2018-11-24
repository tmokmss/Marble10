using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI instructionText;

    Vector3 comboTextInitialPosition;
    Vector3 timeTextInitialPosition;
    Vector3 instructionTextInitialPosition;
    float instructionTextInitialSize;

    // Use this for initialization
    void Start()
    {
        comboTextInitialPosition = comboText.transform.position;
        timeTextInitialPosition = timeText.transform.position;
        instructionTextInitialPosition = instructionText.transform.position;

        instructionTextInitialSize = instructionText.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        DopeBeat();
    }

    public void SetCombo(int comboNum)
    {
        if (comboNum <= 1)
        {
            comboText.SetText("");
            return;
        }
        comboText.SetText($"<size=50>x </size>{comboNum}");
    }

    public void SetTime(float timeLeft)
    {
        timeText.SetText($"{timeLeft:f2}");
    }

    public void SetLevel(int currentLevel)
    {
        instructionText.SetText($"||\n< {currentLevel} <");
    }

    void DopeBeat()
    {
        var dilate = Beat.Sine(0.05f, 0.15f);
        comboText.fontSharedMaterial.SetFloat("_FaceDilate", dilate);
        timeText.fontSharedMaterial.SetFloat("_FaceDilate", dilate);

        // なぜZに1たさないといけないかはよくわからん
        // Screen Space - CameraでCanvasを描画するようにしたら必要になった
        var posDiff = new Vector3(0, Beat.Sine(0, 0.10f), 1);
        comboText.transform.position = comboTextInitialPosition + posDiff;
        timeText.transform.position = timeTextInitialPosition +  posDiff;
        instructionText.transform.position = instructionTextInitialPosition + new Vector3(0, 0, 1);

        instructionText.fontSize = instructionTextInitialSize +  Beat.Sine(0, 2);
    }
}
