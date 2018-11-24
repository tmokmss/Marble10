using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    [SerializeField] Renderer sphereMesh;
    [SerializeField, Range(0, 1)] float factor = 0.15f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float h, s, v;
        var mat = sphereMesh.sharedMaterial;
        Color.RGBToHSV(mat.GetColor("_EmissionColor"), out h, out s, out v);
        var val = Beat.Sine(0.35f, 0.2f);
        mat.SetColor("_EmissionColor", Color.HSVToRGB(h, s, val));
    }

    float CalculateValue()
    {
        var value = Beat.Sine(0, 1);
        //return Mathf.Sign(value) * value * value * factor + 1 - factor;
        //return Mathf.Sign(value) * Mathf.Sqrt(Mathf.Abs(value)) * factor + 1 - factor;
        return value * factor + 1 - factor;
    }
}
