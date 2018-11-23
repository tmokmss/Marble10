using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] MarblePlacer placer;
    [SerializeField] Board board;
    [SerializeField] Director director;

    [SerializeField, Range(0, 0.5f)] 

    Dictionary<KeyCode, Direction> keyMap;

    // Use this for initialization
    void Start()
    {
        keyMap = new Dictionary<KeyCode, Direction>
        {
            [KeyCode.W] = Direction.Up,
            [KeyCode.A] = Direction.Left,
            [KeyCode.D] = Direction.Right,

            [KeyCode.UpArrow] = Direction.Up,
            [KeyCode.LeftArrow] = Direction.Left,
            [KeyCode.RightArrow] = Direction.Right,
        };
    }

    // Update is called once per frame
    void Update()
    {
        // for debug
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                placer.Refresh();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                placer.Increment();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                placer.Decrement();
            }
        }

        var inputDirection = keyMap.Keys.Where(key => Input.GetKeyDown(key)).Select(key => keyMap[key]).ToList();
        inputDirection.ForEach(direction => director.Input(direction));
    }
}
