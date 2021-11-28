using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLevelController : MonoBehaviour
{
    [Header("Initializations")]
    private Rigidbody2D rb;
    private PortalPlacementController portalPlacementController;
    private CharacterMovementController characterMovementController;
    private Animator animator;
    private GameObject reachPoint;

    [Header("Level Control Parameters")]
    [HideInInspector] public int totalCollectableObjectsCounter; // When it equals to 1 the ReachPoint should be activated.
    private int maxXaxis = 18;
    private int maxYaxis = 10;
    private bool isLevelFinished = false;

    void OnEnable()
    {
        totalCollectableObjectsCounter = GameObject.FindGameObjectsWithTag("CollectableObjects").Length + 1;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        portalPlacementController = GameObject.Find("PortalController").GetComponent<PortalPlacementController>();
        characterMovementController = GetComponent<CharacterMovementController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        reachPoint = GameObject.FindGameObjectWithTag("ReachPoint");
    }

    void FixedUpdate()
    {
        if ((Mathf.Abs(rb.position.x) > maxXaxis || Mathf.Abs(rb.position.y) > maxYaxis) || (portalPlacementController.isPortalQuotaFinished && GetComponent<CharacterPhysicsController>().isGrounded && !isLevelFinished))
        {
            RestartTheLevel();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "ReachPoint" && totalCollectableObjectsCounter == 1)
        {
            isLevelFinished = true;
            FinishTheLevel();
        }
        if (coll.gameObject.tag == "CollectableObjects")
        {
            Destroy(coll.gameObject);
            totalCollectableObjectsCounter = GameObject.FindGameObjectsWithTag("CollectableObjects").Length;
            if (totalCollectableObjectsCounter == 1) { ReachPointActivate(); }
        }
    }

    public void RestartTheLevel()
    {
        Debug.Log("restart level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void FinishTheLevel()
    {
        Debug.Log("finish level");
        characterMovementController.gravityDirectionModifier = Vector2.zero;
        rb.position = reachPoint.transform.position;
        animator.SetBool("isLevelCompleted", true);
    }

    public void ReachPointActivate()
    {
        Debug.Log("reachpoint activated");
    }
}
