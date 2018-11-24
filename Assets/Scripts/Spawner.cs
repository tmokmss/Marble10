using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Boardを出現させつつMarbleも配置する
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] MarblePlacer placer;

    float duration = 0.25f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async Task AnswerAndSpawn(Direction direction, Relation answer, int nextNumber)
    {
        await board.StartSwipe(direction, duration);
        placer.RemoveAllMarbles();
        await board.Spawn(0.2f);
        placer.PlaceMarbles(nextNumber);
    }
}
