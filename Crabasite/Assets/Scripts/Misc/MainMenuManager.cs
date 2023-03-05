using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour, IDataPersistence
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsGameButton;
    [SerializeField] private Button quitGameButton;

    [SerializeField] private TMP_Dropdown graphicsDD;
    [SerializeField] private TMP_Dropdown resolutionsDD;

    [SerializeField] private Button backToMenu;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameObject volumeText;
    [SerializeField] private GameObject settingsText;
    [SerializeField] private GameObject graphicsText;
    [SerializeField] private GameObject resolutionsText;

    public AudioMixer audioMixer;
    Resolution[] resolutions;

    // Added for the GameData to save
    private string level = "Level 1 - space";
    private int graphicsIndex = 0;
    private float soundVolume = 0;
    public bool isFullscreen;
    public int resolutionsIndex;


    private void Awake()
    {
        InvisibleOptionScreen();
    }

    // New Game starts with fresh GameData
    private async void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }

        resolutions = Screen.resolutions;

        resolutionsDD.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + "@" + resolutions[i].refreshRate + "hz";
            options.Add(option);
            
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionsDD.AddOptions(options);
        resolutionsDD.value = currentResolutionIndex;
        resolutionsDD.RefreshShownValue();
    }
    // Section for the Main Menu Page
    // ==============================
    public void NewGameMenu()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Level 1 - space");
    }

    // Game will be loaded from saved GameData
    public void LoadGameMenu()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(this.level);
    }

    public void OptionsMenu()
    {
        DisableMenuButtons();
        InvisibleMenuButtons();
        VisibleOptionScreen();
        EnableMenuButtons();
        // Add Options Screen here with settings.
    }
    public void QuitGameMenu()
    {
        DisableMenuButtons();
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
    // Section for the Options Page
    // ============================
    
    public void BackToMenu()
    {
        DisableOptionButtons();
        VisibleMenuButtons();
        InvisibleOptionScreen();
        EnableOptionButtons();
    }

    public void SetVolume(float volume)
    {
        Debug.Log(Mathf.Log10(volume)*20);
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        soundVolume = volume;
        Debug.Log(soundVolume);
    }
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsIndex = qualityIndex;
    }
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionsIndex = resolutionIndex;
    }

    // Section for general Funtions
    // ============================
    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        optionsGameButton.interactable = false;
        quitGameButton.interactable = false;
    }
    private void EnableMenuButtons()
    {
        newGameButton.interactable = true;
        loadGameButton.interactable = true;
        optionsGameButton.interactable = true;
        quitGameButton.interactable = true;
    }
    public void DisableOptionButtons()
    {
        backToMenu.interactable = false;
        volumeSlider.interactable = false;
        graphicsDD.interactable = false;
        resolutionsDD.interactable = false;
        fullscreenToggle.interactable = false;
    }
    public void EnableOptionButtons()
    {
        backToMenu.interactable = true;
        volumeSlider.interactable = true;
        graphicsDD.interactable = true;
        resolutionsDD.interactable = true;
        fullscreenToggle.interactable = true;
    }
    private void InvisibleMenuButtons()
    {
        newGameButton.gameObject.SetActive(false);
        loadGameButton.gameObject.SetActive(false);
        optionsGameButton.gameObject.SetActive(false);
        quitGameButton.gameObject.SetActive(false);
    }
    private void VisibleMenuButtons()
    {
        newGameButton.gameObject.SetActive(true);
        loadGameButton.gameObject.SetActive(true);
        optionsGameButton.gameObject.SetActive(true);
        quitGameButton.gameObject.SetActive(true);
    }

    private void VisibleOptionScreen()
    {
        graphicsDD.gameObject.SetActive(true);
        backToMenu.gameObject.SetActive(true);
        settingsText.gameObject.SetActive(true);
        volumeText.gameObject.SetActive(true);
        graphicsText.gameObject.SetActive(true);
        resolutionsText.gameObject.SetActive(true);
        resolutionsDD.gameObject.SetActive(true);
        volumeSlider.gameObject.SetActive(true);
        fullscreenToggle.gameObject.SetActive(true);
    }
    private void InvisibleOptionScreen()
    {
       graphicsDD.gameObject.SetActive(false);
        backToMenu.gameObject.SetActive(false);
        settingsText.gameObject.SetActive(false);
        volumeText.gameObject.SetActive(false);
        graphicsText.gameObject.SetActive(false);
        resolutionsText.gameObject.SetActive(false);
        resolutionsDD.gameObject.SetActive(false);
        volumeSlider.gameObject.SetActive(false);
        fullscreenToggle.gameObject.SetActive(false);
    }
    
        

    public void LoadData(GameData data)
    {
        this.level = data.level;
        this.graphicsIndex = data.graphicsIndex;
        this.soundVolume = data.soundVolume;
        this.isFullscreen = data.isFullscreen;
        this.resolutionsIndex = data.resolutionsIndex;
    }
    public void SaveData(ref GameData data)
    {
        data.level = this.level;
        data.graphicsIndex = this.graphicsIndex;
        data.soundVolume = this.soundVolume;
        data.isFullscreen = this.isFullscreen;
        data.resolutionsIndex = this.resolutionsIndex;
    }

}
