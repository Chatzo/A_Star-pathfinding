using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Tile : MonoBehaviour
{
    private Color startingColor;
    private Renderer tileRenderer;

    private void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
    }
    internal void Init(Color startingColor)
    {
        this.startingColor = startingColor;
        SetColor(startingColor);
    }

    internal void SetColor(Color newColor)
    {
        tileRenderer.material.color = newColor;
    }

    internal void ResetColor()
    {
        SetColor(startingColor);
    }
}
