using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class TutorialDirector : MonoBehaviour
{
    public bool IsCanvasShown => canvas.gameObject.activeSelf;

    [SerializeField] MarblePlacer placer;
    [SerializeField] Spawner spawner;
    [SerializeField] Canvas canvas;
    [SerializeField] TextManager textManager;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] TextMeshProUGUI shonariText;
    [SerializeField] TextMeshProUGUI dainariText;
    [SerializeField] TextMeshProUGUI equalText;

    CancellationTokenSource tokenSource;
    Task tutorialTask;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCanvasShown) return;
    }

    void ShowCanvas()
    {
        canvas.gameObject.SetActive(true);
    }

    void HideCanvas()
    {
        canvas.gameObject.SetActive(false);
    }

    public void BeginTutorial()
    {
        if (tokenSource != null) return;
        tokenSource = new CancellationTokenSource();
        tutorialTask = BeginTutorial(tokenSource.Token);
    }

    public async Task TerminateTutorial()
    {
        tokenSource.Cancel();
        await tutorialTask;
        HideCanvas();
        tokenSource = null;
    }

    async Task BeginTutorial(CancellationToken token)
    {
        ShowCanvas();
        int tutorialLevel = 5;
        var diffs = new int[] { 0, -1, 1 };
        // 任意タイミングで中断したいときはCroutineのほうが便利が良いな……
        foreach (var diff in diffs)
        {
            var red = "#FF0000";
            await spawner.Spawn(tutorialLevel + diff);
            RevertColor();
            textManager.SetLevel(tutorialLevel);
            tutorialText.text = "YOU SEE MARBLES";
            await new WaitForSeconds(3f);

            if (token.IsCancellationRequested) return;

            tutorialText.text = "YOU COUNT";
            placer.ShowHintAll();
            await new WaitForSeconds(3f);

            if (token.IsCancellationRequested) return;

            tutorialText.text = "YOU COMPARE";
            placer.ShowHintAll("#FFFFFF");
            textManager.SetLevel(tutorialLevel, red);
            await new WaitForSeconds(3f);

            if (token.IsCancellationRequested) return;

            tutorialText.text = "THEN YOU PUSH!";
            var relation = ChooseRelation(diff);
            ChangeColor(relation);
            textManager.SetLevel(tutorialLevel, red, relation);
            await new WaitForSeconds(1f);

            if (token.IsCancellationRequested) return;

            await spawner.Swipe(ChooseDirection(relation));
        }
        tutorialText.text = "Push key to start";
    }

    void RevertColor()
    {
        var normalColor = Color.black;

        shonariText.color = normalColor;
        dainariText.color = normalColor;
        equalText.color = normalColor;
    }

    void ChangeColor(Relation relation)
    {
        var strongColor = Color.red;

        switch (relation)
        {
            case Relation.Equal:
                equalText.color = strongColor;
                break;
            case Relation.Larger:
                dainariText.color = strongColor;
                break;
            case Relation.Smaller:
                shonariText.color = strongColor;
                break;
        }
    }

    Direction ChooseDirection(Relation relation)
    {
        switch (relation)
        {
            case Relation.Equal:
                return Direction.Up;
            case Relation.Larger:
                return Direction.Right;
            case Relation.Smaller:
                return Direction.Left;
            default:
                return Direction.Up;
        }
    }

    Relation ChooseRelation(int diff)
    {
        if (diff > 0) return Relation.Larger;
        if (diff < 0) return Relation.Smaller;
        return Relation.Equal;
    }
}
