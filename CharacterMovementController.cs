using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : CharacterPhysicsController
{
    [Header("Movement Paramaters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpOffSpeed = 6f;

    [Header("Initializations")]
    protected SpriteRenderer spriteRenderer;
    protected CharacterAIMovementController characterAIMovementController;

    [Header("Control Paramaters")]
    protected bool isCharacterLookDirectionRight = true;
    protected float tempVelocityAxisValue;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterAIMovementController = GetComponent<CharacterAIMovementController>();
    }

    protected void MovementInputControl()
    {
        tempVelocityAxisValue = characterAIMovementController.movDirConstant * speed;
        if (isGravityVer) { velocity.x = tempVelocityAxisValue; }
        else { velocity.y = tempVelocityAxisValue; }

        bool flipSprite = (spriteRenderer.flipX ? ((!isGravityVer) ? velocity.y > 0.1f : (velocity.x > 0.1f)) : ((!isGravityVer) ? (velocity.y < -0.1f) : (velocity.x < -0.1f)));
        if (flipSprite) { spriteRenderer.flipX = !spriteRenderer.flipX; }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (isGravityVer) { velocity.y = gravityDirCorrecter * jumpOffSpeed; }
            else { velocity.x = gravityDirCorrecter * jumpOffSpeed; }

            platformNormal = ((isGravityVer) ? Vector2.up : Vector2.right) * gravityDirCorrecter;
        }
    }
}
