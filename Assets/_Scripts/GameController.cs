using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private GameObject carPrefab;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private Level[] levels;
    [HideInInspector]
    public bool isGameOver;
    [HideInInspector]
    public List<GameObject> spawnedCarPrefabs = new();


    public int CurrentLevel { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //private void Start() { StartGame(); }

    public void StartGame()
    {
        uiManager.SetLevelText();
        carPrefab = GameManager.Instance.carPrefabs[gameData.carPrefab];
        CurrentLevel = uiManager.levelSelected;
        uiManager.skipAdsButtonInPauseLevel.SetActive(CurrentLevel + 1 < gameData.unlockedLevel &&
            gameData.unlockedLevel < 9);
        uiManager.skipAdsButtonInFailedLevel.SetActive(CurrentLevel + 1 < gameData.unlockedLevel &&
            gameData.unlockedLevel < 9);
        TouchManager.Instance.moveCount = levels[CurrentLevel].levelMoves;
        uiManager.levelMoves.text = levels[CurrentLevel].levelMoves.ToString();
        CarInstantiate();
    }

    public void CarInstantiate()
    {
        levels[CurrentLevel].levelEnvironment.SetActive(true);
        uiManager.GameplayTheme.sprite = GameManager.Instance.gameplaySceneBG[gameData.gameplaySceneBG];
        TouchManager.Instance.SetParkingCell();
        TouchManager.Instance.SetRowList();
        for (int i = 0; i < levels[CurrentLevel].carAmount; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefab, levels[CurrentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(levels[CurrentLevel].carPosition[i].parent);
            spawnedPrefab.GetComponent<Car>().CarColor = levels[CurrentLevel].carColor[i];
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
            uiManager.UpdateGemsText();
        }
        else
        {
            gameData.diamonds += 2;
            DataManager.Instance.SaveData();
            uiManager.UpdateGemsText();
        }

        // TODO: Show the victory screen and display the final score and time
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
        uiManager.PanelActivate(uiManager.levelCompletedPanel.name);
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
