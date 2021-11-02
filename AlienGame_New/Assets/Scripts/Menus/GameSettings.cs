using UnityEngine;

public class GameSettings
{
    string text = "//Project: AlienGame Game Settings File, DO NOT CHANGE IF NOT CERTAIN";
    string hardHeader = "HARDWARE";
    /*HARDWARE*/
    public int monitorNr;
    public FullScreenMode fulscreenMode;
    public int resolutionIdx;
    public int refreshRate;
    public int vsync;
    public int aspectRatio;
    public int qualityPreset;
    string displayHeader = "DISPLAY";
    /*DISPLAY*/
    public int colorblindMode;
    public float FOV;
    public bool framerateLimit;
    public int frameLimit;
    string texHeader = "Textures";
    /*DISPLAY & TEXTURES*/
    public int textureQuality;
    public int anisotropicFilter;
    public int SSR;
    public int particleQuality;
    string shadowsHeader = "SHADOWS";
    /*SHADOWS*/
    public ShadowQuality shadowQuality;
    public ShadowResolution shadowResolution;
    public int lolumetricLights;
    public int contactShadows;
    public int particleShadows;
    string pfxHeader = "POST PROCESSING";
    /*POST PROCESSING*/
    public int antiAliasing;
    public bool AO;
    public int AOQuality;
    public bool MB;
    public int MBQuality;
    public int SSScattering;
}
