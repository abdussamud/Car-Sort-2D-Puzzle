using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dm;
    [SerializeField]
    private bool resetData;
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
        //Debug.Log("Data Saved");
    }

    private void LoadData()
    {
        string json = File.ReadAllText(filePath, encoding: Encoding.UTF32);
        GameData loadedData = JsonConvert.DeserializeObject<GameData>(json);

        gameData.coins = loadedData.coins;
        gameData.sound = loadedData.sound;
        gameData.music = loadedData.music;
        gameData.level = loadedData.level;
        gameData.theme = loadedData.theme;
        gameData.removeAds = loadedData.removeAds;
        gameData.isFirstTime = loadedData.isFirstTime;
        //Debug.Log("Data Loaded");
    }

    private void ResetData()
    {
        gameData.coins = 0;
        gameData.level = 0;
        gameData.theme = 0;
        gameData.sound = 1f;
        gameData.music = 1f;
        gameData.removeAds = false;
        gameData.isFirstTime = true;
        SaveData();
    }
}
