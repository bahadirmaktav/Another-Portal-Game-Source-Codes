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
        levelPauseTransitionController = GameObject.Find("LevelPauseTransitionController");
        levelPauseTransitionControllerAnimator = levelPauseTransitionController.GetComponent<Animator>();
        levelPauseTransitionController.SetActive(false);
        levelFinishTransitionController = GameObject.Find("LevelFinishTransitionController");
        levelFinishTransitionControllerAnimator = levelFinishTransitionController.GetComponent<Animator>();
        levelFinishTransitionController.SetActive(false);
        leftStonesText = GameObject.Find("LeftStonesText").GetComponent<Text>();
        leftStonesText.text = (GameObject.FindGameObjectsWithTag("CollectableObjects").Length).ToString();
    }

    public void GoToMainPage()
    {

        SceneManager.LoadScene("MainPageScene", LoadSceneMode.Single);
    }

    public void PauseTheLevel()
    {
        levelPauseTransitionController.SetActive(true);
        levelPauseTransitionControllerAnimator.SetBool("isContinued", false);
        levelPauseTransitionControllerAnimator.SetBool("isPaused", true);
    }

    public void ContinueTheLevel()
    {
        levelPauseTransitionController.SetActive(false);
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
        levelFinishTransitionController.SetActive(true);
        levelFinishTransitionControllerAnimator.SetBool("isLevelFinished", true);
    }
}
