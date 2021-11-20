using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CharacterAIMovementController : MonoBehaviour
{
    [Header("Initializations")]
    protected Seeker seeker;
    protected Rigidbody2D rb;
    protected Transform destination;
    protected CharacterGravityController characterGravityController;

    [Header("Path Input Parameters")]
    [SerializeField] private float minDistanceToReachToDes = 0.5f;
    [SerializeField] private float minJumpDistanceToCancelMov = 2f;

    [Header("Path Control Paramaters")]
    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;
    protected Vector2 characterBottomPos;
    protected Vector2 destinationPos;
    protected float gravityAxisCharacter;
    protected bool isGravityVer;
    [SerializeField] protected bool canCharacterMoveToDes = true;
    public float movDirConstant;

    void Start()
    {
        destination = GameObject.Find("Destination").transform;
        characterGravityController = GetComponent<CharacterGravityController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("MovementWithPath", 2f, 0.1f);
    }

    void MovementWithPath()
    {
        canCharacterMoveToDes = true;
        movDirConstant = 0;
        characterBottomPos = rb.position;
        destinationPos = destination.position;
        isGravityVer = characterGravityController.isGravityVer;
        gravityAxisCharacter = (isGravityVer) ? rb.position.y : rb.position.x;
        seeker.StartPath(rb.position, destination.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            PathMovementAvailabilityCheck();
            CharacterPathMovementControl();
        }
    }

    void PathMovementAvailabilityCheck()
    {
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            float gravityAxisWayPoint = (isGravityVer) ? path.vectorPath[i].y : path.vectorPath[i].x;
            if (gravityAxisWayPoint - gravityAxisCharacter > minJumpDistanceToCancelMov)
            {
                canCharacterMoveToDes = false;
                break;
            }
        }
    }

    void CharacterPathMovementControl()
    {
        if (canCharacterMoveToDes && (characterBottomPos - destinationPos).magnitude > minDistanceToReachToDes)
        {
            Vector2 firstPathPoint = path.vectorPath[1];
            Vector2 firstPathVec = firstPathPoint - characterBottomPos;
            movDirConstant = (isGravityVer) ? (Vector2.Dot(firstPathVec, Vector2.right) > 0) ? 1f : -1f
                : (Vector2.Dot(firstPathVec, Vector2.up) > 0) ? -1f : 1f;
        }
    }
}
