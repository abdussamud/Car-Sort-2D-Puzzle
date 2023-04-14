using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region Fields
    public static UIManager Instance;
    public GameData gameData;

    [Header("UI Panels")]
    public GameObject startLoadingPanel;
    public GameObject loadingPanel;
    public GameObject mainMenuPanel;
    public GameObject levelSelectionPanel;
    public GameObject settingsPanel;
    public GameObject storePanel;
    public GameObject diamondStatsImage;
    public GameObject exitGamePanel;
    public GameObject rateUsPanel;
    public GameObject skipLevelPanel;
    public GameObject buyMovePanel;
    public GameObject gamePlayPanel;
    public GameObject levelPausedPanel;
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject[] panelsArray;


    [Header("Start Loading and Loading")]
    public Image startLoadingInner;
    public Image loadingInner;
    [Header("LevelSelection")]
    public GameObject[] levelLocks;
    [Header("Settings Panel")]
    public GameObject settingExitForMain;
    public GameObject settingExitForGamePlay;
    [Header("Game Play")]
    public TextMeshProUGUI levelMoves;

    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameController gameController;
    public CarSpawner carSpawner;
    public Text scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public int levelSelected;
    #endregion


    #region Unity Methods
    private void Awake() { Instance = this; }

    private void Start()
    {
        //_ = StartCoroutine(StartLoading());
    }
    #endregion

    #region Start Loading
    private IEnumerator StartLoading()
    {
        PanelActivate(startLoadingPanel.name);
        startLoadingInner.DOFillAmount(1, 9);
        yield return new WaitForSecondsRealtime(9f);
        GoHome();
    }
    #endregion

    #region Loading
    private IEnumerator Loading(GameObject _goto)
    {
        ThingsToEnable_or_Disables(_goto);
        PanelActivate(loadingPanel.name);
        loadingInner.DOFillAmount(1, 9);
        yield return new WaitForSecondsRealtime(9f);
        PanelActivate(_goto.name);
        loadingInner.fillAmount = 0;
    }

    private void ThingsToEnable_or_Disables(GameObject Panel)
    {
        if (Panel == levelSelectionPanel)
        {
            for (int i = 0; i <= gameData.unlockedLevel; i++)
            {
                levelLocks[i].SetActive(false);
            }
        }
        else if (Panel == gamePlayPanel)
        {
            GameController.Instance.StartGame();
        }
    }
    #endregion

    #region Main Menu
    public void OnGameExitButtonClicked()
    {
        PanelActivate(exitGamePanel.name);
    }

    public void OnStartButtonClicked()
    {
        _ = StartCoroutine(Loading(levelSelectionPanel));
    }

    public void OnRateUsButtonClicked()
    {
        PanelActivate(rateUsPanel.name);
    }

    public void OnMoreGamesButtonClicked()
    {

    }

    public void OnRemoveAdsButtonClicked()
    {

    }
    #endregion

    #region Level Selection
    public void OnLevelButtonClicked(int level)
    {
        if (level <= gameData.unlockedLevel)
        {
            levelSelected = level;
            _ = StartCoroutine(Loading(gamePlayPanel));
        }
    }
    #endregion

    #region Game Play
    public void SetLevelText()
    {
        levelText.text = "LEVEL  " + (1 + levelSelected).ToString();
    }

    public void WrongMoveTextPrompter()
    {
        GameObject wrongMoveText = Instantiate(wrongMoveTextPrompt, wrongMoveTextPromptParent.transform);

        if (wrongMoveTextPromptParent.transform.childCount > 3)
        {
            Destroy(wrongMoveTextPromptParent.transform.GetChild(3).gameObject);
        }
        if (wrongMoveText != null) { Destroy(wrongMoveText, 5f); }
    }

    public void OnRetryButtonCliked()
    {
        GameController.Instance.ResetCarPosition();
    }

    public void OnPauseButtonClicked()
    {
        PanelActivate(levelPausedPanel.name);
    }

    public void OnBuyMoveButtonClicked()
    {
        buyMovePanel.SetActive(true);
    }
    #endregion

    #region Level Pause
    public void OnResumeButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        PanelActivate(gamePlayPanel.name);
    }

    public void OnHomeFromLevelPauseButtonClicked()
    {
        GameController.Instance.ClearGame();
        PanelActivate(mainMenuPanel.name);
    }

    public void OnSkipLevelButtonClicked()
    {
        skipLevelPanel.SetActive(true);
    }
    #endregion
    
    #region Level Complete
    public void OnReplayButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        PanelActivate(gamePlayPanel.name);
        GameController.Instance.StartGame();
    }

    public void OnNextLevelButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        if (levelSelected < 29) { levelSelected++; }
        GameController.Instance.StartGame();
        PanelActivate(gamePlayPanel.name);
    }
    #endregion

    #region Skip Level
    public void OnWatchVideoOfSkipLevelButtonClicked()
    {
        // Run This Function if the player watched the ad video
        ADsVideoOfSkipLevelWatchedCompletley();
    }

    private void ADsVideoOfSkipLevelWatchedCompletley()
    {
        TouchManager.Instance.gameOver = false;
        if (levelSelected < 29) { levelSelected++; }
        GameController.Instance.StartGame();
        PanelActivate(gamePlayPanel.name);
    }

    public void OnPay50GemsToSkipLevelButtonClicked()
    {
        if (gameData.gems >= 50)
        {
            TouchManager.Instance.gameOver = false;
            if (levelSelected < 29) { levelSelected++; }
            GameController.Instance.StartGame();
            PanelActivate(gamePlayPanel.name);
            gameData.gems -= 50;
            GameDataManager.Instance.Save();
        }
        else
        {
            Debug.Log("Less Gems");
        }
    }
    
    public void OnNoThanksOfBuyMoveButtonClicked()
    {
        skipLevelPanel.SetActive(false);
    }
    #endregion

    #region Buy Move
    public void OnWatchVideoOfBuyMoveButtonClicked()
    {
        // Run This Function if the player watched the ad video
        ADsVideoOfBuyMoveWatchedCompletley();
    }

    private void ADsVideoOfBuyMoveWatchedCompletley()
    {
        buyMovePanel.SetActive(false);
        TouchManager.Instance.moveCount += 3;
    }

    public void OnPay30GemsToBuy3Moves()
    {
        if (gameData.gems >= 30)
        {
            buyMovePanel.SetActive(false);
            TouchManager.Instance.moveCount += 3;
            gameData.gems -= 30;
            GameDataManager.Instance.Save();
        }
    }
    
    public void OnNoThanksOfLevelSkipButtonClicked()
    {
        buyMovePanel.SetActive(false);
    }
    #endregion

    #region Public Methods
    public void PanelActivate(string panelName)
    {
        foreach (GameObject panel in panelsArray)
        {
            panel.SetActive(panelName.Equals(panel.name));
        }
    }

    public void GoHome()
    {
        PanelActivate(mainMenuPanel.name);
    }

    public void GoToStore()
    {
        PanelActivate(storePanel.name);
    }

    public void GoToSettings(bool formMainMenu)
    {
        PanelActivate(settingsPanel.name);
        settingExitForMain.SetActive(formMainMenu);
        settingExitForGamePlay.SetActive(!formMainMenu);
    }

    public void LevelFailed()
    {
        PanelActivate(levelFailedPanel.name);
        GameController.Instance.ClearGame();
    }

    public void GameExitYes()
    {
        Application.Quit();
    }
    #endregion
}
