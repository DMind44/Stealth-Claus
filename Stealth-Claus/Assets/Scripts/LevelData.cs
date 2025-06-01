using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector2Int position;
    public int tileID; // Index into the palette
}

[CreateAssetMenu(menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public List<TileData> tiles = new List<TileData>();
    public int gridWidth = 20;
    public int gridHeight = 20;
    
    public TileData GetTileAtPosition(int x, int y)
    {
        return tiles.Find(t => t.position == new Vector2Int(x, y));
    }
}
