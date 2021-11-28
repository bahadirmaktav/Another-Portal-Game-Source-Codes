using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelsPageController : MonoBehaviour
{
    //TODO Levels unlock, lock mechanism will be added.
    [Header("Initializations")]
    public GameObject levelButtonPrefab;
    private GameObject parentOfLevelButtons;

    [Header("Levels Page Input Parameters")]
    public int maxLevelCounter = 5;

    [Header("Levels Page Control Parameters")]
    private float firstXaxisPos = -4.36f;
    private float yAxisPos = -0.9f;
    private float distanceBetweenButtons = 1.3f;
    private GameObject[] levelButtons;

    private void Awake()
    {
        levelButtons = new GameObject[maxLevelCounter];
        parentOfLevelButtons = GameObject.Find("LevelButtonsContainer");
        for (int i = 0; i < maxLevelCounter; i++)
        {
            GameObject levelButtonInstantiated = Instantiate(levelButtonPrefab, new Vector3(firstXaxisPos + i * distanceBetweenButtons, yAxisPos, 0), Quaternion.Euler(0, 0, 0), parentOfLevelButtons.transform);
            levelButtons[i] = levelButtonInstantiated;
            levelButtons[i].GetComponent<Button>().onClick.AddListener(GoToSelectedLevel);
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
        SceneManager.LoadScene("Level" + (selectedLevel + 1).ToString());
    }
}
