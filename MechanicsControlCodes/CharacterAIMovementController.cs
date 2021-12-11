using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CharacterAIMovementController : MonoBehaviour
{
    [Header("Initializations")]
    private Seeker seeker;
    private Rigidbody2D rb;
    private CharacterMovementController characterMovementController;
    private PortalPlacementController portalPlacementController;

    [Header("Path Input Parameters")]
    [SerializeField] private float minDistanceToReachToDes = 0.4f;
    [SerializeField] private float minJumpDistanceToCancelMov = 0.8f;

    [Header("Path Control Paramaters")]
    private Path path;
    private Vector2 characterPos;
    private float gravityAxisCharacter;
    private bool isGravityVer;
    private bool canCharacterMoveToDes = true;
    [HideInInspector] public float movDirConstant;
    [HideInInspector] public Vector2 destinationPos;
    [HideInInspector] public bool activatePathFinder = false;
    [HideInInspector] public bool activatePathFinderIsGrounded = true;
    [HideInInspector] public bool afterNoLeftPortalCharacterReachPointTest = true;

    void Start()
    {
        characterMovementController = GetComponent<CharacterMovementController>();
        portalPlacementController = GameObject.Find("PortalController").GetComponent<PortalPlacementController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("MovementWithPath", 0f, 0.1f);
    }

    void MovementWithPath()
    {
        if (activatePathFinder && activatePathFinderIsGrounded)
        {
            canCharacterMoveToDes = true;
            movDirConstant = 0;
            characterPos = rb.position;
            isGravityVer = characterMovementController.isGravityVer;
            gravityAxisCharacter = (isGravityVer) ? rb.position.y : rb.position.x;
            seeker.StartPath(rb.position, destinationPos, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            PathMovementAvailabilityCheck();
            CharacterPathMovementControl();
        }
    }

    void PathMovementAvailabilityCheck()
    {
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            float gravityAxisWayPoint = (isGravityVer) ? path.vectorPath[i].y : path.vectorPath[i].x;
            if ((gravityAxisWayPoint - gravityAxisCharacter) * characterMovementController.gravityDirCorrecter > minJumpDistanceToCancelMov || portalPlacementController.inPortalNormalInverse != characterMovementController.gravityDirectionModifier)
            {
                Debug.Log("yüksek yer");
                canCharacterMoveToDes = false;
                break;
            }
        }
    }

    void CharacterPathMovementControl()
    {
        if (canCharacterMoveToDes && (characterPos - destinationPos).magnitude > minDistanceToReachToDes)
        {
            Vector2 firstPathPoint = (path.vectorPath.Count >= 2) ? path.vectorPath[1] : path.vectorPath[0];
            Vector2 firstPathVec = firstPathPoint - characterPos;
            movDirConstant = (isGravityVer) ? (Vector2.Dot(firstPathVec, Vector2.right) > 0) ? 1f : -1f
                : (Vector2.Dot(firstPathVec, Vector2.up) > 0) ? -1f : 1f;
        }
        else
        {
            if (!canCharacterMoveToDes)
            {
                GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
                for (int i = 0; i < portals.Length; i++) { Destroy(portals[i]); }
                portalPlacementController.isPortalPlacementActive = true;
                Debug.Log("Character can not move there.");
            }
            movDirConstant = 0;
            activatePathFinder = false;
            afterNoLeftPortalCharacterReachPointTest = false;
        }
    }
}
