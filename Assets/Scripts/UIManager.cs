using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake() => Instance = this;

    [Header("UI Panels")]
    public GameObject StartLoadingPanel;
    public GameObject LoadingPanel;
    public GameObject MainMenuPanel;
    public GameObject SettingPanel;
    public GameObject StorePanel;
    public GameObject GamePlayPanel;
    public GameObject SkipLevelPanel;
    public GameObject MajorLevelSelectionPanel;
    public GameObject MinorLevelSelectionPanel;
    public GameObject WinPanel;

    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;


    public GameController gameController;
    public CarSpawner carSpawner;

    public Text scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;

    public Button hintButton;


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
        ActivatePanel(GamePlayPanel.name);
    }

    public void ActivatePanel(string panelToBeActivated)
    {
        StartLoadingPanel.SetActive(panelToBeActivated.Equals(StartLoadingPanel.name));
        LoadingPanel.SetActive(panelToBeActivated.Equals(LoadingPanel.name));
        MainMenuPanel.SetActive(panelToBeActivated.Equals(MainMenuPanel.name));
        SettingPanel.SetActive(panelToBeActivated.Equals(SettingPanel.name));
        StorePanel.SetActive(panelToBeActivated.Equals(StorePanel.name));
        GamePlayPanel.SetActive(panelToBeActivated.Equals(GamePlayPanel.name));
        SkipLevelPanel.SetActive(panelToBeActivated.Equals(SkipLevelPanel.name));
        MajorLevelSelectionPanel.SetActive(panelToBeActivated.Equals(MajorLevelSelectionPanel.name));
        MinorLevelSelectionPanel.SetActive(panelToBeActivated.Equals(MinorLevelSelectionPanel.name));
        WinPanel.SetActive(panelToBeActivated.Equals(WinPanel.name));
    }

    public void ActivatePanel2(GameObject panelToBeActivated)
    {
        ActivatePanel2(StartLoadingPanel);
        StartLoadingPanel.SetActive(panelToBeActivated.Equals(StartLoadingPanel));
        LoadingPanel.SetActive(panelToBeActivated.Equals(LoadingPanel));
        MainMenuPanel.SetActive(panelToBeActivated.Equals(MainMenuPanel));
        SettingPanel.SetActive(panelToBeActivated.Equals(SettingPanel));
        StorePanel.SetActive(panelToBeActivated.Equals(StorePanel));
        GamePlayPanel.SetActive(panelToBeActivated.Equals(GamePlayPanel));
        SkipLevelPanel.SetActive(panelToBeActivated.Equals(SkipLevelPanel));
        MajorLevelSelectionPanel.SetActive(panelToBeActivated.Equals(MajorLevelSelectionPanel));
        MinorLevelSelectionPanel.SetActive(panelToBeActivated.Equals(MinorLevelSelectionPanel));
        WinPanel.SetActive(panelToBeActivated.Equals(WinPanel));
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
