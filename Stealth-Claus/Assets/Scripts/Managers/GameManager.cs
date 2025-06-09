using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int currentLevel = 0;
    // ordered list of levels
    public LevelData[] levels;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        if (MenuManager.instance != null)
        {
            SetLevel(MenuManager.instance.level);
        }
        else
        {
            SetLevel(0);
        }
    }
    
    void Update()
    {
        
    }
    
    

    public void SetLevel(int level)
    {
        if (level >= 0 && level < levels.Length)
        {
            currentLevel = level;
            ReloadLevel();
        }
        else Debug.Log("Invalid level, out of range");
    }

    public void NextLevel()
    {
        SetLevel(currentLevel + 1);
    }

    private void ReloadLevel()
    {
        while (GridManager.Instance == null || MapManager.Instance == null)
        {
            Debug.Log("waiting for managers to load");
        }
        ClearScene();
        MapManager.Instance.levelData = levels[currentLevel];
        MapManager.Instance.GenerateGridFromData();
        GridManager.Instance.BuildLevelFromData();
    }

    private void ClearScene()
    {
        var allGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var gameObject in allGameObjects)
        {
            if (!gameObject.CompareTag("DoNotDestroy") && !gameObject.CompareTag("MainCamera"))
            {
                Destroy(gameObject);
            }
        }
    }
}
