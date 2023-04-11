using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [HideInInspector]
    public bool isGameOver;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private GameObject carPrefab;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private LevelManager[] level;
    private int currentLevel;
    private List<GameObject> spawnedCarPrefabs = new();




    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        UIManager.Instance.SetLevelText();
        carPrefab = GameManager.Instance.carPrefabs[gameData.carPrefab];
        currentLevel = GameManager.Instance.currentLevel;
        CarInstantiate();
    }

    public void CarInstantiate()
    {
        level[currentLevel].levelEnvironment.SetActive(true);
        SpriteRenderer gameplaySceneBG = level[currentLevel].levelEnvironment.GetComponent<SpriteRenderer>();
        gameplaySceneBG.sprite = GameManager.Instance.gameplaySceneBG[gameData.gameplaySceneBG];
        TouchManager.Instance.SetParkingCell();
        TouchManager.Instance.SetRowList();
        for (int i = 0; i < level[currentLevel].carAmount; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefab, level[currentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(level[currentLevel].carPosition[i].parent);
            spawnedPrefab.GetComponent<Car>().CarColor = level[currentLevel].carColor[i];
            spawnedPrefab.GetComponent<Car>().SetParkingCell();
            spawnedCarPrefabs.Add(spawnedPrefab);
        }
    }

    public void EndGame()
    {
        if (GameManager.Instance.currentLevel < 29 && GameManager.Instance.currentLevel == gameData.unlockedLevel)
        {
            gameData.unlockedLevel++;
            gameData.diamonds += (currentLevel + 1) * 10;
            GameDataManager.Instance.Save();
        }
        else
        {
            gameData.diamonds += 2;
            GameDataManager.Instance.Save();
        }

        // TODO: Show the victory screen and display the final score and time
        uiManager.WinPanel.SetActive(true);
        foreach (Cell parkingCell in TouchManager.Instance.parkingCells)
        {
            parkingCell.IsOccupide = false;
        }
        TouchManager.Instance.parkingCells.Clear();
        TouchManager.Instance.rowsList.Clear();
        foreach (GameObject go in spawnedCarPrefabs)
        {
            go.GetComponent<Car>().parkingCell = null;
            Destroy(go);
        }
        spawnedCarPrefabs.Clear();
        level[currentLevel].levelEnvironment.SetActive(false);
    }

    public void EndGameDelay()
    {
        TouchManager.Instance.gameOver = true;
        Invoke(nameof(EndGame), 1f);
    }

    public void ResetCarPosition()
    {

    }
}

[Serializable]
public class LevelManager
{
    public int level;
    public int carAmount;
    public Color[] carColor;
    public Transform[] carPosition;
    public GameObject levelEnvironment;
}
