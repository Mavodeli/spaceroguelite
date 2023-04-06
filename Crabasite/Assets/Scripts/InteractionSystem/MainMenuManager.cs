using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.IO;
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
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private GameObject textSpeedText;

    public AudioMixer audioMixer;
    Resolution[] resolutions;

    // Added for the GameData to save
    private string level = "Level 0 - spaceship";
    private int graphicsIndex;
    private float soundVolume;
    public bool isFullscreen;
    public int resolutionsIndex;
    public float textSpeed;


    private void Awake()
    {
        InvisibleOptionScreen();
    }

    // New Game starts with fresh GameData
    private async void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "data.game");
        if (!File.Exists(path))
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
    }
    // Section for the Main Menu Page
    // ==============================
    public void NewGameMenu()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Level 0 - spaceship");
    }

    // Game will be loaded from saved GameData
    public void LoadGameMenu()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.LoadGame(true);
        SceneManager.LoadSceneAsync(this.level);
    }

    public void OptionsMenu()
    {
        DisableMenuButtons();
        InvisibleMenuButtons();
        VisibleOptionScreen();
        EnableOptionButtons();
        LoadOptions();
        updateOptionControls();
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
        InvisibleOptionScreen();
        EnableMenuButtons();
        VisibleMenuButtons();
        SaveOptions();
    }

    public void SetVolume(float volume)
    {
        Debug.Log(Mathf.Log10(volume)*20);
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        soundVolume = volume;
        Debug.Log(soundVolume);
        GameObject.Find("Sounds").SendMessage("setVolume", soundVolume);
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
    public void SetTextSpeed (float textSpeed)
    {
        this.textSpeed = textSpeed;
        GameObject cBox = GameObject.Find("CommentBox");
        CommentarySystem cs;
        if (cBox) {
            cs = GameObject.Find("CommentBox").GetComponent<CommentarySystem>();
        } else {
            cs = null;
        }
        if (cs) { // if there is one in the scene, update it directly (for if we add in-game options)
            cs.setTypeWriterSpeed(textSpeed);
        }
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
        textSpeedSlider.interactable = false;
    }
    public void EnableOptionButtons()
    {
        backToMenu.interactable = true;
        volumeSlider.interactable = true;
        graphicsDD.interactable = true;
        resolutionsDD.interactable = true;
        fullscreenToggle.interactable = true;
        textSpeedSlider.interactable = true;
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
        textSpeedSlider.gameObject.SetActive(true);
        textSpeedText.gameObject.SetActive(true);
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
        textSpeedSlider.gameObject.SetActive(false);
        textSpeedText.gameObject.SetActive(false);
    }
    
        

    public void LoadData(GameData data)
    {
        this.level = data.level;
    }
    public void SaveData(ref GameData data)
    {
        data.level = this.level;
    }

    public void LoadOptions() {
        OptionData optionData = OptionPersistenceManager.instance.GetOptionData();
        this.graphicsIndex = optionData.graphicsIndex;
        this.soundVolume = optionData.soundVolume;
        this.isFullscreen = optionData.isFullscreen;
        this.resolutionsIndex = optionData.resolutionsIndex;
        this.textSpeed = optionData.textSpeed;
    }
    public void SaveOptions() {
        OptionData optionData = new OptionData();
        optionData.graphicsIndex = this.graphicsIndex;
        optionData.soundVolume = this.soundVolume;
        optionData.isFullscreen = this.isFullscreen;
        optionData.resolutionsIndex = this.resolutionsIndex;
        optionData.textSpeed = this.textSpeed;
        OptionPersistenceManager.instance.SetOptionData(optionData);
    }

    private void updateOptionControls()
    {
        volumeSlider.value = this.soundVolume;
        textSpeedSlider.value = this.textSpeed;
        fullscreenToggle.isOn = this.isFullscreen;
        graphicsDD.value = this.graphicsIndex;
        resolutionsDD.value = this.resolutionsIndex;
    }
}
