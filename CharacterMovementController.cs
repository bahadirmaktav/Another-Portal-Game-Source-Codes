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
        if (gravityDirectionModifier == Vector2.down) { velocity.x = tempVelocityAxisValue; }
        else if (gravityDirectionModifier == Vector2.left) { velocity.y = tempVelocityAxisValue; }
        else if (gravityDirectionModifier == Vector2.right) { velocity.y = tempVelocityAxisValue; }
        else { velocity.x = tempVelocityAxisValue; }

        bool flipSprite = (spriteRenderer.flipX ? ((gravityDirectionModifier.x != 0) ? velocity.y > 0.1f : (velocity.x > 0.1f)) : ((gravityDirectionModifier.x != 0) ? (velocity.y < -0.1f) :(velocity.x < -0.1f)));
        if (flipSprite) { spriteRenderer.flipX = !spriteRenderer.flipX; }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (gravityDirectionModifier == Vector2.down) { velocity.y = jumpOffSpeed; }
            else if (gravityDirectionModifier == Vector2.left) { velocity.x = jumpOffSpeed; }
            else if (gravityDirectionModifier == Vector2.right) { velocity.x = -jumpOffSpeed; }
            else { velocity.y = -jumpOffSpeed; }
            platformNormal = (gravityDirectionModifier.x != 0) ? Vector2.right : Vector2.up;
        }
    }
}
