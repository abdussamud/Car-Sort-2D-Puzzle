using System;
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
    public GameObject notEnoughCoinsPanel;
    public GameObject[] panelsArray;

    [Header("Game Play")]
    public int levelSelected;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelMoves;
    public GameObject winParticalsGO;
    public GameObject buyMoveLoaderGO;
    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameObject skipAdsButtonInPauseLevel;
    public GameObject skipAdsButtonInFailedLevel;
    public GameObject[] gameplayThemeGO;

    [Header("Ads")]
    public int skipLevelOrBuyMoves;

    private GameData gd;
    private GameController Gc => GameController.gc;
    private TouchManager Tm => TouchManager.tm;
    private GameManager Gm => GameManager.Instance;
    #endregion


    #region Unity Methods
    private void Awake()
    {
        gui = this;
        gd = DataManager.dm.gameData;
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
        Gc.ClearGame();
        Gc.StartGame();
        PanelActivate(gamePlayPanel.name);
    }

    public void OnPauseButtonClicked()
    {
        PanelActivate(levelPausedPanel.name);
    }

    public void OnBuyMoveButtonClicked()
    {
        Tm.gameOver = true;
        buyMovePanel.SetActive(true);
        buyMoveLoaderGO.SetActive(true);
        Invoke(nameof(DisableBuyMovesLoader), 1.5f);
    }

    public void OnResumeButtonClicked()
    {
        Tm.gameOver = false;
        PanelActivate(gamePlayPanel.name);
    }

    public void OnSkipLevelButtonClicked()
    {
        skipLevelPanel.SetActive(true);
    }

    public void OnReplayButtonClicked()
    {
        Tm.gameOver = false;
        PanelActivate(gamePlayPanel.name);
        Gc.StartGame(Gm.level);
    }

    public void OnNextLevelButtonClicked()
    {
        Tm.gameOver = false;
        Gm.level++;
        Gc.StartGame(Gm.level);
        PanelActivate(gamePlayPanel.name);
    }

    public void OnWatchAdsForSkipLevelButtonClick()
    {
        skipLevelOrBuyMoves = 0;
        AdsManager.Instance.ShowRewardedVideoUnity();
        //BuySkipLevelAdsCompleted();
    }

    public void AdsWatchedCompleted()
    {
        (skipLevelOrBuyMoves == 0 ? (Action)BuySkipLevelAdsCompleted : BuyMovesAdsCompleted)();
    }

    public void BuySkipLevelAdsCompleted()
    {
        Tm.gameOver = false;
        gd.level++;
        SaveData();
        Gm.level++;
        Gc.ClearGame();
        Gc.StartGame(Gm.level);
        PanelActivate(gamePlayPanel.name);
        SetLevelText();
    }
    public void OnPay50GemsToSkipLevelButtonClicked()
    {
        if (gd.coins >= 15 && levelSelected < Gc.maxLevels && gd.level < Gc.maxLevels)
        {
            Tm.gameOver = false;
            gd.level++;
            gd.coins -= 15;
            SaveData();
            Gm.level++;
            Gc.ClearGame();
            Gc.StartGame(Gm.level);
            PanelActivate(gamePlayPanel.name);
            SetLevelText();
        }
        else if (gd.coins < 15 && levelSelected < Gc.maxLevels && gd.level < Gc.maxLevels)
        {
            notEnoughCoinsPanel.SetActive(true);
            Invoke(nameof(DisableNotEnoughPanel), 2f);
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
        skipLevelOrBuyMoves = 1;
        AdsManager.Instance.ShowRewardedVideoUnity();
        //BuyMovesAdsCompleted();
    }

    public void BuyMovesAdsCompleted()
    {
        buyMovePanel.SetActive(false);
        Tm.moveCount += 3;
        Tm.gameOver = false;
        UpdateMoveText();
    }

    public void OnPay30GemsToBuy3Moves()
    {
        if (gd.coins >= 5)
        {
            buyMovePanel.SetActive(false);
            Tm.gameOver = false;
            Tm.moveCount += 3;
            UpdateMoveText();
            gd.coins -= 5;
            SaveData();
        }
        else
        {
            notEnoughCoinsPanel.SetActive(true);
            Invoke(nameof(DisableNotEnoughPanel), 2f);
        }
    }

    public void OnNoThanksOfBuyMovesButtonClicked()
    {
        buyMovePanel.SetActive(false);
        Tm.gameOver = Tm.moveCount <= 0;
        if (Tm.moveCount <= 0) { LevelFailed(); }
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
        Gc.ClearGame();
        if (!gd.removeAds) { AdsManager.Instance.LevelCompleted(); }
    }

    public void SetLevelText()
    {
        levelText.text = "LEVEL  " + (1 + Gm.level).ToString();
    }

    public void UpdateMoveText()
    {
        levelMoves.text = Tm.moveCount.ToString();
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

    public void PlaySound(string soundName)
    {
        AudioManager.am.Play(soundName);
    }

    public void ActivateBuyMoves()
    {
        buyMovePanel.SetActive(true);
        buyMoveLoaderGO.SetActive(true);
        Invoke(nameof(DisableBuyMovesLoader), 1.5f);
    }
    #endregion

    #region Private Methods
    private void DisableNotEnoughPanel()
    {
        notEnoughCoinsPanel.SetActive(false);
    }

    private void SaveData() { DataManager.dm.SaveData(); }

    private void DisableBuyMovesLoader()
    {
        buyMoveLoaderGO.SetActive(false);
    }
    #endregion
}
