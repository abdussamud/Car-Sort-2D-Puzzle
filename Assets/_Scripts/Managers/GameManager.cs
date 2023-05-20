using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameData gameData;
    public int currentLevel;
    public GameObject[] carPrefabs;
    public Sprite[] gameplaySceneBG;
    public string nextScene;


    private void Awake()
    {
        Instance = this;
        currentLevel = gameData.unlockedLevel;
    }
}
