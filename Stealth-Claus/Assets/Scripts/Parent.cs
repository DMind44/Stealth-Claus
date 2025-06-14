using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class ParentAction
{

    public int directionX;
    public int directionY;
    public int distance;
    public int speed;
    public int delay;

    private float timer = 0;
    private int distanceLeft = 0;
    private bool ranDelay = false;


    public void setup()
    {
        timer = delay;
        distanceLeft = distance;
        ranDelay = false;
    }

    public void actionStep(Parent parent)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!ranDelay)
            {
                ranDelay = true;
                timer = 1 / (float)speed;
                parent.dirX = directionX;
                parent.dirY = directionY;
                return;
            }

            if (parent.tryDeltaMove(directionX, directionY))
            {
                distanceLeft--;
                timer = 1 / (float)speed;
            }
        }
    }

    public bool actionDone() {
        return distanceLeft == 0 && ranDelay;   
    }
}

public class Parent : Tile
{

    public List<ParentAction> parentActions = new List<ParentAction>();
    public int dirX = 1;
    public int dirY = 0;

    private int startDirX = 1;

    private int startDirY = 0;

    public int visionDistance = 3;

    private int actionIndex = 0;

    private Texture2D visionTexture;
    private Color visionColor;

    void DrawQuad(Rect position)
    {
        visionTexture.SetPixel(0, 0, visionColor);
        visionTexture.Apply();
        GUI.skin.box.normal.background = visionTexture;
        GUI.Box(position, GUIContent.none);
    }

    override public void restart()
    {
        base.restart();
        dirX = startDirX;
        dirY = startDirY;
        actionIndex = 0;
        parentActions[actionIndex].setup();
    }

    void drawVision()
    {
        for (int i = 1; i < visionDistance + 1; i++)
        {
            if (checkOcupiedDelta(dirX * i, dirY * i))
            {
                Tile possibleSanta = GridManager.Instance.getTile((int)x + dirX * i, (int)y + dirY * i);
                if (possibleSanta != null && possibleSanta.isSanta())
                {
                    Vector3 cords2 = camera.WorldToScreenPoint(GridManager.Instance.convertPoint(new Vector2(x - 0.5f + dirX * i, y + 0.5f + dirY * i)));
                    DrawQuad(new Rect(cords2.x + 5, Screen.height - cords2.y + 5, tileWidth - 10, tileWidth - 10));
                    if (!GridManager.Instance.isFrozen())
                    {
                        GridManager.Instance.startCaught();
                    }
                }
                break;
            }
            Vector3 cords = camera.WorldToScreenPoint(GridManager.Instance.convertPoint(new Vector2(x-0.5f + dirX*i, y+0.5f+dirY*i)));
            DrawQuad(new Rect(cords.x+5, Screen.height-cords.y+5, tileWidth-10, tileWidth-10));

        }
    }

    private Camera camera;

    private float tileWidth;
    void Start()
    {
        base.Start();
        if (parentActions.Count == 0)
        {
            parentActions.Add(new ParentAction() { directionX = 0, directionY = 0, delay = 1, speed = 0});
        }
        camera = Camera.main;
        parentActions[actionIndex].setup();
        startDirX = dirX;
        startDirY = dirY;

        tileWidth = camera.WorldToScreenPoint(new Vector3(1, 0, 0)).x - camera.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        
        visionTexture = new Texture2D(1, 1);
        visionColor = new Color(0.7f, 0.7f, 0, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        initAfterStart();
        if (!GridManager.Instance.isFrozen())
        {
            parentActions[actionIndex].actionStep(this);
            if (parentActions[actionIndex].actionDone())
            {
                actionIndex = (actionIndex + 1) % parentActions.Count;
                parentActions[actionIndex].setup();
            }
        }
    }

    void OnGUI()
    {
        drawVision();
    }
}
