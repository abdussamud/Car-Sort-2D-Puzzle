using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public Camera m_Camera;
    public GameObject[] carPrefabs;
    [SerializeField]
    private Level[] levels;
    [HideInInspector]
    public bool isGameOver;
    [HideInInspector]
    public List<GameObject> spawnedCarPrefabs = new();
    private GameObject carPrefab;
    private GameplayUI gui;
    private GameData gd;
    private DataManager dm;
    private TouchManager tm;

    public int CurrentLevel { get; set; }

    private void Awake()
    {
        gc = this;
        dm = DataManager.dm;
    }

    private void Start()
    {
        gd = dm.gameData;
        gui = GameplayUI.gui;
        tm = TouchManager.tm;
        StartGame();
    }

    public void StartGame()
    {
        gui.SetLevelText();
        carPrefab = GameManager.Instance.carPrefabs[gd.carPrefab];
        CurrentLevel = gd.unlockedLevel;
        m_Camera.orthographicSize = levels[CurrentLevel].cameraSize;
        gui.skipAdsButtonInPauseLevel.SetActive(CurrentLevel + 1 < gd.unlockedLevel &&
            gd.unlockedLevel < 9);
        gui.skipAdsButtonInFailedLevel.SetActive(CurrentLevel + 1 < gd.unlockedLevel &&
            gd.unlockedLevel < 9);
        tm.moveCount = 10;
        gui.levelMoves.text = 10.ToString();
        CarInitialization();
    }

    public void CarInitialization()
    {
        levels[CurrentLevel].levelEnvironment.SetActive(true);
        gui.GameplayTheme.sprite = GameManager.Instance.gameplaySceneBG[gd.gameplaySceneBG];
        levels[CurrentLevel].cellsArray.ToList().ForEach(c => tm.parkingCells.Add(c));
        levels[CurrentLevel].rowsArray.ToList().ForEach(r => tm.rowsList.Add(r));
        for (int i = 0; i < levels[CurrentLevel].carPosition.Length; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefabs[levels[CurrentLevel].carCode[i]],
                levels[CurrentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(levels[CurrentLevel].carPosition[i].parent);
            Car car = spawnedPrefab.GetComponent<Car>();
            car.SetParkingCell();
            spawnedCarPrefabs.Add(spawnedPrefab);
        }
    }

    public void EndGame()
    {
        bool shouldIncrementUnlockedLevel = CurrentLevel == gd.unlockedLevel && CurrentLevel < 29;
        gd.unlockedLevel = shouldIncrementUnlockedLevel ? +1 : gd.unlockedLevel;
        gd.diamonds = shouldIncrementUnlockedLevel ? +(CurrentLevel + 1) * 10 : +2;

        SaveData();

        foreach (Cell parkingCell in tm.parkingCells)
        {
            parkingCell.IsOccupide = false;
        }
        tm.parkingCells.Clear();
        tm.rowsList.Clear();
        foreach (GameObject go in spawnedCarPrefabs)
        {
            go.GetComponent<Car>().parkingCell = null;
            Destroy(go);
        }
        spawnedCarPrefabs.Clear();
        levels[CurrentLevel].levelEnvironment.SetActive(false);
        gui.PanelActivate(gui.levelCompletedPanel.name);
    }

    public void ClearGame()
    {
        foreach (Cell parkingCell in tm.parkingCells)
        {
            parkingCell.IsOccupide = false;
        }
        tm.parkingCells.Clear();
        tm.rowsList.Clear();
        foreach (GameObject go in spawnedCarPrefabs)
        {
            go.GetComponent<Car>().parkingCell = null;
            Destroy(go);
        }
        spawnedCarPrefabs.Clear();
        levels[CurrentLevel].levelEnvironment.SetActive(false);
    }

    public void EndGameDelay()
    {
        tm.gameOver = true;
        Invoke(nameof(EndGame), 1f);
    }

    public void ResetCarPosition()
    {
        ClearGame();
        StartGame();
    }

    private void SaveData()
    {
        dm.SaveData();
    }
}
