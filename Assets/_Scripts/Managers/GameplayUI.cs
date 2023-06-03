using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class GameplayUI : MonoBehaviour
{
    #region Fields
    public static GameplayUI Instance;
    public GameData gameData;

    [Header("UI Panels")]
    public GameObject levelSelectionPanel;
    public GameObject settingsPanel;
    public GameObject gamePlayPanel;
    public GameObject levelPausedPanel;
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject skipLevelPanel;
    public GameObject buyMovePanel;
    public GameObject[] panelsArray;


    [Header("Start Loading and Loading")]
    public Image startLoadingInner;
    public Image loadingInner;

    [Header("LevelSelection")]
    public GameObject[] levelLocks;

    [Header("Store")]
    public GameObject gemsStats;
    public GameObject IAPStore;
    public GameObject CarStore;
    public GameObject ThemesStore;
    public TextMeshProUGUI gemsAmountText;

    [Header("Settings")]
    public AudioMixer audioMixer;
    public GameObject mainMenuSoundOn;
    public GameObject mainMenuSoundOff;
    public GameObject mainMenuMusicOn;
    public GameObject mainMenuMusicOff;
    public GameObject gamePlaySoundOn;
    public GameObject gamePlaySoundOff;
    public GameObject gamePlayMusicOn;
    public GameObject gamePlayMusicOff;

    [Header("Game Play")]
    public int levelSelected;
    public Image GameplayTheme;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelMoves;
    public GameController gameController;
    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameObject skipAdsButtonInPauseLevel;
    public GameObject skipAdsButtonInFailedLevel;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        Instance = this;
        levelSelected = gameData.unlockedLevel;
        //levelSelected = GameManager.Instance.currentLevel;
    }

    private void Start()
    {
        UpdateGemsText();
        if (gameData.isSoundOn)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
        if (gameData.isMusicOn)
        {
            MusicOn();
        }
        else
        {
            MusicOff();
        }
    }
    #endregion

    #region Settings
    public void SoundOn()
    {
        _ = audioMixer.SetFloat("SFX", -3);
        _ = audioMixer.SetFloat("Ui", -3);
        gamePlaySoundOn.SetActive(true);
        gamePlaySoundOff.SetActive(false);
        gameData.isSoundOn = true;
        DataManager.Instance.SaveData();
    }
    public void SoundOff()
    {
        _ = audioMixer.SetFloat("SFX", -80);
        _ = audioMixer.SetFloat("Ui", -80);
        gamePlaySoundOn.SetActive(false);
        gamePlaySoundOff.SetActive(true);
        gameData.isSoundOn = false;
        DataManager.Instance.SaveData();
    }
    public void MusicOn()
    {
        _ = audioMixer.SetFloat("BGM", -3);
        gamePlayMusicOn.SetActive(true);
        gamePlayMusicOff.SetActive(false);
        gameData.isMusicOn = true;
        DataManager.Instance.SaveData();
    }
    public void MusicOff()
    {
        _ = audioMixer.SetFloat("BGM", -80);
        gamePlayMusicOn.SetActive(false);
        gamePlayMusicOff.SetActive(true);
        gameData.isMusicOn = false;
        DataManager.Instance.SaveData();
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
        if (levelSelected < 9) { levelSelected++; }
        GameController.Instance.StartGame();
        PanelActivate(gamePlayPanel.name);
    }
    #endregion

    #region Skip Level
    public void OnPay50GemsToSkipLevelButtonClicked()
    {
        if (gameData.gems >= 50 && levelSelected < 9 && gameData.unlockedLevel < 9)
        {
            TouchManager.Instance.gameOver = false;
            if (levelSelected < 29) { levelSelected++; }
            GameController.Instance.ClearGame();
            GameController.Instance.StartGame();
            PanelActivate(gamePlayPanel.name);
            gameData.gems -= 50;
            DataManager.Instance.SaveData();
            UpdateGemsText();
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
            TouchManager.Instance.moveCount += 3;
            UpdateMoveText();
            gameData.gems -= 30;
            DataManager.Instance.SaveData();
            UpdateGemsText();
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
        gemsStats.SetActive(panelName.Equals(settingsPanel.name) || panelName.Equals(levelSelectionPanel.name));
        TouchManager.Instance.gameOver = !panelName.Equals(gamePlayPanel.name);
    }

    public void GoToSettings()
    {
        PanelActivate(settingsPanel.name);
    }

    public void LevelFailedDelay()
    {
        Invoke(nameof(LevelFailed), 0.7f);
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

    public void UpdateMoveText()
    {
        levelMoves.text = TouchManager.Instance.moveCount.ToString();
    }

    public void UpdateGemsText()
    {
        gemsAmountText.text = gameData.gems.ToString();
    }

    public void PrivacyButton()
    {
        Debug.Log("Privacy Button Clicked");
    }
    #endregion
}
