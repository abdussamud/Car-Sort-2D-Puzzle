using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public UIManager uiManager;
    public GameObject carPrefab;
    public int difficultyLevel;
    public int score;
    public float timer;
    public bool isGameOver;
    public Canvas canvas;
    public IEnumerator StartTime;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private List<GameObject> spawnedCarPrefabs = new();
    public int carAmount;

    public List<Cell> cells = new();
    public List<Color> randomColor = new();

    private int currentLevel;


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
        score = 100;
        timer = 0;

        CarSetter();
        StartTime = StartTimer();
        StartCoroutine(StartTime);
    }

    public void EndGame()
    {
        if (GameManager.Instance.currentLevel == GameManager.Instance.unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockLevel", GameManager.Instance.currentLevel + 1);
            PlayerPrefs.Save();
        }
        // TODO: Show the victory screen and display the final score and time
        uiManager.WinPanel.SetActive(true);
        TouchManager.Instance.parkingCells.Clear();
        TouchManager.Instance.rowsList.Clear();
        foreach (GameObject go in spawnedCarPrefabs)
        {
            go.GetComponent<Car>().parkingCell = null;
            Destroy(go);
        }
        spawnedCarPrefabs.Clear();
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
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

    #region Car Setter
    public void CarSetter()
    {
        currentLevel = GameManager.Instance.currentLevel;
        int activeLevelGameObject = currentLevel < 10 ? 0 : currentLevel is > 9 and < 20 ? 1 : 2;
        GameManager.Instance.levelGameObjectList[0].SetActive(activeLevelGameObject == 0);
        GameManager.Instance.levelGameObjectList[1].SetActive(activeLevelGameObject == 1);
        GameManager.Instance.levelGameObjectList[2].SetActive(activeLevelGameObject == 2);
        carAmount = currentLevel < 10 ? 8 : currentLevel is > 9 and < 20 ? 11 : 14;
        TouchManager.Instance.SetParkingCell();
        TouchManager.Instance.SetRowList();

        foreach (Cell parkingCell in TouchManager.Instance.parkingCells)
        {
            cells.Add(parkingCell);
            parkingCell.IsOccupide = false;
        }
        for (int i = 0; i < carAmount; i++)
        {
            CarSpawner(i);
        }
        CarColorShuffler();
        //timerText.text = string.Format("{0:D2}:{1:D2}", 00, 00);
    }

    private void CarSpawner(int carNumber)
    {
        int randomIndex = UnityEngine.Random.Range(0, cells.Count);
        Transform spawnPoint = cells[randomIndex].transform;
        GameObject spawnedPrefab = Instantiate(carPrefab, spawnPoint.position, Quaternion.identity);
        spawnedPrefab.transform.SetParent(cells[randomIndex].transform.parent);
        spawnedCarPrefabs.Add(spawnedPrefab);
        cells.RemoveAt(randomIndex);
        if (carNumber == carAmount - 1) { cells.Clear(); }
    }

    private void CarColorShuffler()
    {
        List<Color> randomColor = new();
        if (currentLevel < 5)
        {
            for (int i = 0; i < 3; i++)
            {
                randomColor.Add(new(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 0.56f), UnityEngine.Random.Range(0.6f, 1f)));
            }
        }
        else if (currentLevel >= 5)
        {
            for (int i = 0; i < 3; i++)
            {
                randomColor.Add(new(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
            }
        }
        for (int carNumber = 0; carNumber < carAmount; carNumber++)
        {
            Car car = spawnedCarPrefabs[carNumber].GetComponent<Car>();

            if (carAmount == 8)
            {
                if (carNumber is 0 or 1 or 2) { car.carColor = randomColor[0]; }
                if (carNumber is 3 or 4 or 5) { car.carColor = randomColor[1]; }
                if (carNumber is 6 or 7) { car.carColor = randomColor[2]; }
            }
            else if (carAmount == 11)
            {
                if (carNumber is 0 or 1 or 2 or 3) { car.carColor = randomColor[0]; }
                if (carNumber is 4 or 5 or 6 or 7) { car.carColor = randomColor[1]; }
                if (carNumber is 8 or 9 or 10) { car.carColor = randomColor[2]; }
            }
            else if (carAmount == 14)
            {
                if (carNumber is 0 or 1 or 2 or 3 or 4) { car.carColor = randomColor[0]; }
                if (carNumber is 5 or 6 or 7 or 8 or 9) { car.carColor = randomColor[1]; }
                if (carNumber is 10 or 11 or 12 or 13) { car.carColor = randomColor[2]; }
            }
            car.SetCarColor();
            car.SetParkingCell();
        }
        List<Row> rowList = TouchManager.Instance.rowsList;
        if (rowList[0].IsCarColorSameInRow() && rowList[1].IsCarColorSameInRow() && rowList[2].IsCarColorSameInRow())
        {
            CarColorShuffler();
        }
    }
    #endregion

    public void EndGameDelay()
    {
        StopCoroutine(StartTime);
        Invoke(nameof(EndGame), 0.5f);
    }

    public void ResetCarPosition()
    {

    }
}
