using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public string levelName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<TMP_Text>().text = levelName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel()
    {
        if (int.TryParse(levelName, out int levelNum))
        {
            MenuManager.instance.level = levelNum - 1;
            MenuManager.instance.StartButton();
        }
    }
}
