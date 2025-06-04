using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Level/Tile Palette")]
    public class TilePalette : ScriptableObject
    {
        public GameObject[] tilePrefabs;
        public GameObject[] entityPrefabs;
    }
}