using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI instructionText;
    [SerializeField] TextMeshProUGUI levelUpText;

    Vector3 timeTextInitialPosition;
    Vector3 instructionTextInitialPosition;
    float instructionTextInitialSize;

    // Use this for initialization
    void Start()
    {
        timeTextInitialPosition = timeText.transform.position;
        instructionTextInitialPosition = instructionText.transform.position;

        instructionTextInitialSize = instructionText.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        DopeBeat();
    }

    public void SetTime(float timeLeft)
    {
        timeText.SetText($"{timeLeft:f2}");
    }

    public void SetLevel(int currentLevel)
    {
        instructionText.SetText($"||\n< {currentLevel} <");
    }

    public void SetLevel(int currentLevel, string colorCode)
    {
        instructionText.SetText($"||\n< <color={colorCode}>{currentLevel}</color> <");
    }

    public void SetLevel(int currentLevel, string colorCode, Relation target)
    {
        switch (target)
        {
            case Relation.Equal:
                instructionText.SetText($"<color={colorCode}>||</color>\n< {currentLevel} <");
                break;
            case Relation.Larger:
                instructionText.SetText($"||\n< {currentLevel} <color={colorCode}><</color>");
                break;
            case Relation.Smaller:
                instructionText.SetText($"||\n<color={colorCode}><</color> {currentLevel} <");
                break;
        }
    }

    public async Task ShowLevelUp(float duration)
    {
        levelUpText.gameObject.SetActive(true);
        // 位置振動させるか
        var initialPos = levelUpText.transform.position;
        var start = Time.time;
        while (true)
        {
            var progress = (Time.time - start) / duration;
            levelUpText.transform.position = initialPos + new Vector3(0, Beat.Sine(0, 0.2f), 1);
            if (progress >= 1)
            {
                break;
            }
            await new WaitForEndOfFrame();
        }
        levelUpText.gameObject.SetActive(false);
        levelUpText.transform.position = initialPos;
    }

    void DopeBeat()
    {
        var dilate = Beat.Sine(0.05f, 0.15f);
        timeText.fontSharedMaterial.SetFloat("_FaceDilate", dilate);

        // なぜZに1たさないといけないかはよくわからん
        // Screen Space - CameraでCanvasを描画するようにしたら必要になった
        var posDiff = new Vector3(0, Beat.Sine(0, 0.10f), 1);
        timeText.transform.position = timeTextInitialPosition + posDiff;
        instructionText.transform.position = instructionTextInitialPosition + new Vector3(0, 0, 1);

        instructionText.fontSize = instructionTextInitialSize + Beat.Sine(0, 2);
    }
}
