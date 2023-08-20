using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    #region Fields
    public static GameplayUI gui;

    [Header("UI Panels")]
    public GameObject gamePlayPanel;
    public GameObject levelPausedPanel;
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject skipLevelPanel;
    public GameObject buyMovePanel;
    public GameObject[] panelsArray;

    [Header("Game Play")]
    public int levelSelected;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelMoves;
    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameObject skipAdsButtonInPauseLevel;
    public GameObject skipAdsButtonInFailedLevel;
    public GameObject[] gameplayThemeGO;

    private GameController gc;
    private TouchManager tm;
    private DataManager dm;
    private GameData gameData;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        gui = this;
        dm = DataManager.dm;
        gameData = dm.gameData;
        //levelSelected = gameData.unlockedLevel;
        //levelSelected = GameManager.Instance.currentLevel;
    }

    private void Start()
    {
        tm = TouchManager.tm;
        gc = GameController.gc;
    }
    #endregion

    #region Button Call Back
    public void OnHomeButtonClick()
    {
        GameManager.Instance.nextScene = "Main Menu";
        SceneManager.LoadScene("Loading");
    }

    public void OnGamePlaySettingsExitsButtonClicked()
    {
        PanelActivate(gamePlayPanel.name);
    }

    public void OnRetryButtonCliked()
    {
        gc.ClearGame();
        gc.StartGame();
    }

    public void OnPauseButtonClicked()
    {
        PanelActivate(levelPausedPanel.name);
    }

    public void OnBuyMoveButtonClicked()
    {
        tm.gameOver = true;
        buyMovePanel.SetActive(true);
    }

    public void OnResumeButtonClicked()
    {
        tm.gameOver = false;
        PanelActivate(gamePlayPanel.name);
    }

    public void OnSkipLevelButtonClicked()
    {
        skipLevelPanel.SetActive(true);
    }

    public void OnReplayButtonClicked()
    {
        tm.gameOver = false;
        PanelActivate(gamePlayPanel.name);
        gc.StartGame();
    }

    public void OnNextLevelButtonClicked()
    {
        tm.gameOver = false;
        if (levelSelected < 9) { levelSelected++; }
        gc.StartGame();
        PanelActivate(gamePlayPanel.name);
    }

    public void OnWatchAdsForSkipLevelButtonClick()
    {
        BuySkipLevelAdsCompleted();
    }

    public void BuySkipLevelAdsCompleted()
    {
        tm.gameOver = false;
        if (levelSelected < 9) { levelSelected++; }
        gc.ClearGame();
        gc.StartGame();
        PanelActivate(gamePlayPanel.name);
        SetLevelText();
    }
    public void OnPay50GemsToSkipLevelButtonClicked()
    {
        if (gameData.coins >= 50 && levelSelected < 9 && gameData.level < 9)
        {
            tm.gameOver = false;
            if (levelSelected < 9) { levelSelected++; }
            gc.ClearGame();
            gc.StartGame();
            PanelActivate(gamePlayPanel.name);
            gameData.coins -= 50;
            SaveData();
            SetLevelText();
        }
        else if (gameData.coins < 50 && levelSelected < 9 && gameData.level < 9)
        {
            Debug.Log("Less Gems ");
        }
        else
        {
            Debug.Log("All Level unlocked! ");
        }
    }

    public void OnNoThanksOfSkipLevelButtonClicked()
    {
        skipLevelPanel.SetActive(false);
    }

    public void OnWatchAdsForMoveButtonClick()
    {
        BuyMovesAdsCompleted();
    }

    public void BuyMovesAdsCompleted()
    {
        buyMovePanel.SetActive(false);
        tm.moveCount += 3;
        tm.gameOver = false;
        UpdateMoveText();
    }

    public void OnPay30GemsToBuy3Moves()
    {
        if (gameData.coins >= 5)
        {
            buyMovePanel.SetActive(false);
            tm.gameOver = false;
            tm.moveCount += 3;
            UpdateMoveText();
            gameData.coins -= 5;
            SaveData();
        }
        else
        {
            Debug.Log("Dont have enough money!");
        }
    }

    public void OnNoThanksOfBuyMovesButtonClicked()
    {
        buyMovePanel.SetActive(false);
        tm.gameOver = tm.moveCount <= 0;
        if (tm.moveCount <= 0) { LevelFailed(); }
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

    public void LevelFailedDelay()
    {
        Invoke(nameof(LevelFailed), 0.7f);
    }

    public void LevelFailed()
    {
        PanelActivate(levelFailedPanel.name);
        gc.ClearGame();
    }

    public void SetLevelText()
    {
        levelText.text = "LEVEL  " + (1 + levelSelected).ToString();
    }

    public void UpdateMoveText()
    {
        levelMoves.text = tm.moveCount.ToString();
    }

    public void PrivacyButton()
    {
        Debug.Log("Privacy Button Clicked");
    }

    public void EnterToPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ExitFromPanel(GameObject panel)
    {
        panel.SetActive(false);
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

    #endregion

    #region Private Methods
    private void SaveData() { dm.SaveData(); }
    #endregion
}
