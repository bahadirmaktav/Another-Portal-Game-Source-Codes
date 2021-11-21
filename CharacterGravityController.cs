using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravityController : MonoBehaviour
{
    [Header("Gravity Input Paramaters")]
    [SerializeField] private float temporaryOutPortalShiftConstant = 0.5f;

    [Header("Initializations")]
    protected CharacterMovementController characterMovementController;
    protected Rigidbody2D rb;
    protected CharacterAIMovementController characterAIMovementController;
    protected PortalController portalController;
    public GameObject characterPrefab;

    [Header("Gravity Control Parameters")]
    protected float portalDetectRayLength = 1f;
    protected int layerMaskOnlyPortal = 1 << 12;
    protected bool enteredPortal = false;
    protected bool exitedPortal = true;
    protected bool isDestroyOpStart = false;
    protected Vector2 initialCharacterPosHolder;

    protected void Awake()
    {
        characterMovementController = GetComponent<CharacterMovementController>();
        characterAIMovementController = GetComponent<CharacterAIMovementController>();
        portalController = GameObject.Find("PortalController").GetComponent<PortalController>();
        rb = GetComponent<Rigidbody2D>();
        initialCharacterPosHolder = rb.position;
    }

    protected void Update()
    {
        if (exitedPortal && (initialCharacterPosHolder - rb.position).magnitude > temporaryOutPortalShiftConstant * 3)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            exitedPortal = false;
        }
        if (isDestroyOpStart && (initialCharacterPosHolder - rb.position).magnitude > temporaryOutPortalShiftConstant * 3)
        {
            Destroy(this.gameObject);
        }

        bool activatePathFinder = characterAIMovementController.activatePathFinder;
        if (activatePathFinder && !enteredPortal)
        {
            bool isGravityVer = characterMovementController.isGravityVer;
            float gravityDirCorrecter = characterMovementController.gravityDirCorrecter;
            Vector2 rayDir = (isGravityVer) ? Vector2.down * gravityDirCorrecter : Vector2.left * gravityDirCorrecter;
            RaycastHit2D hit = Physics2D.Raycast(rb.position, rayDir, portalDetectRayLength, layerMaskOnlyPortal);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "InPortal")
                {
                    enteredPortal = true;
                }
            }
        }

        if (enteredPortal && !activatePathFinder)
        {
            rb.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Vector2 outPortalPos = GameObject.Find("OutPortal").transform.position;
            Vector2 gravityDir = portalController.outPortalGravityDir;
            GameObject teleportedCharacter = Instantiate(characterPrefab, outPortalPos + -gravityDir * temporaryOutPortalShiftConstant, Quaternion.Euler(0, 0, 0));
            SpriteRenderer spriteRenderer = teleportedCharacter.transform.GetChild(0).GetComponent<SpriteRenderer>();
            CharacterMovementController teleportedChaMovController = teleportedCharacter.GetComponent<CharacterMovementController>();
            if (gravityDir == Vector2.up)
            {
                //characterMovementController.platformNormal = Vector2.zero;
                teleportedCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipY = true;
            }
            else if (gravityDir == Vector2.down)
            {
                //characterMovementController.platformNormal = Vector2.zero;
                teleportedCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipY = false;
            }
            else if (gravityDir == Vector2.right)
            {
                //characterMovementController.platformNormal = Vector2.zero;
                teleportedCharacter.transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.flipY = true;
            }
            else if (gravityDir == Vector2.left)
            {
                //characterMovementController.platformNormal = Vector2.zero;
                teleportedCharacter.transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.flipY = false;
            }
            teleportedChaMovController.gravityDirectionModifier = gravityDir;
            enteredPortal = false;
            isDestroyOpStart = true;
            initialCharacterPosHolder = rb.position;
        }
    }
}
