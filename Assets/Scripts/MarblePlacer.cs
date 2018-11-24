using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarblePlacer : MonoBehaviour
{
    [SerializeField] GameObject marblePrefab;
    [SerializeField] Board board;

    int itemNumber;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Refresh()
    {
        RemoveAllMarbles();
        PlaceMarbles(itemNumber);
    }

    public void RemoveAllMarbles()
    {
        var children = GetComponentsInChildren<Marble>().ToList();
        children.ForEach(child => Destroy(child.gameObject));
    }

    public void PlaceMarbles(int iconNum)
    {
        //var size = Vector3.Scale(marblePrefab.GetComponent<Renderer>().bounds.size, marblePrefab.transform.localScale);
        var size = new Vector2();
        for (var i = 0; i < iconNum; i++)
        {
            var icon = Instantiate(marblePrefab, transform);
            icon.transform.position = SamplePosition(board.MinBound, board.MaxBound, size.x / 2);
        }
    }

    Vector2 SamplePosition(Vector2 min, Vector2 max, float iconRadius)
    {
        return new Vector2(Random.Range(min.x + iconRadius, max.x - iconRadius), Random.Range(min.y + iconRadius, max.y - iconRadius));
    }
}
