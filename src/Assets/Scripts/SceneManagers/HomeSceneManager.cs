
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[AddComponentMenu("Scene Management/Home Scene Manager")]
public class HomeSceneManager : MonoBehaviour
{

    public Profile profile;
    public Settings settings;
    public ItemList itemList;
    public LevelInfo currentSelectedLevel;
    public ViewManager ui;
    public StageSelector levelSelector;

    [Header("Camera")]
    public Transform cameraRig;

    [Header("Settings")]
	public AudioMixer masterMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Customize")]
    public InputField characterNameInputField;

    void Start()
    {
        this.CheckLoadSettings();

        characterNameInputField.text = profile.CharacterName;
        characterNameInputField.onEndEdit.AddListener(value =>
        {
            profile.CharacterName = value;
        });
        profile.OnRevert.AddListener(() =>
        {
            characterNameInputField.text = profile.CharacterName;
        });

        bgmSlider.value = settings.BgmVolume;
        bgmSlider.onValueChanged.AddListener(value =>
        {
            settings.BgmVolume = value;
            masterMixer.SetFloat("BgmVolume", bgmSlider.value);
        });
        sfxSlider.value = settings.SfxVolume;
        sfxSlider.onValueChanged.AddListener(value =>
        {
            settings.SfxVolume = value;
            masterMixer.SetFloat("SfxVolume", sfxSlider.value);
        });
        settings.OnRevert.AddListener(() =>
        {
            bgmSlider.value = settings.BgmVolume;
            sfxSlider.value = settings.SfxVolume;
            masterMixer.SetFloat("BgmVolume", bgmSlider.value);
            masterMixer.SetFloat("SfxVolume", sfxSlider.value);
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ui.currentView == ViewType.MainMenu)
            {
                ui.Goto(ViewType.Exit);
            }
            else
            {
                ui.Goto(ui.previousView);
            }
        }
    }
    
    /// <summary>
    /// Called from button OnClick
    /// </summary>
    public void GotoHome()
    {
        ui.Goto(ViewType.MainMenu);
    }

    /// <summary>
    /// Called from button OnClick
    /// </summary>
    public void GotoSettings()
    {
        ui.Goto(ViewType.Settings);
    }

    /// <summary>
    /// Called from button OnClick
    /// </summary>
    public void GotoCustomize()
    {
        ui.Goto(ViewType.Customize);
    }

    /// <summary>
    /// Called from button OnClick
    /// </summary>
    public void GotoLevelSelect()
    {
        ui.Goto(ViewType.LevelSelect);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CancelExitGame()
    {
        ui.Goto(ui.previousView);
    }

    public void LoadSelectedScene()
    {
        this.LoadScene(this.currentSelectedLevel.BuildIndex);
    }

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    private void CheckLoadSettings()
    {
        var loadSettings = GameObject.FindGameObjectWithTag("SceneLoadOptions");
        if (loadSettings != null)
        {
            var comp = loadSettings.GetComponent<SceneLoadOptions>();
            this.ui.Goto(comp.TargetView);
            var levelSelector = GameObject.FindObjectOfType<StageSelector>();
            if (levelSelector != null)
            {
                levelSelector.SelectLevelWithBuildIndex(comp.ExitedLevel);
            }

            Destroy(loadSettings.gameObject);
        }
    }
}
