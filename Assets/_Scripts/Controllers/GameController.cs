using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public int currentLevel;
    public Camera m_Camera;
    public GameObject[] carPrefabs;
    [SerializeField]
    private Level[] levels;
    private GameData gd;
    private GameplayUI gui;
    private DataManager dm;
    private TouchManager tm;
    private readonly List<Car> spawnedCars = new();
    public int CurrentLevel { get; set; }

    private void Awake()
    {
        gc = this;
        dm = DataManager.dm;
        gd = dm.gameData;
    }

    private void Start()
    {
        gui = GameplayUI.gui;
        tm = TouchManager.tm;
        StartGame();
    }

    public void StartGame()
    {
        CurrentLevel = currentLevel;// gd.unlockedLevel;
        gui.levelSelected = currentLevel;
        m_Camera.orthographicSize = levels[CurrentLevel].cameraSize;
        gui.gameplayThemeGO[levels[CurrentLevel].theme].SetActive(true);
        gui.skipAdsButtonInPauseLevel.SetActive(CurrentLevel + 1 < gd.level && gd.level < 9);
        gui.skipAdsButtonInFailedLevel.SetActive(CurrentLevel + 1 < gd.level && gd.level < 9);
        tm.moveCount = levels[CurrentLevel].moveCount;
        gui.SetLevelText();
        gui.UpdateMoveText();
        LevelSetter();
    }

    public void LevelSetter()
    {
        levels[CurrentLevel].levelEnvironment.SetActive(true);
        levels[CurrentLevel].cellsArray.ToList().ForEach(c => tm.parkingCells.Add(c));
        levels[CurrentLevel].rowsArray.ToList().ForEach(r => tm.rowsList.Add(r));
        for (int i = 0; i < levels[CurrentLevel].carPosition.Length; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefabs[levels[CurrentLevel].carCode[i]],
                levels[CurrentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(levels[CurrentLevel].levelEnvironment.transform);
            Car car = spawnedPrefab.GetComponent<Car>();
            car.SetParkingCell();
            spawnedCars.Add(car);
        }
    }

    public void EndGame()
    {
        gui.PanelActivate(gui.levelCompletedPanel.name);
        bool newLevel = CurrentLevel == gd.level && CurrentLevel < 10;
        gd.level += newLevel ? 1 : 0;
        gd.coins += newLevel ? 4 : 1;
        SaveData();
        ClearGame();
    }

    public void ClearGame()
    {
        foreach (Cell parkingCell in tm.parkingCells)
        {
            parkingCell.IsOccupied = false;
        }
        foreach (Car car in spawnedCars)
        {
            Destroy(car.gameObject);
        }
        spawnedCars.Clear();
        tm.rowsList.Clear();
        tm.parkingCells.Clear();
        levels[CurrentLevel].levelEnvironment.SetActive(false);
    }

    public void EndGameDelay() { Invoke(nameof(EndGame), 1f); }

    private void SaveData() { dm.SaveData(); }
}
