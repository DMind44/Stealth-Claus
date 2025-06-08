using System;
using UnityEngine;

public class Tree : MapTile
{
    private Collider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Tile possibleSanta = GetTileDelta(i, j);
                if (possibleSanta != null && possibleSanta.isSanta() && !GridManager.Instance.isFrozen())
                {
                    GridManager.Instance.startWon();
                }
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GridManager.Instance.startWon();
            Debug.Log("Won");
        }
    }
}
