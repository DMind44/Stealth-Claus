using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    protected GridManager gridManager;
    
    public Sprite sprite;

    public int x, y;

    private bool initialized = false;

    protected void initAfterStart()
    {
        if (!initialized)
        {
            gridManager.setTile(x, y, this);
            initialized = true;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    public void moveTo(int newX, int newY)
    {
        if (gridManager != null && newX < gridManager.width && newY < gridManager.height) {
            gridManager.setTile(x, y, gridManager.getTile(newX, newY));
            x = newX; y = newY;
            gridManager.setTile(x, y, this);
        }
    }
    public bool tryMoveTo(int newX, int newY)
    {
        if (!checkOcupied((int)newX, (int)newY))
        {
            moveTo(newX, newY);
            return true;
        }
        return false;
    }
    public void deltaMove(int dx, int dy)
    {
        if (dx + x >= 0 && dy + y >= 0)
        {
            moveTo((int)(dx + x), (int)(dy + y));
        }
    }

    public bool tryDeltaMove(int dx, int dy)
    {
        if (!checkOcupiedDelta(dx, dy))
        {
            deltaMove(dx, dy);
            return true;
        }
        return false;

    }
    public bool checkOcupied(int x, int y)
    {
        if (x < gridManager.width && y < gridManager.height && x >= 0 && y >= 0)
        {
            return gridManager.getTile((int)x, (int)y) != null;
        }
        return true;
    }

    public bool checkOcupiedDelta(int dx, int dy)
    {
        return checkOcupied((int)(x + dx), (int)(y + dy));
    }


    // Update is called once per frame
    void Update()
    {
        initAfterStart(); 
    }
}
