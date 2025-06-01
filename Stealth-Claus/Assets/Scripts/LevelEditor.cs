using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevel;
    private List<GameObject> tilePrefabs;
    private int selectedTileIndex = 0;
    private Vector2 scrollPos;
    private string prefabFolderPath = "Assets/Prefabs/MapTiles";

    [MenuItem("Tools/Level Editor")]
    public static void OpenWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnEnable()
    {
        LoadTilePrefabs();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        currentLevel = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevel, typeof(LevelData), false);

        EditorGUILayout.Space();
        prefabFolderPath = EditorGUILayout.TextField("Prefab Folder Path:", prefabFolderPath);

        if (GUILayout.Button("Reload Prefabs"))
        {
            LoadTilePrefabs();
        }

        if (tilePrefabs == null || tilePrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("No prefabs found! Make sure prefabs exist in the folder path.", MessageType.Warning);
            return;
        }

        DrawTilePalette();

        if (currentLevel == null) return;

        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        DrawTileGrid();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        if (GUILayout.Button("Save Level"))
        {
            EditorUtility.SetDirty(currentLevel);
            AssetDatabase.SaveAssets();
        }
    }

    private void LoadTilePrefabs()
    {
        tilePrefabs = new List<GameObject>();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                tilePrefabs.Add(prefab);
            }
        }
    }

    private void DrawTilePalette()
    {
        EditorGUILayout.LabelField("Tile Palette", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < tilePrefabs.Count; i++)
        {
            Texture2D preview = AssetPreview.GetAssetPreview(tilePrefabs[i]);
            if (preview == null)
            {
                preview = Texture2D.grayTexture;
            }

            GUIStyle style = new GUIStyle(GUI.skin.button);
            if (i == selectedTileIndex)
            {
                style.normal.background = Texture2D.whiteTexture;
            }

            if (GUILayout.Button(preview, style, GUILayout.Width(50), GUILayout.Height(50)))
            {
                selectedTileIndex = i;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawGridLines(Rect gridRect, int gridWidth, int gridHeight, float cellSize)
    {
        Handles.color = new Color(1f, 1f, 1f, 0.3f); // Light white lines with transparency

        // Vertical lines
        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 p1 = new Vector3(gridRect.x + x * cellSize, gridRect.y);
            Vector3 p2 = new Vector3(gridRect.x + x * cellSize, gridRect.y + gridHeight * cellSize);
            Handles.DrawLine(p1, p2);
        }

        // Horizontal lines
        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 p1 = new Vector3(gridRect.x, gridRect.y + y * cellSize);
            Vector3 p2 = new Vector3(gridRect.x + gridWidth * cellSize, gridRect.y + y * cellSize);
            Handles.DrawLine(p1, p2);
        }
    }

    private void RemoveTileAtPosition(int x, int y)
    {
        TileData tile = GetTileAtPosition(x, y);
        if (tile != null)
        {
            currentLevel.tiles.Remove(tile);
        }
    }

    private void DrawTileGrid()
    {
        EditorGUILayout.LabelField("Tile Grid", EditorStyles.boldLabel);

        int gridWidth = currentLevel.gridWidth;
        int gridHeight = currentLevel.gridHeight;
        float cellSize = 30f;

        Rect gridRect = GUILayoutUtility.GetRect(gridWidth * cellSize, gridHeight * cellSize);

        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;

        for (int y = 0; y < gridHeight; y++)
        {
            int flippedY = (gridHeight - 1) - y;

            for (int x = 0; x < gridWidth; x++)
            {
                Rect cellRect = new Rect(
                    gridRect.x + x * cellSize,
                    gridRect.y + flippedY * cellSize,
                    cellSize,
                    cellSize
                );

                EditorGUI.DrawRect(cellRect, new Color(0.2f, 0.2f, 0.2f, 1f));

                TileData tile = GetTileAtPosition(x, y);
                if (tile != null && tile.tileID >= 0 && tile.tileID < tilePrefabs.Count)
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(tilePrefabs[tile.tileID]);
                    if (preview != null)
                    {
                        GUI.DrawTexture(cellRect, preview, ScaleMode.ScaleToFit);
                    }
                }

                if (cellRect.Contains(mousePos))
                {
                    EditorGUI.DrawRect(cellRect, new Color(1f, 1f, 1f, 0.1f));

                    if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
                    {
                        SetTileAtPosition(x, y, selectedTileIndex);
                        e.Use();
                    }
                    else if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 1)
                    {
                        RemoveTileAtPosition(x, y);
                        e.Use();
                    }
                }
            }
        }

        DrawGridLines(gridRect, gridWidth, gridHeight, cellSize);
    }


    private TileData GetTileAtPosition(int x, int y)
    {
        return currentLevel.tiles.Find(t => t.position == new Vector2Int(x, y));
    }

    private void SetTileAtPosition(int x, int y, int tileID)
    {
        TileData tile = GetTileAtPosition(x, y);
        if (tile == null)
        {
            tile = new TileData { position = new Vector2Int(x, y), tileID = tileID };
            currentLevel.tiles.Add(tile);
        }
        else
        {
            tile.tileID = tileID;
        }
    }
}
