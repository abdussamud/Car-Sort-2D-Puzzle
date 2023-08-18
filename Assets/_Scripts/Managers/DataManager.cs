using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
//using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dm;

    [SerializeField] private bool resetData;
    public GameData gameData;
    private string filePath;

    private void Awake()
    {
        dm = this;
        filePath = Application.persistentDataPath + "/GameFile.json";
        (resetData ? (Action)ResetData : LoadData)();
    }

    private void OnApplicationQuit() { SaveData(); }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(filePath, json, encoding: Encoding.UTF32);
        Debug.Log("Data Saved");
    }

    private void LoadData()
    {
        string json = File.ReadAllText(filePath, encoding: Encoding.UTF32);
        GameData loadedData = JsonConvert.DeserializeObject<GameData>(json);

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
