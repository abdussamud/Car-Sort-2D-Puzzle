using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake() => Instance = this;
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
    public GameObject gamePlayPanel;
    public GameObject levelPausedPanel;
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject[] panelsArray;

    [Header("Start Loading")]
    public Image startLoadingInner;
    [Header("Loading")]
    public Image loadingInner;
    [Header("Main Menu")]

    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameController gameController;
    public CarSpawner carSpawner;
    public Text scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public Button hintButton;


    private void Start()
    {
        PanelActivate(startLoadingPanel.name);
        StartCoroutine(StartLoading());
    }


    // Start Loading
    #region Start Loading
    private IEnumerator StartLoading()
    {
        startLoadingInner.DOFillAmount(1, 9);
        yield return new WaitForSecondsRealtime(9f);
        PanelActivate(mainMenuPanel.name);
    }
    #endregion

    // Loading
    #region Loading
    public void LoadingComplete()
    {

    }
    #endregion

    // Main Menu Button Functions
    #region Main Menu
    public void OnGameExitButtonClicked()
    {
        PanelActivate(exitGamePanel.name);
    }

    public void OnStoreButtonClicked()
    {
        PanelActivate(storePanel.name);
    }

    public void OnSettingsButtonClicked()
    {
        PanelActivate(settingsPanel.name);
    }

    public void OnStartButtonClicked()
    {
        PanelActivate(levelSelectionPanel.name);
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

    // Level Selection
    #region Level Selection
    public void OnLevelSelectionExitButtonClicked()
    {
        PanelActivate(mainMenuPanel.name);
    }

    public void OnLevelButtonClicked(int level)
    {
        if (level <= gameData.unlockedLevel)
        {
            PanelActivate(loadingPanel.name);
        }
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

    public void OnGoHomeButtonClicked()
    {
        PanelActivate(mainMenuPanel.name);
    }
    #endregion



    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateTimerUI(float time)
    {
        timerText.text = "Time: " + time.ToString("F0");
    }

    public void ToggleHintButton(bool isActive)
    {
        hintButton.gameObject.SetActive(isActive);
    }

    public void OnHintButtonClick()
    {
        // TODO: Implement the hint system
    }

    public void OnRetryButtonCliked()
    {
        GameController.Instance.ResetCarPosition();
    }

    public void OnNextLevelButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        if (GameManager.Instance.currentLevel < 29) { GameManager.Instance.currentLevel++; }
        GameController.Instance.StartGame();
        //ActivatePanel(GamePlayPanel.name);
    }

    public void ActivatePanel(string panelToBeActivated)
    {
        startLoadingPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        loadingPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        mainMenuPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        levelSelectionPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        settingsPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        storePanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        diamondStatsImage.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        exitGamePanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        rateUsPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        skipLevelPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        gamePlayPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        levelPausedPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        levelCompletedPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
        levelFailedPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel.name));
    }


    public void ActivatePanel2(GameObject panelToBeActivated)
    {
        ActivatePanel2(startLoadingPanel);
        startLoadingPanel.SetActive(panelToBeActivated.Equals(startLoadingPanel));
        //panel.SetActive(panel.name == panelName);
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

    public void SetLevelText()
    {
        levelText.text = "LEVEL  " + (1 + GameManager.Instance.currentLevel).ToString();
    }
}
