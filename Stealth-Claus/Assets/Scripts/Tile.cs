using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Sprite sprite;

    public int x, y;

    private int startx, starty;

    private bool initialized = false;

    virtual public bool isSanta()
    {
        return false;
    }

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
        startx = x;
        starty = y;
    }

    public void moveTo(int newX, int newY)
    {
        if (GridManager.instance != null && newX < GridManager.instance.width && newY < GridManager.instance.height) {

            Tile tile = GridManager.instance.getTile(newX, newY);
            if (tile != null)
            {
                tile.x = x;
                tile.y = y;
            }
            GridManager.instance.setTile(x, y, GridManager.instance.getTile(newX, newY));
            x = newX; y = newY;
            GridManager.instance.setTile(x, y, this);
        }
    }

    virtual public void restart()
    {
        moveTo(startx, starty);
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

    public Tile getTile(int x, int y)
    {
        if (x < GridManager.instance.width && y < GridManager.instance.height && x >= 0 && y >= 0)
        {
            return GridManager.instance.getTile((int)x, (int)y);
        }
        return null;
    }

    public bool checkOcupiedDelta(int dx, int dy)
    {
        return checkOcupied((int)(x + dx), (int)(y + dy));
    }

    public Tile GetTileDelta(int dx, int dy)
    {
        return getTile((int)(x + dx), (int)(y + dy));
    }


    // Update is called once per frame
    void Update()
    {
        initAfterStart(); 
    }
}
