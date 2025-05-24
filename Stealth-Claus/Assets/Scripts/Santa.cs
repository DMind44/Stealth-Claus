using UnityEngine;

public class Santa : Tile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            deltaMove(-1, 0);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            deltaMove(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            deltaMove(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            deltaMove(0, 1);
        }
    }
}
