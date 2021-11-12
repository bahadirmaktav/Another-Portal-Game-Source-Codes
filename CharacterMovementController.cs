using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : CharacterPhysicsController
{
    [Header("Movement Paramaters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpOffSpeed = 6f;

    [Header("Initializations")]
    public SpriteRenderer spriteRenderer;

    [Header("Control Paramaters")]
    protected bool isCharacterLookDirectionRight = true;
    protected float tempVelocityAxisValue;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void MovementInputControl()
    {
        tempVelocityAxisValue = Input.GetAxis("Horizontal") * speed;
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
