using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPageController : MonoBehaviour
{
    [Header("Initializations")]
    private Slider uiSoundsLevelSlider;
    private Slider musicLevelSlider;

    [Header("Settings Page Control Paramters")]
    private float musicLevel;
    private float uiSoundLevel;

    private void Awake()
    {
        uiSoundsLevelSlider = GameObject.Find("UISoundsLevelSlider").GetComponent<Slider>();
        musicLevelSlider = GameObject.Find("MusicLevelSlider").GetComponent<Slider>();
        DataController dataController = new DataController();
        musicLevel = dataController.GetMusicLevel();
        uiSoundLevel = dataController.GetUISoundLevel();
        musicLevelSlider.value = musicLevel;
        uiSoundsLevelSlider.value = uiSoundLevel;
    }

    public void BackToMainPageButton()
    {
        DataController dataController = new DataController();
        dataController.SaveUISoundAndMusicLevel(uiSoundLevel, musicLevel);
        SceneManager.LoadScene("MainPageScene", LoadSceneMode.Single);
    }

    public void SetMusicLevel()
    {
        musicLevel = musicLevelSlider.value;
    }

    public void SetUISoundLevel()
    {
        uiSoundLevel = uiSoundsLevelSlider.value;
    }
}
