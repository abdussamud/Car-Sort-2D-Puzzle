using Newtonsoft.Json;
using System.IO;
using System.Text;
//using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] private bool resetData;
    [SerializeField] private GameData gameData;
    [SerializeField] private string filePath;


    private void Awake()
    {
        Instance = this;
        filePath = Application.persistentDataPath + "/GameFile.json";
        if (!resetData)
        {
            LoadData();
        }
        else
        {
            ResetData();
        }
    }

    private void OnApplicationQuit() => SaveData();

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(filePath, json, encoding: Encoding.UTF32);
        Debug.Log("Data Saved");

        //string dataString = JsonConvert.SerializeObject(gameData);
        //BinaryFormatter bf = new();
        //FileStream fileStream = File.Create(filePath);
        //fileStream.Close();
        //bf.Serialize(fileStream, dataString);
    }

    private void LoadData()
    {
        string json = File.ReadAllText(filePath, encoding: Encoding.UTF32);
        GameData loadedData = JsonConvert.DeserializeObject<GameData>(json);

        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream fileStream = File.OpenRead(filePath);
        //string gameDataString = PlayerPrefs.GetString("GameData");
        //GameData loadData;

        gameData.isSoundOn = loadedData.isSoundOn;
        gameData.isMusicOn = loadedData.isMusicOn;
        gameData.isFirstTime = loadedData.isFirstTime;
        gameData.diamonds = loadedData.diamonds;
        gameData.carPrefab = loadedData.carPrefab;
        gameData.unlockedLevel = loadedData.unlockedLevel;
        gameData.gameplaySceneBG = loadedData.gameplaySceneBG;
        Debug.Log("Data Loaded");
    }

    private void ResetData()
    {
        gameData.diamonds = 0;
        gameData.carPrefab = 0;
        gameData.unlockedLevel = 0;
        gameData.gameplaySceneBG = 0;
        gameData.isSoundOn = true;
        gameData.isMusicOn = true;
        gameData.isFirstTime = true;
        SaveData();
    }
}
