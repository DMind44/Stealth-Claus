using System;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public uint width, height;

    public MapTile tile;

    public Transform camera;

    public static MapManager instance;

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tile, new Vector3(x, y, 1), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                
                spawnedTile.ColorTile(isOffset);
            }
        }

        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
