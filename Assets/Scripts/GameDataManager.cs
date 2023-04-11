using Newtonsoft.Json;
using UnityEngine;


public class GameDataManager : MonoBehaviour
{
    public GameData gameData;

    #region Singleton
    public static GameDataManager Instance;

    internal readonly object gameDataFromPlayerPrefs;

    private void Awake() => GetInstance();

    private void GetInstance()
    {
        if (Instance != null) { Destroy(gameObject); }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private void Start() => Load();

    private void OnApplicationQuit() => Save();

    public void Save()
    {
        string gameDataString = JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString("GameData", gameDataString);
        PlayerPrefs.Save();
        //print("Data Saved");
    }

    private void Load()
    {
        string gameDataString = PlayerPrefs.GetString("GameData");
        GameData gameDataFromPlayerPrefs;
        _ = (GameData)ScriptableObject.CreateInstance(typeof(GameData));
        gameDataFromPlayerPrefs = JsonConvert.DeserializeObject<GameData>(gameDataString);
        if (gameDataFromPlayerPrefs == null)
        {
            print("No Data found.");
            return;
        }
        //print("Data Loaded");
        // Set Local GameData Variables
        gameData.diamonds = gameDataFromPlayerPrefs.diamonds;
        gameData.carPrefab = gameDataFromPlayerPrefs.carPrefab;
        gameData.unlockedLevel = gameDataFromPlayerPrefs.unlockedLevel;
        gameData.gameplaySceneBG = gameDataFromPlayerPrefs.gameplaySceneBG;
    }
}
