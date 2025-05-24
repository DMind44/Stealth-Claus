using UnityEngine;
public class GridManager : MonoBehaviour
{
    public uint width, height;
    
    private Tile[,] tiles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiles = new Tile[width, height];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile getTile(uint x, uint y)
    {
        if (x >= width || y >= height) return null;

        return tiles[x, y];
    }

    public void setTile(uint x, uint y, Tile tile)
    {
        if (x >= width || y >= height) return;
        tiles[x, y] = tile;
        tile.transform.position = new Vector3(((float)x-((float)width-1)/2), ((float)y-((float)height-1)/2), 0f);
    }
}
