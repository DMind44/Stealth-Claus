using UnityEngine;

public class Tree : Tile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        initAfterStart();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Tile possibleSanta = GetTileDelta(i, j);
                if (possibleSanta != null && possibleSanta.isSanta() && !GridManager.instance.isFrozen())
                {
                    GridManager.instance.startWon();
                }
            }
        }
    }
}
