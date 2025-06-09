using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int width, height;

    public MapTile tile;

    public Transform camera;

    public static MapManager Instance;
    
    public LevelData levelData;

    private GameObject[] tilePrefabs;
    
    void LoadTilePrefabs()
    {
        tilePrefabs = levelData.palette.tilePrefabs;
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

    public void GenerateGridFromData()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tileData = levelData.GetTileAtPosition(x, y);
                if (tileData == null) continue;
                var currentTile = tilePrefabs[tileData.tileID];
                var spawnedTile = Instantiate(currentTile, new Vector3(x, y, 1), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";
                spawnedTile.GetComponent<MapTile>().x = x;
                spawnedTile.GetComponent<MapTile>().y = y;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelData == null) GenerateGrid();
        else
        {
            
            GenerateGridFromData();
        }
        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
