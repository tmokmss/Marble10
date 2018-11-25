using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Director director;

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
        keyMap.Keys.Where(key => Input.GetKeyDown(key)).Select(key => keyMap[key]).Take(1).Select(dir=>director.Input(dir)).ToList();
    }
}
