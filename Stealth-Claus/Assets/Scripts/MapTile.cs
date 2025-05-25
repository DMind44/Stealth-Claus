using System;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void ColorTile(bool isOffset)
    {
        _spriteRenderer.color = isOffset ? offsetColor : baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
