using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class GameSettingsManager : MonoBehaviour
{
    public GameObject OptionsMenu;
    /*HARDWARE*/
    [Header("HARDWARE")]
    public bool monitorNrEnable;
    public Dropdown monitorNrDropdown;
    public bool fullscreenModeEnable;
    public Dropdown fulscreenModeDropdown;
    public Resolution[] resolutions;
    public bool resolutionEnable;
    public Dropdown resolutionDropdown;
    public bool refreshRateEnable;
    public Dropdown refreshRateDropdown;
    public bool vsyncToggleEnable;
    public ToggleGroup vsyncToggle;
    public bool aspectRationEnable;
    public Dropdown aspectRatioDropdown;
    public bool QualityPresetEnable;
    public Dropdown QualityPresetDropdown;
    /*DISPLAY*/
    [Header("DISPLAY")]
    public bool colorblindModeEnable;
    public Dropdown colorblindModeDropdown;
    public bool FOVSliderEnable;
    public Slider FOVSlider;
    public Text FOVValueText;
    public float FOVValue;
    public bool framerateLimitSliderEnable;
    public ToggleGroup framerateLimitToggle;
    public Slider frameLimitSlider;
    public Text framelimitValueText;
    public int frameLimitValue;
    /*DETAILS & TEXTURES*/
    [Header("DETAILS & TEXTURES")]
    public bool textureQualityEnable;
    public Dropdown textureQualityDropdown;
    public bool anisotropicFilterEnable;
    public Dropdown anisotropicFilterDropdown;
    public bool SSREnable;
    public Dropdown SSR;
    public bool particleQualityEnable;
    public Dropdown particleQuality;
    /*SHADOWS*/
    [Header("SHADOWS")]
    public ShadowQuality[] shadowQualities;
    public bool shadowQualityEnable;
    public Dropdown shadowQualityDropdown;
    public bool shadowResolutionEnable;
    public Dropdown shadowResolutionDropdown;
    public bool volumetricLightsEnable;
    public Dropdown volumetricLightsDropdown;
    public bool contactShadowsEnable;
    public Dropdown contactShadowsDropdown;
    public bool particleShadowsEnable;
    public Dropdown particleShadows;
    /*POST PROCESSING*/
    [Header("POST PROCESSING")]
    public bool antiAliasingEnable;
    public Dropdown antiAliasingDropdown;
    public bool AOToggleEnable;
    public ToggleGroup AOToggle;
    public Dropdown AOQualityDropdown;
    public bool MotionBlurEnable;
    public ToggleGroup MBToggle;
    public Dropdown MBQualityDropdown;
    public bool SSScatteringEnable;
    public Dropdown SSScatteringDropdown;

    public GameSettings gameSettings;
    public Toggle currentToggle
    {
        get { return vsyncToggle.ActiveToggles().FirstOrDefault(); }
    }

    void OnEnable()
    {
        gameSettings = new GameSettings();
        LoadSettings();
        resolutions = Screen.resolutions;
        
        if (monitorNrEnable) { monitorNrDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { monitorNrDropdown.interactable = false; }
        if (fullscreenModeEnable) { fulscreenModeDropdown.onValueChanged.AddListener(delegate { onFullscreenToggle(); }); } else { fulscreenModeDropdown.interactable = false; }
        if (resolutionEnable) { resolutionDropdown.onValueChanged.AddListener(delegate { onResolutionChanged(); }); } else { resolutionDropdown.interactable = false; }
        if (refreshRateEnable) { refreshRateDropdown.onValueChanged.AddListener(delegate { onRefreshRateChanged(); }); } else { refreshRateDropdown.interactable = false; }
        /*if (vsyncToggleEnable) { vsyncToggle.ActiveToggles().AddListener(delegate { onVsyncChanged(); }); } else { fulscreenModeDropdown.interactable = false; }*/ 
        if (aspectRationEnable) { aspectRatioDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { aspectRatioDropdown.interactable = false; }
        if (QualityPresetEnable) { QualityPresetDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { QualityPresetDropdown.interactable = false; }
        if (colorblindModeEnable) { colorblindModeDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { colorblindModeDropdown.interactable = false; }
        if (FOVSliderEnable) { FOVSlider.onValueChanged.AddListener(delegate { onFOVChanged(); }); } else { FOVSlider.interactable = false; }
        if (framerateLimitSliderEnable) { frameLimitSlider.onValueChanged.AddListener(delegate { onFrameLimitChanged(); }); } else { frameLimitSlider.interactable = false; }
        if (textureQualityEnable) { textureQualityDropdown.onValueChanged.AddListener(delegate { onTextureQualityChanged(); }); } else { textureQualityDropdown.interactable = false; }
        if (anisotropicFilterEnable) { anisotropicFilterDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { anisotropicFilterDropdown.interactable = false; }
        if (SSREnable) { SSR.onValueChanged.AddListener(delegate { /**/ }); } else { SSR.interactable = false; }
        if (particleQualityEnable) { particleQuality.onValueChanged.AddListener(delegate { /**/ }); } else { particleQuality.interactable = false; }
        if (shadowQualityEnable) { shadowQualityDropdown.onValueChanged.AddListener(delegate { onShadowQualityChanged(); }); } else { shadowQualityDropdown.interactable = false; }
        if (shadowResolutionEnable) { shadowResolutionDropdown.onValueChanged.AddListener(delegate { onShadowResolutionChanged(); }); } else { shadowResolutionDropdown.interactable = false; }
        if (volumetricLightsEnable) { volumetricLightsDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { volumetricLightsDropdown.interactable = false; }
        if (contactShadowsEnable) { contactShadowsDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { contactShadowsDropdown.interactable = false; }
        if (particleShadowsEnable) { particleShadows.onValueChanged.AddListener(delegate { /**/ }); } else { particleShadows.interactable = false; }
        if (antiAliasingEnable) { antiAliasingDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { antiAliasingDropdown.interactable = false; }
        if (AOToggleEnable) { AOQualityDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { AOQualityDropdown.interactable = false; }
        if (MotionBlurEnable) { MBQualityDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { MBQualityDropdown.interactable = false; }
        if (SSScatteringEnable) { SSScatteringDropdown.onValueChanged.AddListener(delegate { /**/ }); } else { SSScatteringDropdown.interactable = false; }


        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.width + " x " + resolution.height));
            refreshRateDropdown.options.Add(new Dropdown.OptionData(resolution.refreshRate + " Hz"));
        }
    }

    void Update()
    {
        onFOVChanged();
        onFrameLimitChanged();
        onTextureQualityChanged();
        onShadowQualityChanged();
        onShadowResolutionChanged();
    }

    public void ApplyButton()
    {
        SaveSettings();
        Debug.Log("Settings Saved");
    }

    public void onFullscreenToggle()
    {
        gameSettings.fulscreenMode = (FullScreenMode)fulscreenModeDropdown.value;
    }

    public void onResolutionChanged()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
    }

    public void onRefreshRateChanged()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreenMode, resolutions[refreshRateDropdown.value].refreshRate);
    }

    public void onVsyncChanged() //needs work
    {
        var toggle = vsyncToggle.GetComponentsInChildren<Toggle>();
        if (toggle[0].isOn)
        {
            gameSettings.vsync = 1;
        } 
        else if (toggle[1].isOn)
        {
            gameSettings.vsync = 0;
        }

        QualitySettings.vSyncCount = gameSettings.vsync;
    }

    public void onFOVChanged()
    {
        FOVValue = gameSettings.FOV = FOVSlider.value;
        //needs player update

        //Update UI
        FOVValueText.text = FOVValue.ToString();
    }

    public void onFrameLimitChanged()
    {
        frameLimitValue = gameSettings.frameLimit = (int)frameLimitSlider.value;
        Application.targetFrameRate = frameLimitValue;

        //Update UI
        framelimitValueText.text = frameLimitValue.ToString();
    }

    public void onTextureQualityChanged()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    public void onShadowQualityChanged()
    {
        QualitySettings.shadows = gameSettings.shadowQuality = (ShadowQuality)shadowQualityDropdown.value;
    }

    public void onShadowResolutionChanged()
    {
        QualitySettings.shadowResolution = gameSettings.shadowResolution = (ShadowResolution)shadowResolutionDropdown.value;
    }

    public void SaveSettings()
    {
        string dir = Application.persistentDataPath + "/GameSettings.json";

        string SavegameSettings = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(dir, SavegameSettings);
    }

    public void LoadSettings()
    {
        if(!File.Exists(Application.persistentDataPath + "/GameSettings.json"))
        {
            SaveSettings();
        }
        
        string LoadgameSettings = File.ReadAllText(Application.persistentDataPath + "/GameSettings.json");
        gameSettings = JsonUtility.FromJson<GameSettings>(LoadgameSettings);
        //Load Texture Setting
        QualitySettings.masterTextureLimit = textureQualityDropdown.value = gameSettings.textureQuality;
        //Load ShadowQuality Setting
        shadowQualityDropdown.value = (int)gameSettings.shadowQuality;
        QualitySettings.shadows = (ShadowQuality)shadowQualityDropdown.value;
        //Load ShadowResolution Setting
        shadowResolutionDropdown.value = (int)gameSettings.shadowQuality;
        QualitySettings.shadowResolution = (ShadowResolution)shadowResolutionDropdown.value;
        //Load Framelimt Setting
        frameLimitSlider.value = gameSettings.frameLimit;
        Application.targetFrameRate = (int)frameLimitSlider.value;
            

        Debug.Log("Settings Loaded");
    }
}
