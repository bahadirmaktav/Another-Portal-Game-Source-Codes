using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPageController : MonoBehaviour
{
    private void Awake()
    {
        DataController dataController = new DataController();
        dataController.InitializeAllData();
    }

    public void GoLevelsPage()
    {
        SceneManager.LoadScene("LevelsPageScene", LoadSceneMode.Single);
    }

    public void GoSettingsPage()
    {
        SceneManager.LoadScene("SettingsPageScene", LoadSceneMode.Single);
    }
}
