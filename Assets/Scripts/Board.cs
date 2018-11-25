using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2 MinBound => plane.bounds.min;
    public Vector2 MaxBound => plane.bounds.max;

    [SerializeField, Range(0, 1)] float swipeProgerss;

    [SerializeField] MeshRenderer plane;

    float collisionWidth = 15.5f;

    void Start()
    {
        CreateColliders();
    }

    void CreateColliders()
    {
        var size = plane.bounds.size;
        var max = plane.bounds.max;
        var min = plane.bounds.min;

        var up = gameObject.AddComponent<BoxCollider2D>();
        var bottom = gameObject.AddComponent<BoxCollider2D>();
        var right = gameObject.AddComponent<BoxCollider2D>();
        var left = gameObject.AddComponent<BoxCollider2D>();
        up.offset = new Vector2(0, (size.y + collisionWidth) / 2);
        up.size = new Vector2(size.x + collisionWidth * 2, collisionWidth);
        bottom.offset = new Vector2(0, -(size.y + collisionWidth) / 2);
        bottom.size = new Vector2(size.x + collisionWidth * 2, collisionWidth);
        left.offset = new Vector2(-(size.x + collisionWidth) / 2, 0);
        left.size = new Vector2(collisionWidth, size.y + collisionWidth);
        right.offset = new Vector2((size.x + collisionWidth) / 2, 0);
        right.size = new Vector2(collisionWidth, size.y + collisionWidth);
        up.enabled = false;
    }

    public async Task Spawn(float duration)
    {
        transform.rotation = Quaternion.identity;
        var start = Time.time;
        while (true)
        {
            var progress = (Time.time - start) / duration;
            progress *= progress;
            var posy = Mathf.Lerp(-10, 0, progress);
            transform.position = new Vector3(0, posy, 0);
            if (progress >= 1)
            {
                break;
            }
            await new WaitForEndOfFrame();
        }
        await new WaitForSeconds(0.1f);
    }

    void Swipe(Direction dir, float progress)
    {
        var rotation = 0f;
        Vector2 offset = new Vector2();

        if (dir != Direction.Up)
        {
            rotation = Mathf.Lerp(0, 25, progress) * (dir == Direction.Left ? 1 : -1);
            var ofsx = Mathf.Lerp(0, 8, progress) * (dir == Direction.Left ? -1 : 1);
            offset = new Vector2(ofsx, 0);
        }
        else
        {
            var ofsy = Mathf.Lerp(0, 11, progress);
            offset = new Vector2(0, ofsy);
        }

        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position = offset;
    }

    public async Task StartSwipe(Direction dir, float duration)
    {
        var start = Time.time;
        duration = dir == Direction.Up ? duration / (16f / 9f) : duration;
        while (true)
        {
            var progress = (Time.time - start) / duration;
            progress *= progress;
            Swipe(dir, progress);
            if (progress >= 1)
            {
                break;
            }
            await new WaitForEndOfFrame();
        }
        await new WaitForSeconds(0.1f);
    }
}
