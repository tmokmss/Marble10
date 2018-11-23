using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    [SerializeField] Behaviour halo;
    [SerializeField, Range(-1, 1)] float offset;
    [SerializeField, Range(30, 200)] int bpm = 120;
    [SerializeField, Range(0, 1)] float factor = 0.25f;

    SpriteRenderer renderer;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float h, s, v;
        Color.RGBToHSV(renderer.color, out h, out s, out v);
        renderer.color = Color.HSVToRGB(h, s, CalculateValue());
        //   halo.r
    }

    float CalculateValue()
    {
        var frequency = bpm / 60;
        var period = 1 / frequency;
        var omega = 2 * Mathf.PI * frequency;
        var value = Mathf.Sin(omega * (Time.time + period * offset));
        return Mathf.Sign(value) * value * value * factor + 1 - factor;
        //return Mathf.Sign(value) * Mathf.Sqrt(Mathf.Abs(value)) * factor + 1 - factor;
        return value * factor + 1 - factor;
    }
}
