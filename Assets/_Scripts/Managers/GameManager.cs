using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData gameData;
    public int theme;
    public int currentLevel;
    public string nextScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        theme = gameData.theme;
        currentLevel = gameData.level;
    }

    public void SetNextSceneName(string nextScene = "Main Menu")
    {
        this.nextScene = nextScene;
    }
}
