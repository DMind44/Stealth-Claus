using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;



public class LevelEditorWindow : EditorWindow
{
    private List<LevelData> allLevels;
    private int selectedLevelIndex = -1;
    private LevelData currentLevel;
    private List<GameObject> tilePrefabs;
    private List<GameObject> entityPrefabs;
    private int selectedTileIndex = 0;
    private int selectedEntityIndex = 0;
    private Vector2 scrollPos;
    public TilePalette tilePalette;

    public string actionText = "";
    private EntityData selectedEntity = null;

    private enum EditorMode {Map, Entity}
    private EditorMode mode = EditorMode.Map;
    
    [MenuItem("Tools/Level Editor")]
    public static void OpenWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnGUI()
    {
        mode = (EditorMode)EditorGUILayout.EnumPopup("Mode", mode);
        EditorGUILayout.Space();

        // Level Selection Dropdown
        EditorGUILayout.LabelField("Select Level", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        if (allLevels.Count == 0)
        {
            EditorGUILayout.HelpBox("No LevelData assets found! Create one in your project.", MessageType.Warning);
            return;
        }

        List<string> levelNames = new List<string>();
        foreach (var level in allLevels)
        {
            levelNames.Add(level.name);
        }

        selectedLevelIndex = EditorGUILayout.Popup("Level", selectedLevelIndex, levelNames.ToArray());

        if (selectedLevelIndex >= 0 && selectedLevelIndex < allLevels.Count)
        {
            currentLevel = allLevels[selectedLevelIndex];
        }
        else
        {
            currentLevel = null;
        }

        // Proceed only if currentLevel is selected
        if (currentLevel == null)
        {
            EditorGUILayout.HelpBox("Please select a level to edit.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Reload Prefabs"))
        {
            LoadTilePrefabs();
        }

        if (tilePalette == null)
        {
            EditorGUILayout.HelpBox("No TilePalette assigned in the selected LevelData!", MessageType.Warning);
            return;
        }

        DrawTilePalette();

        EditorGUILayout.Space();
        try
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawTileGrid();
        }
        finally
        {
            EditorGUILayout.EndScrollView();
        }
        
        if (mode == EditorMode.Entity && selectedEntity != null)
        {
            DrawEntityActions(selectedEntity);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Save Level"))
        {
            EditorUtility.SetDirty(currentLevel);
            AssetDatabase.SaveAssets();
        }
    }

    private void LoadTilePrefabs()
    {
        tilePalette = currentLevel.palette;
        tilePrefabs = new List<GameObject>();
        entityPrefabs = new List<GameObject>();
        foreach (var tilePrefab in tilePalette.tilePrefabs)
        {
            if (tilePrefab != null)
            {
                tilePrefabs.Add(tilePrefab);
            }
        }
        foreach (var tilePrefab in tilePalette.entityPrefabs)
        {
            if (tilePrefab != null)
            {
                entityPrefabs.Add(tilePrefab);
            }
        }
    }

    private void DrawTilePalette()
    {
        EditorGUILayout.LabelField("Tile Palette", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        List<GameObject> currentPalette = mode == EditorMode.Map ? tilePrefabs : entityPrefabs;
        //if (currentPalette.Count == 0) return;
        for (int i = 0; i < currentPalette.Count; i++)
        {
            Texture2D preview = AssetPreview.GetAssetPreview(currentPalette[i]);
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
                if (mode == EditorMode.Map)
                {
                    selectedTileIndex = i;
                }
                else if (mode == EditorMode.Entity)
                {
                    selectedEntityIndex = i;
                }
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
                
                EntityData entity = GetEntityAtPosition(x, y);
                if (entity != null && entity.entityID >= 0 && entity.entityID < entityPrefabs.Count && mode == EditorMode.Entity)
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(entityPrefabs[entity.entityID]);
                    if (preview != null)
                    {
                        GUI.DrawTexture(cellRect, preview, ScaleMode.ScaleToFit);
                    }
                }

                if (cellRect.Contains(mousePos))
                {
                    EditorGUI.DrawRect(cellRect, new Color(1f, 1f, 1f, 0.1f));
                    if (mode == EditorMode.Map)
                    {
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
                    if (mode == EditorMode.Entity)
                    {
                        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
                        {
                            SetEntityAtPosition(x, y, selectedEntityIndex);
                            selectedEntity = GetEntityAtPosition(x, y);
                            LoadActionCommands(selectedEntity);
                            e.Use();
                        }
                        else if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 1)
                        {
                            RemoveEntityAtPosition(x, y);
                            selectedEntity = null;
                            actionText = "";  // clear text area
                            e.Use();
                        }
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
    
    private EntityData GetEntityAtPosition(int x, int y)
    {
        return currentLevel.entities.Find(e => e.position == new Vector2Int(x, y));
    }

    private void SetEntityAtPosition(int x, int y, int entityID)
    {
        EntityData entity = GetEntityAtPosition(x, y);
        if (entity == null)
        {
            entity = new EntityData { position = new Vector2Int(x, y), entityID = entityID };
            currentLevel.entities.Add(entity);
        }
        /*else
        {
            entity.entityID = entityID;
        }*/
    }
    
    private void RemoveEntityAtPosition(int x, int y)
    {
        var entity = GetEntityAtPosition(x, y);
        if (entity != null)
        {
            currentLevel.entities.Remove(entity);
        }
    }

    private void OnEnable()
    {
        LoadLevelDataAssets();
        LoadTilePrefabs();
    }

    private void LoadLevelDataAssets()
    {
        allLevels = new List<LevelData>();
        string[] guids = AssetDatabase.FindAssets("t:LevelData");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelData level = AssetDatabase.LoadAssetAtPath<LevelData>(path);
            if (level != null)
            {
                allLevels.Add(level);
            }
        }

        if (allLevels.Count != 0)
        {
            selectedLevelIndex = 0;
            currentLevel = allLevels[selectedLevelIndex];
        }
    }

    private void DrawEntityActions(EntityData entity)
    {
        EditorGUILayout.LabelField("Entity Actions", EditorStyles.boldLabel);
        actionText = EditorGUILayout.TextArea(actionText, GUILayout.Height(100));

        if (GUILayout.Button("Apply Actions"))
        {
            entity.actions = ParseActions(actionText);
        }
    }

    private List<EntityAction> ParseActions(string text)
    {
        var actions = new List<EntityAction>();
        var lines = text.Split(new[] { '\n', '\r' });

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            if (string.IsNullOrEmpty(trimmedLine)) continue;
            
            var tokens = trimmedLine.Split(' ');

            var command = tokens[0];

            if (command.Equals("Move", StringComparison.InvariantCultureIgnoreCase) && tokens.Length >= 5)
            {
                if (int.TryParse(tokens[1], out int dx) && int.TryParse(tokens[2], out int dy) &&
                    int.TryParse(tokens[3], out int dist) && int.TryParse(tokens[4], out int speed))
                {
                    actions.Add(new MoveAction
                    {
                        dx = dx,
                        dy = dy,
                        distance = dist,
                        speed = speed
                    });
                }
            } 
            else if (command.Equals("Wait", StringComparison.OrdinalIgnoreCase) && tokens.Length >= 2)
            {
                if (int.TryParse(tokens[1], out int duration))
                {
                    actions.Add(new WaitAction
                    {
                        duration = duration
                    });
                }
            }
            else
            {
                Debug.LogWarning($"Unrecognized command: {trimmedLine}");
            }
        }
        return actions;
    }
    
    private void LoadActionCommands(EntityData entity)
    {
        var sb = new System.Text.StringBuilder();

        if (entity.actions == null)
        {
            actionText = "";
            return;
        }
        foreach (var action in entity.actions)
        {
            if (action is MoveAction move)
            {
                sb.AppendLine($"Move {move.dx} {move.dy} {move.distance}");
            }
            else if (action is WaitAction wait)
            {
                sb.AppendLine($"Wait {wait.duration}");
            }
        }
        actionText = sb.ToString();
    }

}
