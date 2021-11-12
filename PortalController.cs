using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    protected CharacterMovementController characterMovementController;

    void Awake()
    {
        characterMovementController = GameObject.Find("Character").GetComponent<CharacterMovementController>();
    }

    public void GravityToRight()
    {
        characterMovementController.gravityDirectionModifier = Vector2.right;
        characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        characterMovementController.spriteRenderer.flipY = true;
    }
    public void GravityToLeft()
    {
        characterMovementController.gravityDirectionModifier = Vector2.left;
        characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        characterMovementController.spriteRenderer.flipY = false;
    }
    public void GravityToUp()
    {
        characterMovementController.gravityDirectionModifier = Vector2.up;
        characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        characterMovementController.spriteRenderer.flipY = true;
    }
    public void GravityToDown()
    {
        characterMovementController.gravityDirectionModifier = Vector2.down;
        characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        characterMovementController.spriteRenderer.flipY = false;
    }
}
