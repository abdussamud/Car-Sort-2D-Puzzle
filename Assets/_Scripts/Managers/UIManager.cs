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
    public GameObject levelSelectionPanel;
    public GameObject settingsPanel;
    public GameObject storePanel;
    public GameObject exitGamePanel;
    public GameObject rateUsPanel;
    public GameObject[] panelsArray;

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

    #endregion


    #region Unity Methods
    private void Awake() { Instance = this; }

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
        gameData.isSoundOn = true;
        DataManager.Instance.SaveData();
    }
    public void SoundOff()
    {
        _ = audioMixer.SetFloat("SFX", -80);
        _ = audioMixer.SetFloat("Ui", -80);
        mainMenuSoundOn.SetActive(false);
        mainMenuSoundOff.SetActive(true);
        gameData.isSoundOn = false;
        DataManager.Instance.SaveData();
    }
    public void MusicOn()
    {
        _ = audioMixer.SetFloat("BGM", -3);
        mainMenuMusicOn.SetActive(true);
        mainMenuMusicOff.SetActive(false);
        gameData.isMusicOn = true;
        DataManager.Instance.SaveData();
    }
    public void MusicOff()
    {
        _ = audioMixer.SetFloat("BGM", -80);
        mainMenuMusicOn.SetActive(false);
        mainMenuMusicOff.SetActive(true);
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
        gemsStats.SetActive(panelName.Equals(mainMenuPanel.name) || panelName.Equals(storePanel.name) ||
            panelName.Equals(settingsPanel.name) || panelName.Equals(levelSelectionPanel.name));
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

    public void GameExitYes()
    {
        Application.Quit();
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
