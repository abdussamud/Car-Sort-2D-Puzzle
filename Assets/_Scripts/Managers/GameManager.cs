using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData gameData;
    public bool startLoadingDone;
    public int theme;
    public int level;
    public string nextScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        theme = gameData.theme;
        level = gameData.level;
    }

    public void SetNextSceneName(string nextScene = "Main Menu")
    {
        this.nextScene = nextScene;
    }
}
