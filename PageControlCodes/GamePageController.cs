using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePageController : MonoBehaviour
{
    private Animator levelPauseTransitionControllerAnimator;
    private GameObject levelPauseTransitionController;
    private Animator levelFinishTransitionControllerAnimator;
    private GameObject levelFinishTransitionController;
    [HideInInspector] public Text leftStonesText;

    private void Awake()
    {
        levelPauseTransitionControllerAnimator = GameObject.Find("LevelPauseTransitionController").GetComponent<Animator>();
        levelFinishTransitionControllerAnimator = GameObject.Find("LevelFinishTransitionController").GetComponent<Animator>();
        leftStonesText = GameObject.Find("LeftStonesText").GetComponent<Text>();
        leftStonesText.text = (GameObject.FindGameObjectsWithTag("CollectableObjects").Length).ToString();
    }

    public void GoToMainPage()
    {

        SceneManager.LoadScene("MainPageScene", LoadSceneMode.Single);
    }

    public void PauseTheLevel()
    {
        levelPauseTransitionControllerAnimator.SetBool("isContinued", false);
        levelPauseTransitionControllerAnimator.SetBool("isPaused", true);
    }

    public void ContinueTheLevel()
    {
        levelPauseTransitionControllerAnimator.SetBool("isPaused", false);
        levelPauseTransitionControllerAnimator.SetBool("isContinued", true);
    }

    public void RestartTheLevel()
    {
        Debug.Log("restart level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void GoNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void ControlFinishLevelPage()
    {
        DataController dataController = new DataController();
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex - 2;
        dataController.SaveCompletedLevelCounter(nextLevelIndex);
        levelFinishTransitionControllerAnimator.SetBool("isLevelFinished", true);
    }
}
