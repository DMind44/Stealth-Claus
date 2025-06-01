using System;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int width, height;

    public MapTile tile;

    public Transform camera;

    public static MapManager instance;
    
    public LevelData levelData;

    private GameObject[] tilePrefabs;
    
    void LoadTilePrefabs()
    {
        string prefabFolderPath = "Assets/Prefabs/MapTiles"; // same as the editor
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });
        tilePrefabs = new GameObject[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            tilePrefabs[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
    }

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
    }

    public void GenerateGridFromData(LevelData data)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tileData = data.GetTileAtPosition(x, y);
                if (tileData == null) continue;
                var currentTile = tilePrefabs[tileData.tileID];
                var spawnedTile = Instantiate(currentTile, new Vector3(x, y, 1), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelData == null) GenerateGrid();
        else GenerateGridFromData(levelData);
        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        if (levelData != null)
        {
            width = levelData.gridWidth;
            height = levelData.gridHeight;
            LoadTilePrefabs();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
