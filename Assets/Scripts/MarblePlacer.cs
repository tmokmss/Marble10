using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarblePlacer : MonoBehaviour
{
    [SerializeField] GameObject marblePrefab;
    [SerializeField] Board board;

    List<Marble> marbles = new List<Marble>();

    public void RemoveAllMarbles()
    {
        marbles.ForEach(child => Destroy(child.gameObject));
        marbles.Clear();
    }

    public void PlaceMarbles(int iconNum)
    {
        //var size = Vector3.Scale(marblePrefab.GetComponent<Renderer>().bounds.size, marblePrefab.transform.localScale);
        var size = new Vector2();
        for (var i = 0; i < iconNum; i++)
        {
            var marble = Instantiate(marblePrefab, transform);
            marble.transform.position = SamplePosition(board.MinBound, board.MaxBound, size.x / 2);
            marbles.Add(marble.GetComponent<Marble>());
        }
    }

    public void ShowHintAll(string color = "#FF0000")
    {
        for (var i = 0; i < marbles.Count; i++)
        {
            marbles[i].ShowHint(i + 1, color);
        }
    }

    Vector2 SamplePosition(Vector2 min, Vector2 max, float iconRadius)
    {
        return new Vector2(Random.Range(min.x + iconRadius, max.x - iconRadius), Random.Range(min.y + iconRadius, max.y - iconRadius));
    }
}
