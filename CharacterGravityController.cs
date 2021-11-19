using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravityController : CharacterMovementController
{
    protected override void Update()
    {
        MovementInputControl();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravityDirectionModifier = Vector2.up;
            platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipY = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityDirectionModifier = Vector2.down;
            platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipY = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravityDirectionModifier = Vector2.right;
            platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            spriteRenderer.flipY = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gravityDirectionModifier = Vector2.left;
            platformNormal = Vector2.zero;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            spriteRenderer.flipY = false;
        }
    }
}
