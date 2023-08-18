using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData gameData;
    public int currentLevel;
    public string nextScene;
    public GameObject[] carPrefabs;
    public Sprite[] gameplaySceneBG;

    private void Awake()
    {
        Instance = this;
        currentLevel = gameData.unlockedLevel;
    }

    public void SetNextSceneName(string nextScene)
    {
        this.nextScene = nextScene;
    }
}
