using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Portal Input Paramaters")]

    [Header("Initializations")]
    protected CharacterAIMovementController characterAIMovementController;
    protected CompositeCollider2D midGroundColl;
    public GameObject portalPrefab;

    [Header("Portal Control Parameters")]
    protected int layerMaskOnlyPortalPlacePlatform = 1 << 13;
    protected int activePortalCount = 0;
    protected GameObject inPortal;
    protected GameObject outPortal;
    [HideInInspector] public Vector2 outPortalGravityDir;

    void Awake()
    {
        midGroundColl = GameObject.Find("TilemapMidground").GetComponent<CompositeCollider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosV3 = Input.mousePosition;
            mousePosV3.z = 10;
            mousePosV3 = Camera.main.ScreenToWorldPoint(mousePosV3);
            Vector2 mousePosV2 = new Vector2(mousePosV3.x, mousePosV3.y);

            RaycastHit2D[] hit4Dir = new RaycastHit2D[4];
            float[] hit4DirDistances = new float[4];
            hit4Dir[0] = Physics2D.Raycast(mousePosV2, Vector2.right, Mathf.Infinity, layerMaskOnlyPortalPlacePlatform);
            hit4Dir[1] = Physics2D.Raycast(mousePosV2, Vector2.left, Mathf.Infinity, layerMaskOnlyPortalPlacePlatform);
            hit4Dir[2] = Physics2D.Raycast(mousePosV2, Vector2.up, Mathf.Infinity, layerMaskOnlyPortalPlacePlatform);
            hit4Dir[3] = Physics2D.Raycast(mousePosV2, Vector2.down, Mathf.Infinity, layerMaskOnlyPortalPlacePlatform);
            for (int i = 0; i < 4; i++) { hit4DirDistances[i] = (hit4Dir[i].collider != null) ? hit4Dir[i].distance : 100f; }

            int smallestHitIndex = 0;
            for (int k = 1; k < 4; k++)
            {
                if (hit4DirDistances[smallestHitIndex] > hit4DirDistances[k])
                {
                    smallestHitIndex = k;
                }
            }
            if (hit4DirDistances[smallestHitIndex] <= 0.5f && !midGroundColl.OverlapPoint(mousePosV2))
            {
                RaycastHit2D closestHit = hit4Dir[smallestHitIndex];
                Vector2 closestHitNormal = hit4Dir[smallestHitIndex].normal;
                float closestHitDistance = closestHit.distance;
                Vector3 clonedPortalPosition = new Vector3(mousePosV2.x + (closestHitNormal.x * -closestHitDistance), mousePosV2.y + (closestHitNormal.y * -closestHitDistance), 0);
                Quaternion clonedPortalRotation;
                if (smallestHitIndex == 0) { outPortalGravityDir = Vector2.left; clonedPortalRotation = Quaternion.Euler(0, 0, 90); }
                else if (smallestHitIndex == 1) { outPortalGravityDir = Vector2.right; clonedPortalRotation = Quaternion.Euler(0, 0, -90); }
                else if (smallestHitIndex == 2) { outPortalGravityDir = Vector2.down; clonedPortalRotation = Quaternion.Euler(0, 0, 180); }
                else { outPortalGravityDir = Vector2.up; clonedPortalRotation = Quaternion.Euler(0, 0, 0); }

                if (activePortalCount == 0)
                {
                    inPortal = Instantiate(portalPrefab, clonedPortalPosition, clonedPortalRotation);
                    inPortal.name = "InPortal";
                    activePortalCount++;
                }
                else if (activePortalCount == 1)
                {
                    outPortal = Instantiate(portalPrefab, clonedPortalPosition, clonedPortalRotation);
                    outPortal.name = "OutPortal";
                    activePortalCount++;
                    characterAIMovementController = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterAIMovementController>();
                    characterAIMovementController.destinationPos = inPortal.transform.position;
                    characterAIMovementController.activatePathFinder = true;
                }
                else
                {
                    GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
                    for (int i = 0; i < portals.Length; i++) { Destroy(portals[i]); }
                    activePortalCount = 0;
                }
            }
        }
    }
}
