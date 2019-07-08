using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour {
	public Toggle fullscreenToggle;
	public Dropdown resolutionsDropdown;
	public Dropdown textureQualityDropdown;
    //public Toggle shadowsToggle;
    public Toggle motionToggle;
	public Dropdown aaDropdown;
	public Dropdown vSyncDropdown;
	public Slider musicVolumeSlider;
    public Slider FOVSlider;
    public Button applyButton;
	public AudioSource music;
	public Resolution[] _resolutions;
	public GameSettings gameSettings;
    public GameObject PlayerCamera;

	void OnEnable() 
	{
        if(File.Exists(Application.persistentDataPath + "/gamesettings.json") == false)
        {
            SaveSettings();
        }


		gameSettings = new GameSettings ();

		fullscreenToggle.onValueChanged.AddListener (delegate { OnFullscreenToggle(); });
		resolutionsDropdown.onValueChanged.AddListener (delegate { OnResolutionChange(); });
		textureQualityDropdown.onValueChanged.AddListener (delegate { OnTextureChange(); });
		aaDropdown.onValueChanged.AddListener (delegate { OnAAChange(); });
		vSyncDropdown.onValueChanged.AddListener (delegate { OnVSyncChange(); });
		musicVolumeSlider.onValueChanged.AddListener (delegate { OnMusicVolume(); });
        FOVSlider.onValueChanged.AddListener (delegate { OnFOVChange(); });
        //shadowsToggle.onValueChanged.AddListener (delegate { OnShadowToggle(); });
        motionToggle.onValueChanged.AddListener (delegate { OnMotionChange(); });
        applyButton.onClick.AddListener (delegate { OnApplyButtonClick(); });

		_resolutions = Screen.resolutions;
		foreach (Resolution resolution in _resolutions) 
		{
			resolutionsDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
		}

		LoadSettings ();
	}

	public void OnApplyButtonClick() 
	{
		OnFullscreenToggle();
		OnResolutionChange();
		OnTextureChange();
        //OnShadowToggle();
		OnAAChange();
		OnVSyncChange();
        OnMusicVolume();
        OnFOVChange();
		SaveSettings ();
	}

	public void OnFullscreenToggle()
	{
		gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
	}

	public void OnResolutionChange() 
	{
		Screen.SetResolution (_resolutions [resolutionsDropdown.value].width, _resolutions [resolutionsDropdown.value].height, Screen.fullScreen);
		gameSettings.resIndex = resolutionsDropdown.value;
	}

	public void OnTextureChange()
	{
		QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
	}

	public void OnShadowToggle()
	{
        //LOL I DON'T DO ANYTHING! XD
	}

    public void OnMotionChange()
    {
        //PlayerCamera.GetComponent<A>
    }

	public void OnAAChange()
	{
		QualitySettings.antiAliasing = gameSettings.antialiasing = (int)Mathf.Pow (2, aaDropdown.value);

	}

	public void OnVSyncChange()
	{
		QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
	}

	public void OnMusicVolume()
	{
		music.volume = gameSettings.musicVolume = musicVolumeSlider.value;
	}

	public void OnFOVChange()
	{
        PlayerCamera.GetComponent<Camera>().fieldOfView = gameSettings.FOV = FOVSlider.value;
	}

	public void SaveSettings() 
	{
		string jsonData = JsonUtility.ToJson(gameSettings, true);
		File.WriteAllText (Application.persistentDataPath + "/gamesettings.json", jsonData);
	}

	public void LoadSettings()
	{
		gameSettings = JsonUtility.FromJson<GameSettings> (File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

		musicVolumeSlider.value = gameSettings.musicVolume;
		aaDropdown.value = gameSettings.antialiasing;
		vSyncDropdown.value = gameSettings.vSync;
		textureQualityDropdown.value = gameSettings.textureQuality;
		resolutionsDropdown.value = gameSettings.resIndex;
		fullscreenToggle.isOn = gameSettings.fullscreen;
        FOVSlider.value = gameSettings.FOV;
		resolutionsDropdown.RefreshShownValue ();
        //shadowsToggle.isOn = gameSettings.shadows;
	}
}
