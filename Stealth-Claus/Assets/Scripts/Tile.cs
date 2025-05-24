using UnityEngine;

public class Tile : MonoBehaviour
{
    public GridManager gridManager;
    
    public Sprite sprite;

    public uint x, y;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        gridManager.setTile(x, y, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
