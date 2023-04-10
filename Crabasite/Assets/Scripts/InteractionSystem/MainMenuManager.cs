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
    [SerializeField] private TMP_Dropdown vsyncDD;

    [SerializeField] private Button backToMenu;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameObject volumeText;
    [SerializeField] private GameObject settingsText;
    [SerializeField] private GameObject graphicsText;
    [SerializeField] private GameObject resolutionsText;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private GameObject textSpeedText;
    [SerializeField] private GameObject vsyncText;

    [SerializeField] private GameObject confirmNewGameMenu;

    public AudioMixer audioMixer;
    Resolution[] resolutions;

    // Added for the GameData to save
    private string level = "Level 0 - spaceship";
    private int graphicsIndex;
    private float soundVolume;
    public bool isFullscreen;
    public int resolutionsIndex;
    public float textSpeed;
    private int vsync;


    private void Awake()
    {
        InvisibleOptionScreen();
        InvisibleConfirmNewGameMenu();
    }

    private void Start() {
        Invoke("delayedStart", 0.1f); // delay start to allow QualitySystem & Co to fully set up
    }

    private void delayedStart() {
        // Load button available?
        string path = Path.Combine(Application.persistentDataPath, "data.game");
        if (!File.Exists(path))
        {
            loadGameButton.interactable = false;            
        }

        // Resolution options
        Resolution[] availableResolutions = Screen.resolutions;
        List<Resolution> resolutionCollection = new List<Resolution>();

        resolutionsDD.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            // hide options that don't play nice with our menu size
            if (availableResolutions[i].width < 800 || availableResolutions[i].height < 600) { continue; }
            // add resolution to list of resolutions
            resolutionCollection.Add(availableResolutions[i]);
            options.Add(availableResolutions[i].ToString());
        }
        resolutionsDD.AddOptions(options);
        this.resolutions = resolutionCollection.ToArray();

        // default resolution if no options are available should be best available
        string optionsPath = Path.Combine(Application.persistentDataPath, "options.json");
        if (!File.Exists(optionsPath)) {
            LoadOptions();
            this.resolutionsIndex = resolutions.Length - 1;
        }
        else { LoadOptions(); }
        applyOptions();
        SaveOptions();
    }
    // Section for the Main Menu Page
    // ==============================
    public void NewGameMenu()
    {
        if (this.loadGameButton.interactable) { // if there is a SaveGame
            InvisibleMenuButtons();
            VisibleConfirmNewGameMenu();
        } else {
            NewGameConfirmed();
        }
    }

    public void NewGameConfirmed()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Level 0 - spaceship");
    }

    // Game will be loaded from saved GameData
    public void LoadGameMenu()
    {
        DataPersistenceManager.instance.LoadGame(true);
        SceneManager.LoadSceneAsync(this.level);
    }

    public void OptionsMenu()
    {
        InvisibleMenuButtons();
        VisibleOptionScreen();
        LoadOptions();
        updateOptionControls();
    }
    public void QuitGameMenu()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
    // Section for the Options Page
    // ============================
    
    public void BackToMenu()
    {
        InvisibleOptionScreen();
        InvisibleConfirmNewGameMenu();
        VisibleMenuButtons();
        SaveOptions();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        soundVolume = volume;
        GameObject.Find("Sounds").SendMessage("setVolume", soundVolume, SendMessageOptions.DontRequireReceiver);
    }
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsIndex = qualityIndex;
    }
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        this.isFullscreen = isFullscreen;
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
    public void SetVSync(int vsyncValue)
    {
        this.vsync = vsyncValue;
        QualitySettings.vSyncCount = vsyncValue;
    }

    // Section for general Funtions
    // ============================
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
        vsyncDD.gameObject.SetActive(true);
        vsyncText.gameObject.SetActive(true);
#if UNITY_WEBGL
        fullscreenToggle.gameObject.SetActive(false);
#endif
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
        vsyncDD.gameObject.SetActive(false);
        vsyncText.gameObject.SetActive(false);
    }
    private void VisibleConfirmNewGameMenu()
    {
        confirmNewGameMenu.SetActive(true);
    }
    private void InvisibleConfirmNewGameMenu()
    {
        confirmNewGameMenu.SetActive(false);
    }
    
        

    public void LoadData(GameData data)
    {
        // this.level = data.level;
    }
    public void SaveData(ref GameData data)
    {
        // data.level = this.level;
    }

    public void LoadOptions() {
        OptionData optionData = OptionPersistenceManager.instance.GetOptionData();
        this.graphicsIndex = optionData.graphicsIndex;
        this.soundVolume = optionData.soundVolume;
        this.isFullscreen = optionData.isFullscreen;
        this.resolutionsIndex = optionData.resolutionsIndex;
        this.textSpeed = optionData.textSpeed;
        this.vsync = optionData.vsync;
    }
    public void SaveOptions() {
        OptionData optionData = new OptionData();
        optionData.graphicsIndex = this.graphicsIndex;
        optionData.soundVolume = this.soundVolume;
        optionData.isFullscreen = this.isFullscreen;
        optionData.resolutionsIndex = this.resolutionsIndex;
        optionData.textSpeed = this.textSpeed;
        optionData.vsync = this.vsync;
        OptionPersistenceManager.instance.SetOptionData(optionData);
    }

    private void updateOptionControls()
    {
        volumeSlider.value = this.soundVolume;
        textSpeedSlider.value = this.textSpeed;
        fullscreenToggle.isOn = this.isFullscreen;
        graphicsDD.value = this.graphicsIndex;
        resolutionsDD.value = this.resolutionsIndex;
        vsyncDD.value = this.vsync;
    }

    private void applyOptions() {
        // Volume
        audioMixer.SetFloat("volume", Mathf.Log10(soundVolume)*20);
        GameObject.Find("Sounds").SendMessage("setVolume", soundVolume, SendMessageOptions.DontRequireReceiver);
        // Quality
        QualitySettings.SetQualityLevel(this.graphicsIndex);
        // V-Sync
        QualitySettings.vSyncCount = this.vsync;
        // Resolution and fullscreen
        Resolution resolution = resolutions[this.resolutionsIndex];
        Screen.SetResolution(resolution.width, resolution.height, this.isFullscreen);
        // textSpeed
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
}
