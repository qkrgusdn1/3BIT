using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkStartPoint : MonoBehaviour
{
    public LinkColor linkColor;
    public LineRenderer lineRenderer;
    public bool isLink;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    
}

public enum LinkColor
{
    Red,
    Yellow,
    Green
}
