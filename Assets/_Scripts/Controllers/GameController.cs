using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public int currentLevel;
    public int maxLevels = 19;
    public Camera m_Camera;
    public GameObject[] carPrefabs;
    [SerializeField]
    private Level[] levels;
    private GameData gd;
    private GameManager gm;
    private readonly List<Car> spawnedCars = new();
    private GameplayUI Gui => GameplayUI.gui;
    private TouchManager Tm => TouchManager.tm;

    private void Awake()
    {
        gc = this;
        gd = DataManager.dm.gameData;
        gm = GameManager.Instance;
    }

    private void Start()
    {
        StartGame(GameManager.Instance.level);
    }

    public void StartGame(int level = 0)
    {
        currentLevel = level;
        Gui.levelSelected = currentLevel;
        m_Camera.orthographicSize = levels[currentLevel].cameraSize;
        Gui.gameplayThemeGO[levels[currentLevel].theme].SetActive(true);
        Gui.skipAdsButtonInPauseLevel.SetActive(currentLevel <= gd.level && gd.level < 9);
        Gui.skipAdsButtonInFailedLevel.SetActive(currentLevel <= gd.level && gd.level < 9);
        Tm.moveCount = levels[currentLevel].moveCount;
        Gui.SetLevelText();
        Gui.UpdateMoveText();
        LevelSetter();
    }

    public void LevelSetter()
    {
        levels[currentLevel].levelEnvironment.SetActive(true);
        levels[currentLevel].cellsArray.ToList().ForEach(c => Tm.parkingCells.Add(c));
        levels[currentLevel].rowsArray.ToList().ForEach(r => Tm.rowsList.Add(r));
        for (int i = 0; i < levels[currentLevel].carPosition.Length; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefabs[levels[currentLevel].carCode[i]],
                levels[currentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(levels[currentLevel].levelEnvironment.transform);
            Car car = spawnedPrefab.GetComponent<Car>();
            car.SetParkingCell();
            spawnedCars.Add(car);
        }
    }

    public void EndGame()
    {
        Gui.PanelActivate(Gui.levelCompletedPanel.name);
        gd.allLevel = gd.level == maxLevels;
        bool newLevel = currentLevel == gd.level && currentLevel < maxLevels;
        gd.level += gd.allLevel ? 0 : 1;
        gd.coins += newLevel ? 4 : 1;
        SaveData();
        ClearGame();
        if (!gd.removeAds) { AdsManager.Instance.LevelCompleted(); }
    }

    public void ClearGame()
    {
        foreach (Cell parkingCell in Tm.parkingCells)
        {
            parkingCell.IsOccupied = false;
        }
        foreach (Car car in spawnedCars)
        {
            Destroy(car.gameObject);
        }
        spawnedCars.Clear();
        Tm.rowsList.Clear();
        Tm.parkingCells.Clear();
        levels[currentLevel].levelEnvironment.SetActive(false);
    }

    public void EndGameDelay()
    {
        Gui.winParticalsGO.SetActive(true);
        Invoke(nameof(DisableWinParticals), 2f);
        Invoke(nameof(EndGame), 2f);
    }

    private void SaveData() => DataManager.dm.SaveData();

    private void DisableWinParticals()
    {
        Gui.winParticalsGO.SetActive(false);
    }
}
