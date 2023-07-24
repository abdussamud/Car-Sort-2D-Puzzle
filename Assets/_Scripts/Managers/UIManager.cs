using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Fields
    public static UIManager Instance;
    public GameData gameData;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject exitGamePanel;
    public GameObject settingsPanel;
    public GameObject rateUsPanel;
    public GameObject[] panelsArray;

    [Header("Settings")]
    public AudioMixer audioMixer;

    #endregion


    #region Unity Methods
    private void Awake() { Instance = this; }

    private void Start()
    {
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
    public void SoundOn()
    {
        _ = audioMixer.SetFloat("SFX", -3);
        _ = audioMixer.SetFloat("Ui", -3);
        gameData.isSoundOn = true;
        DataManager.Instance.SaveData();
    }
    public void SoundOff()
    {
        _ = audioMixer.SetFloat("SFX", -80);
        _ = audioMixer.SetFloat("Ui", -80);
        gameData.isSoundOn = false;
        DataManager.Instance.SaveData();
    }
    public void MusicOn()
    {
        _ = audioMixer.SetFloat("BGM", -3);
        gameData.isMusicOn = true;
        DataManager.Instance.SaveData();
    }
    public void MusicOff()
    {
        _ = audioMixer.SetFloat("BGM", -80);
        gameData.isMusicOn = false;
        DataManager.Instance.SaveData();
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
}
