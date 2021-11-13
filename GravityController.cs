using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    protected CharacterMovementController characterMovementController;

    void Awake()
    {
        characterMovementController = GameObject.Find("Character").GetComponent<CharacterMovementController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.up;
            characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            characterMovementController.spriteRenderer.flipY = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.down;
            characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            characterMovementController.spriteRenderer.flipY = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.right;
            characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            characterMovementController.spriteRenderer.flipY = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            characterMovementController.gravityDirectionModifier = Vector2.left;
            characterMovementController.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            characterMovementController.spriteRenderer.flipY = false;
        }
    }
}
