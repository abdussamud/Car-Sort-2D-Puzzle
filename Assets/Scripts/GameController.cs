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
    private int difficultyLevel;
    [SerializeField]
    private int score;
    [SerializeField]
    private float timer;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private IEnumerator StartTime;

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
        UIManager.Instance.SetLevelText(currentLevel);
    }

    public void StartGame()
    {
        score = 100;
        timer = 0;

        currentLevel = GameManager.Instance.currentLevel;
        CarInstantiate();
        StartTime = StartTimer();
        StartCoroutine(StartTime);
    }

    public void CarInstantiate()
    {
        Debug.Log("Current Level" + currentLevel);
        level[currentLevel].levelEnvironment.SetActive(true);
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

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer++;
            if (score >= difficultyLevel) UpdateScore(-difficultyLevel);
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    public void EndGame()
    {
        if (GameManager.Instance.currentLevel == gameData.unlockedLevel)
        {
            gameData.unlockedLevel++;
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

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void EndGameDelay()
    {
        StopCoroutine(StartTime);
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
