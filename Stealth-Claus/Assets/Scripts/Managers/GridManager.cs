using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public int width, height;

    private bool isCaught = false;
    private bool isWon = false;

    private double timer = 0;
    
    public Tile[,] tiles;

    public static GridManager Instance;
    
    public GameObject[] entityPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (MapManager.Instance != null)
        {
            entityPrefabs = MapManager.Instance.levelData.palette.entityPrefabs;
            width = MapManager.Instance.width;
            height = MapManager.Instance.height;
            tiles = new Tile[width, height];
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //if (MapManager.Instance.levelData != null) BuildLevelFromData();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCaught)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isCaught = false;
                restart();
            }
        }
        else if (isWon)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isWon = false;
                nextLevel();
            }
        }
    }

    public void nextLevel()
    {
        GameManager.Instance.NextLevel();
    }

    public void startWon()
    {
        if (!isFrozen())
        {
            timer = 2;
            isWon = true;
        }
    }

    public void restart()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Tile t = getTile(i, j);
                if (t != null)
                {
                    t.restart();
                }
            }
        }
    }

    public void startCaught()
    {
        isCaught = true;
        timer = 2;
    }

    public bool isFrozen()
    {
        return isCaught || isWon;
    }

    public Tile getTile(int x, int y)
    {
        if (x >= width || y >= height || x < 0 || y < 0) return null;

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
        foreach (var tile in MapManager.Instance.levelData.entities)
        {
            if (tile == null) continue;
            var prefab = entityPrefabs[tile.entityID];
            var entity = Instantiate(prefab, new Vector3(tile.position.x, tile.position.y, 0f), Quaternion.identity);

            var isParent = entity.TryGetComponent<Parent>(out var parent);
            var dataActions = tile.actions;
            if (isParent && dataActions != null)
            {
                parent.parentActions.Clear();
                foreach (var action in dataActions)
                {
                    ParentAction newAction = new ParentAction();
                    switch (action.actionType)
                    {
                        case "Move":
                            newAction.directionX = action.param1;
                            newAction.directionY = action.param2;
                            newAction.distance = action.param3;
                            newAction.speed = action.param4;
                            newAction.delay = 0;
                            break;
                        case "Wait":
                            newAction.directionX = 0;
                            newAction.directionY = 0;
                            newAction.distance = 0;
                            newAction.speed = 0;
                            newAction.delay = action.param1;
                            break;
                    }
                    parent.parentActions.Add(newAction);
                }
                parent.enabled = true;
            }
            entity.GetComponent<Tile>().moveTo(tile.position.x, tile.position.y);
        }
    }
}
