using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PleaseWait : MonoBehaviour
{
    [SerializeField] bool includeNewLine;
    [SerializeField] float interval = 0.5f;
    [SerializeField] int maxPeriodsNum = 3;
    TextMeshProUGUI text;

    const string Please = "Please";
    const string Wait = "Wait";

    int current;
    bool isShowed;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void StartView()
    {
        isShowed = true;
        gameObject.SetActive(true);
        var task = UpdateText();
    }

    async Task UpdateText()
    {
        while (isShowed)
        {
            current = current < maxPeriodsNum ? ++current : 0;
            var periods = new string('.', current);
            
            // ここも原因分からんがたまにnullになる
            if (text == null) return;
            if (includeNewLine)
            {
                text.text = $"{Please}\n{periods}{Wait}{periods}";
            }
            else
            {
                text.text = $"{periods}{Please} {Wait}{periods}";
            }
            await new WaitForSeconds(interval);
        }
    }

    public void Done()
    {
        isShowed = false;
    }

    public void Hide()
    {
        Done();
        gameObject.SetActive(false);
    }
}
