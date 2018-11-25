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
    bool isSwiping;

    public void Reset()
    {
        placer.RemoveAllMarbles();
    }

    public async Task Swipe(Direction direction)
    {
        if (isSwiping) return;

        isSwiping = true;
        await board.StartSwipe(direction, duration);
        placer.RemoveAllMarbles();
        isSwiping = false;
    }

    public async Task Spawn(int nextNumber)
    {
        if (isSwiping) return;

        await board.Spawn(0.2f);
        placer.PlaceMarbles(nextNumber);
    }
}
