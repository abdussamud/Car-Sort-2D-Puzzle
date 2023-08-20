using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Fields
    public static UIManager mui;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject exitGamePanel;
    public GameObject settingsPanel;
    public GameObject rateUsPanel;
    public GameObject[] panelsArray;

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
    #endregion

    #region Main Menu
    public void OnGameExitButtonClicked()
    {
        PanelActivate(exitGamePanel.name);
    }

    public void OnStartButtonClicked()
    {
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
