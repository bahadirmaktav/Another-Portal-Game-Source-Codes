using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : CharacterPhysicsController
{
    [Header("Initializations")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Movement Input Paramaters")]
    [SerializeField] private float speed = 3f;
    //private float jumpOffSpeed = 6f; //Jump ability will be not used in the game.

    [Header("Movement Control Paramaters")]
    private float tempVelocityAxisValue;

    private void Awake()
    {
        spriteRenderer = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void MovementInputControl()
    {
        tempVelocityAxisValue = characterAIMovementController.movDirConstant * speed;
        if (tempVelocityAxisValue == 0) { animator.SetBool("isRunning", false); }
        else { animator.SetBool("isRunning", true); }
        if (isGravityVer) { velocity.x = tempVelocityAxisValue; }
        else { velocity.y = tempVelocityAxisValue; }

        bool flipSprite = (spriteRenderer.flipX ? ((!isGravityVer) ? velocity.y > 0.1f : (velocity.x > 0.1f)) : ((!isGravityVer) ? (velocity.y < -0.1f) : (velocity.x < -0.1f)));
        if (flipSprite) { spriteRenderer.flipX = !spriteRenderer.flipX; }

        //Jump ability will be not used in the game.
        /*if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (isGravityVer) { velocity.y = gravityDirCorrecter * jumpOffSpeed; }
            else { velocity.x = gravityDirCorrecter * jumpOffSpeed; }

            platformNormal = ((isGravityVer) ? Vector2.up : Vector2.right) * gravityDirCorrecter;
        }*/
    }
}
