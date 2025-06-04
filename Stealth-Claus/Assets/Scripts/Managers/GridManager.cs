using System;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public int width, height;
    
    private Tile[,] tiles;

    public static GridManager instance;
    
    public GameObject[] entityPrefabs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        width = MapManager.instance.width;
        height = MapManager.instance.height;
        tiles = new Tile[width, height];
        if (MapManager.instance.levelData != null) BuildLevelFromData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile getTile(int x, int y)
    {
        if (x >= width || y >=  height || x < 0 || y < 0) return null;

        return tiles[x, y];
    }

    public Vector3 convertPoint(Vector2 vec3)
    {
        return new Vector3(vec3.x, vec3.y, 0f);
    }

    public void setTile(int x, int y, Tile tile)
    {
        if (x >= width || y >= height || x < 0 || y < 0) return;
        tiles[x, y] = tile;
        if (tile != null)
        {
            tile.transform.position = new Vector3(x, y, 0);
            /*new Vector3(((float)x - ((float)width - 1) / 2), ((float)y - ((float)height - 1) / 2), 0f);*/
        }
    }

    public void BuildLevelFromData()
    {
        entityPrefabs = MapManager.instance.levelData.palette.entityPrefabs;
        foreach (var tile in MapManager.instance.levelData.entities)
        {
            if (tile == null) continue;
            var prefab = entityPrefabs[tile.entityID];
            var entity = Instantiate(prefab, new Vector3(tile.position.x, tile.position.y, 0f), Quaternion.identity);
            entity.GetComponent<Tile>().moveTo(tile.position.x, tile.position.y);            
        }
    }
}
