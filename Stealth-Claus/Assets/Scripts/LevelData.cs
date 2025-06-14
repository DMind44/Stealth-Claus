using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector2Int position;
    public int tileID; // Index into the palette
}

[System.Serializable]
public class EntityActionData
{
    public string actionType;
    public int param1;
    public int param2;
    public int param3;
    public int param4;
}

[System.Serializable]
public class EntityData
{
    public Vector2Int position;
    public int entityID;
    public List<EntityActionData> actions;
}

[CreateAssetMenu(menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public TilePalette palette;
    public List<TileData> tiles = new List<TileData>();
    public List<EntityData> entities = new List<EntityData>();
    public int gridWidth = 20;
    public int gridHeight = 20;
    
    public TileData GetTileAtPosition(int x, int y)
    {
        return tiles.Find(t => t.position == new Vector2Int(x, y));
    }
}
