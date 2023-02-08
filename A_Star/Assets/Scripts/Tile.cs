using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Tile : MonoBehaviour
{
    private Node tileNode;
    private Color startingColor;
    private Renderer tileRenderer;

    private void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
    }
    internal void Init(Node node, Color startingColor)
    {
        this.tileNode = node;
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
