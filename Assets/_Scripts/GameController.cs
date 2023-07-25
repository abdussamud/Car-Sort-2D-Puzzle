using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private GameObject carPrefab;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private GameplayUI gameplayUI;
    [SerializeField]
    private Level[] levels;
    [HideInInspector]
    public bool isGameOver;
    [HideInInspector]
    public List<GameObject> spawnedCarPrefabs = new();


    public int CurrentLevel { get; set; }

    private void Awake() { Instance = this; }

    private void Start() { StartGame(); }

    public void StartGame()
    {
        gameplayUI.SetLevelText();
        carPrefab = GameManager.Instance.carPrefabs[gameData.carPrefab];
        CurrentLevel = gameData.unlockedLevel;
        gameplayUI.skipAdsButtonInPauseLevel.SetActive(CurrentLevel + 1 < gameData.unlockedLevel &&
            gameData.unlockedLevel < 9);
        gameplayUI.skipAdsButtonInFailedLevel.SetActive(CurrentLevel + 1 < gameData.unlockedLevel &&
            gameData.unlockedLevel < 9);
        TouchManager.Instance.moveCount = 10;
        gameplayUI.levelMoves.text = 10.ToString();
        CarInitialization();
    }

    public void CarInitialization()
    {
        levels[CurrentLevel].levelEnvironment.SetActive(true);
        gameplayUI.GameplayTheme.sprite = GameManager.Instance.gameplaySceneBG[gameData.gameplaySceneBG];
        levels[CurrentLevel].cellsArray.ToList().ForEach(c => TouchManager.Instance.parkingCells.Add(c));
        levels[CurrentLevel].rowsArray.ToList().ForEach(r => TouchManager.Instance.rowsList.Add(r));
        //TouchManager.Instance.SetParkingCell();
        //TouchManager.Instance.SetRowList();
        for (int i = 0; i < levels[CurrentLevel].carInfos.Length; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefab, levels[CurrentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(levels[CurrentLevel].carPosition[i].parent);
            spawnedPrefab.GetComponent<Car>().CarColor = levels[CurrentLevel].carInfos[i].carColor;
            spawnedPrefab.GetComponent<Car>().SetParkingCell();
            spawnedCarPrefabs.Add(spawnedPrefab);
        }
    }

    public void EndGame()
    {
        if (CurrentLevel == gameData.unlockedLevel && CurrentLevel < 29)
        {
            gameData.unlockedLevel++;
            gameData.diamonds += (CurrentLevel + 1) * 10;
            DataManager.Instance.SaveData();
            gameplayUI.UpdateGemsText();
        }
        else
        {
            gameData.diamonds += 2;
            DataManager.Instance.SaveData();
            gameplayUI.UpdateGemsText();
        }

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
        levels[CurrentLevel].levelEnvironment.SetActive(false);
        gameplayUI.PanelActivate(gameplayUI.levelCompletedPanel.name);
    }

    public void ClearGame()
    {
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
        levels[CurrentLevel].levelEnvironment.SetActive(false);
    }

    public void EndGameDelay()
    {
        TouchManager.Instance.gameOver = true;
        Invoke(nameof(EndGame), 1f);
    }

    public void ResetCarPosition()
    {
        ClearGame();
        StartGame();
    }
}
