using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Fields
    public static MainMenuUI mui;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject exitGamePanel;
    public GameObject settingsPanel;
    public GameObject rateUsPanel;
    public GameObject levelSelectionPanel;
    public GameObject[] panelsArray;
    public Button[] levelButtons;
    public GameObject[] levelButtonParticals;

    [Header("Settings")]
    public AudioMixer audioMixer;

    private DataManager dm;
    private GameData gd;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        mui = this;
        dm = DataManager.dm;
        gd = dm.gameData;
    }

    private void Start()
    {
        for (int i = 0; i <= gd.level; i++)
        {
            levelButtons[i].interactable = true;
        }
        levelButtonParticals[gd.level].SetActive(true);
    }
    #endregion

    #region Main Menu
    public void OnGameExitButtonClicked()
    {
        PanelActivate(exitGamePanel.name);
        AdsManager.Instance.SetBannerPosition();
        AdsManager.Instance.ShowBanner();
    }

    public void OnStartButtonClicked()
    {
        levelSelectionPanel.SetActive(true);
    }

    public void OnLevelSelectButton(int level)
    {
        GameManager.Instance.level = level;
        GameManager.Instance.nextScene = "Game Play";
        SceneManager.LoadScene("Loading");
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

    #region Settings
    public void PlaySound(string soundName)
    {
        AudioManager.am.Play(soundName);
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

    public void GoToSettings()
    {
        PanelActivate(settingsPanel.name);
    }

    public void GameExitYes()
    {
        Application.Quit();
    }

    public void PrivacyButton()
    {
        Debug.Log("Privacy Button Clicked");
    }

    public void SoundVolume(float amount)
    {

    }

    public void MusicVolume(float amount)
    {

    }
    #endregion

    private void SaveData()
    {
        dm.SaveData();
    }
}
