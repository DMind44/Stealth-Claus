using System;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public int width, height;

    private bool isCaught = false;
    private bool isWon = false;

    private double timer = 0;
    
    private Tile[,] tiles;

    public static GridManager instance;

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
}
