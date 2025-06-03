using UnityEngine;

public class Fireplace : MapTile
{
    public Santa santaPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnSanta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnSanta()
    {
        var pos = transform.position;
        pos.z = -1;
        var santa = Instantiate(santaPrefab, pos, Quaternion.identity);
        santa.moveTo(x, y);
    }
}
