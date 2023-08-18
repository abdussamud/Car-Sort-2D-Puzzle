using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Image GameplayTheme;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelMoves;
    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameObject skipAdsButtonInPauseLevel;
    public GameObject skipAdsButtonInFailedLevel;

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
        levelSelected = gameData.unlockedLevel;
        //levelSelected = GameManager.Instance.currentLevel;
    }

    private void Start()
    {
        tm = TouchManager.tm;
        gc = GameController.gc;
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

    public void OnGamePlaySettingsExitsButtonClicked()
    {
        PanelActivate(gamePlayPanel.name);
    }

    public void OnRetryButtonCliked()
    {
        gc.ResetCarPosition();
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
        tm.gameOver = false;
        PanelActivate(gamePlayPanel.name);
    }

    public void OnSkipLevelButtonClicked()
    {
        skipLevelPanel.SetActive(true);
    }
    #endregion

    #region Level Complete
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
    #endregion

    #region Skip Level
    public void OnPay50GemsToSkipLevelButtonClicked()
    {
        if (gameData.gems >= 50 && levelSelected < 9 && gameData.unlockedLevel < 9)
        {
            tm.gameOver = false;
            if (levelSelected < 29) { levelSelected++; }
            gc.ClearGame();
            gc.StartGame();
            PanelActivate(gamePlayPanel.name);
            gameData.gems -= 50;
            SaveData();
        }
        else if (gameData.gems < 50 && levelSelected < 9 && gameData.unlockedLevel < 9)
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
    #endregion

    #region Buy Move
    public void OnPay30GemsToBuy3Moves()
    {
        if (gameData.gems >= 30)
        {
            buyMovePanel.SetActive(false);
            tm.moveCount += 3;
            UpdateMoveText();
            gameData.gems -= 30;
            SaveData();
        }
    }

    public void OnNoThanksOfBuyMovesButtonClicked()
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

    public void LevelFailedDelay()
    {
        Invoke(nameof(LevelFailed), 0.7f);
    }

    public void LevelFailed()
    {
        PanelActivate(levelFailedPanel.name);
        gc.ClearGame();
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
    #endregion

    private void SaveData()
    {
        dm.SaveData();
    }
}
