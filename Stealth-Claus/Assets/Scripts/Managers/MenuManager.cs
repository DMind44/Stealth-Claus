using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public int level = 0;
    private bool inGame = false;
    public int numLevels = 6;
    public LevelButton levelButtonPrefab;
    public GameObject LevelSelectButtons;
    public GameObject selectText;
    private GameObject startButton;
    private GameObject quitButton;
    private GameObject selectButton;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        startButton = GameObject.Find("StartButton").gameObject;
        quitButton = GameObject.Find("QuitButton").gameObject;
        selectButton = GameObject.Find("SelectButton").gameObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < numLevels; i++)
        {
            
            LevelButton button = Instantiate(levelButtonPrefab, LevelSelectButtons.transform);
            int adjustedLevel = i + 1;
            button.levelName = adjustedLevel.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("GameplayScene");
        inGame = true;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        if (!inGame)
        {
            startButton.SetActive(false);
            quitButton.SetActive(false);
            selectButton.SetActive(false);
            LevelSelectButtons.SetActive(true);
            selectText.SetActive(true);
        }
    }

    public void BackButton()
    {
        if (!inGame)
        {
            startButton.SetActive(true);
            quitButton.SetActive(true);
            selectButton.SetActive(true);
            LevelSelectButtons.SetActive(false);
            selectText.SetActive(false);
        }
    }
}
