using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelsPageController : MonoBehaviour
{
    [Header("Initializations")]
    public GameObject levelButtonPrefab;
    private GameObject parentOfLevelButtons;

    [Header("Levels Page Input Parameters")]
    public int maxLevelCounter = 5;

    [Header("Levels Page Control Parameters")]
    private float firstXaxisPos = -4.5f;
    private float yAxisPos = -1.8f;
    private float distanceBetweenButtons = 1.8f;
    private GameObject[] levelButtons;
    private int completedLevelCounter;

    private void Awake()
    {
        DataController dataController = new DataController();
        completedLevelCounter = dataController.GetCompletedLevelCounter();
        levelButtons = new GameObject[maxLevelCounter];
        parentOfLevelButtons = GameObject.Find("LevelButtonsContainer");
        for (int i = 0; i < maxLevelCounter; i++)
        {
            GameObject levelButtonInstantiated = Instantiate(levelButtonPrefab, new Vector3(firstXaxisPos + i * distanceBetweenButtons, yAxisPos, 0), Quaternion.Euler(0, 0, 0), parentOfLevelButtons.transform);
            levelButtons[i] = levelButtonInstantiated;
            levelButtons[i].GetComponent<Button>().onClick.AddListener(GoToSelectedLevel);
            if (completedLevelCounter >= i)
            {
                levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                levelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                levelButtons[i].transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            }
        }
    }

    private void GoToSelectedLevel()
    {
        GameObject pressedButtonObj = EventSystem.current.currentSelectedGameObject;
        int selectedLevel = 0;
        for (int i = 0; i < maxLevelCounter; i++)
        {
            if (pressedButtonObj == levelButtons[i])
            {
                selectedLevel = i;
                break;
            }
        }
        if (completedLevelCounter >= selectedLevel) { SceneManager.LoadScene("Level" + (selectedLevel + 1).ToString()); }
    }

    public void BackToMainPageButton()
    {
        SceneManager.LoadScene("MainPageScene", LoadSceneMode.Single);
    }
}
