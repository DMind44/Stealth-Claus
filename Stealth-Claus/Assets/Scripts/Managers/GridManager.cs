using UnityEngine;
public class GridManager : MonoBehaviour
{
    public uint width, height;
    
    private Tile[,] tiles;
    
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
        
    }

    public Tile getTile(uint x, uint y)
    {
        if (x >= width || y >= height) return null;

        return tiles[x, y];
    }

    public Vector3 convertPoint(Vector2 vec3)
    {
        return new Vector3(vec3.x, vec3.y, 0f);
    }

    public void setTile(uint x, uint y, Tile tile)
    {
        if (x >= width || y >= height) return;
        tiles[x, y] = tile;
        if (tile != null)
        {
            tile.transform.position = new Vector3(x, y, 0);
            /*new Vector3(((float)x - ((float)width - 1) / 2), ((float)y - ((float)height - 1) / 2), 0f);*/
        }
    }
}
