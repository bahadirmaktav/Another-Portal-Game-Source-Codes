using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravityController : MonoBehaviour
{
    [Header("Initializations")]
    protected CharacterMovementController characterMovementController;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected CharacterAIMovementController characterAIMovementController;
    protected PortalController portalController;

    [Header("Gravity Control Parameters")]
    protected float portalDetectRayLength = 1f;
    protected int layerMaskOnlyPortal = 1 << 12;
    protected bool enteredPortal = false;
    public float temporaryOutPortalShiftConstant = 1f;

    protected void Awake()
    {
        characterMovementController = GetComponent<CharacterMovementController>();
        characterAIMovementController = GetComponent<CharacterAIMovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalController = GameObject.Find("PortalController").GetComponent<PortalController>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
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
            Vector2 outPortalPos = GameObject.Find("OutPortal").transform.position;
            Vector2 gravityDir = portalController.outPortalGravityDir;
            if (gravityDir == Vector2.up)
            {
                Debug.Log("buraya girmeli.up");
                characterMovementController.platformNormal = Vector2.zero;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipY = true;
            }
            else if (gravityDir == Vector2.down)
            {
                Debug.Log("buraya girmeli.down");
                characterMovementController.platformNormal = Vector2.zero;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipY = false;
            }
            else if (gravityDir == Vector2.right)
            {
                characterMovementController.platformNormal = Vector2.zero;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.flipY = true;
            }
            else if (gravityDir == Vector2.left)
            {
                characterMovementController.platformNormal = Vector2.zero;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.flipY = false;
            }
            rb.position = outPortalPos + gravityDir * temporaryOutPortalShiftConstant;
            characterMovementController.gravityDirectionModifier = gravityDir;
            enteredPortal = false;
        }



        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.up;
            characterMovementController.platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipY = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.down;
            characterMovementController.platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipY = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.right;
            characterMovementController.platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            spriteRenderer.flipY = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.left;
            characterMovementController.platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            spriteRenderer.flipY = false;
        }*/
    }
}
