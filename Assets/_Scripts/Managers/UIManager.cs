using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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
    public GameObject gamePlaySettingsPanel;
    public GameObject storePanel;
    public GameObject exitGamePanel;
    public GameObject rateUsPanel;
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
    private void Awake() { Instance = this; }

    private void Start()
    {
        _ = StartCoroutine(StartLoading());
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

    #region Store
    public void OnStoreSelectButtonClicked(GameObject storeType)
    {
        IAPStore.SetActive(storeType == IAPStore);
        CarStore.SetActive(storeType == CarStore);
        ThemesStore.SetActive(storeType == ThemesStore);
    }

    public void OnIAPStoreButtonClicked()
    {
        IAPStore.SetActive(true);
        CarStore.SetActive(false);
        ThemesStore.SetActive(false);
    }

    public void OnCarStoreButtonClicked()
    {
        CarStore.SetActive(true);
        IAPStore.SetActive(false);
        ThemesStore.SetActive(false);
    }

    public void OnThemesStoreButtonClicked()
    {
        ThemesStore.SetActive(true);
        IAPStore.SetActive(false);
        CarStore.SetActive(false);
    }
    #endregion

    #region Settings
    public void SoundOn()
    {
        _ = audioMixer.SetFloat("SFX", -3);
        _ = audioMixer.SetFloat("Ui", -3);
        mainMenuSoundOn.SetActive(true);
        mainMenuSoundOff.SetActive(false);
        gamePlaySoundOn.SetActive(true);
        gamePlaySoundOff.SetActive(false);
        gameData.isSoundOn = true;
        DataManager.Instance.SaveData();
    }
    public void SoundOff()
    {
        _ = audioMixer.SetFloat("SFX", -80);
        _ = audioMixer.SetFloat("Ui", -80);
        mainMenuSoundOn.SetActive(false);
        mainMenuSoundOff.SetActive(true);
        gamePlaySoundOn.SetActive(false);
        gamePlaySoundOff.SetActive(true);
        gameData.isSoundOn = false;
        DataManager.Instance.SaveData();
    }
    public void MusicOn()
    {
        _ = audioMixer.SetFloat("BGM", -3);
        mainMenuMusicOn.SetActive(true);
        mainMenuMusicOff.SetActive(false);
        gamePlayMusicOn.SetActive(true);
        gamePlayMusicOff.SetActive(false);
        gameData.isMusicOn = true;
        DataManager.Instance.SaveData();
    }
    public void MusicOff()
    {
        _ = audioMixer.SetFloat("BGM", -80);
        mainMenuMusicOn.SetActive(false);
        mainMenuMusicOff.SetActive(true);
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

    public void OnGamePlaySettingsButtonClicked()
    {
        PanelActivate(gamePlaySettingsPanel.name);
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
        if (levelSelected < 9) { levelSelected++; }
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
        if (levelSelected < 9 && gameData.unlockedLevel < 9)
        {
            TouchManager.Instance.gameOver = false;
            levelSelected++;
            GameController.Instance.ClearGame();
            GameController.Instance.StartGame();
            PanelActivate(gamePlayPanel.name);
        }
    }

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
    public void OnWatchVideoOfBuyMoveButtonClicked()
    {
        // Run This Function if the player watched the ad video
        ADsVideoOfBuyMoveWatchedCompletley();
    }

    private void ADsVideoOfBuyMoveWatchedCompletley()
    {
        buyMovePanel.SetActive(false);
        TouchManager.Instance.moveCount += 3;
        UpdateMoveText();
    }

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
        gemsStats.SetActive(panelName.Equals(mainMenuPanel.name) || panelName.Equals(storePanel.name) ||
            panelName.Equals(settingsPanel.name) || panelName.Equals(levelSelectionPanel.name));
        TouchManager.Instance.gameOver = !panelName.Equals(gamePlayPanel.name);
    }

    public void GoHome()
    {
        PanelActivate(mainMenuPanel.name);
    }

    public void GoToStore()
    {
        PanelActivate(storePanel.name);
    }

    public void GoToSettings()
    {
        PanelActivate(settingsPanel.name);
    }

    public void GamePlaySettingsExit()
    {
        PanelActivate(gamePlaySettingsPanel.name);
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
