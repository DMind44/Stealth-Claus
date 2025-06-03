using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Sprite sprite;

    public int x, y;

    private bool initialized = false;

    protected void initAfterStart()
    {
        if (!initialized)
        {
            GridManager.instance.setTile(x, y, this);
            initialized = true;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        GridManager.instance = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    public void moveTo(int newX, int newY)
    {
        if (GridManager.instance != null && newX < GridManager.instance.width && newY < GridManager.instance.height) {
            GridManager.instance.setTile(x, y, GridManager.instance.getTile(newX, newY));
            x = newX; y = newY;
            GridManager.instance.setTile(x, y, this);
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
        if (x < GridManager.instance.width && y < GridManager.instance.height && x >= 0 && y >= 0)
        {
            return GridManager.instance.getTile((int)x, (int)y) != null;
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
