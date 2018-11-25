﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    [SerializeField] Renderer sphereMesh;

    // Update is called once per frame
    void Update()
    {
        float h, s, v;
        var mat = sphereMesh.sharedMaterial;
        Color.RGBToHSV(mat.GetColor("_EmissionColor"), out h, out s, out v);
        var val = Beat.Sine(0.30f, 0.25f);
        mat.SetColor("_EmissionColor", Color.HSVToRGB(h, s, val));
    }
}
