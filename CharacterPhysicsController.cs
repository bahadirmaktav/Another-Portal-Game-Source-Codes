/* 
                    2D Platformer Character Physics Controller
    --------------------------------------------------------------------------------------
    Can Do:    
                Horizontal movement
                Vertical movement
                Gravitiy control
                Movement on slopes with spesific angle range
    --------------------------------------------------------------------------------------
    Cant Do:    
                Cant falling on the slopes outside the angle range
                Some speed up when hit slopes under it (Not looking like an error.)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicsController : MonoBehaviour
{
    [Header("Physics Input Paramaters")]
    [SerializeField] private float minPlatformDistance = 0.01f;
    [SerializeField] private float minMovementDistance = 0.001f;
    [SerializeField] private float gravityConstantModifier = 1f;
    [SerializeField] private float maxPlatformAngleToMakeGrounded = 45;
    public Vector2 gravityDirectionModifier = new Vector2(0, -1);

    [Header("Physics Control Parameters")]
    protected Vector2 velocity;
    protected Vector2 platformNormal;

    [Header("Initializations")]
    protected ContactFilter2D contactFilter;
    protected Rigidbody2D rb;

    [Header("Status Flags")]
    [SerializeField] protected bool isGrounded = false;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void FixedUpdate()
    {
        isGrounded = false;
        velocity += gravityConstantModifier * Physics2D.gravity.magnitude * Time.deltaTime * gravityDirectionModifier;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 platformParallel = new Vector2(platformNormal.y, -platformNormal.x);

        Vector2 deltaMovement = deltaPosition.x * platformParallel;
        MovementPhysicsControl(deltaMovement, false);
        deltaMovement = deltaPosition.y * Vector2.up;
        MovementPhysicsControl(deltaMovement, true);
    }

    void Update()
    {
        MovementInputControl();
    }

    protected virtual void MovementInputControl() { }

    protected void MovementPhysicsControl(Vector2 deltaMovement, bool isVerticalMovementControlOn)
    {
        float moveDistance = deltaMovement.magnitude;
        if (moveDistance > minMovementDistance)
        {
            RaycastHit2D[] htiBuffer = new RaycastHit2D[2];
            rb.Cast(deltaMovement, contactFilter, htiBuffer, moveDistance + minPlatformDistance);
            if (htiBuffer[0].collider != null)
            {
                Vector2 currentPlatformNormal = htiBuffer[0].normal;
                if (currentPlatformNormal.y > Mathf.Sin(maxPlatformAngleToMakeGrounded))
                {
                    isGrounded = true;
                    if (isVerticalMovementControlOn)
                    {
                        platformNormal = currentPlatformNormal;
                        currentPlatformNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(velocity, currentPlatformNormal);
                if (projection < 0) { velocity -= projection * currentPlatformNormal; }
                deltaMovement = (moveDistance > htiBuffer[0].distance - minPlatformDistance) ? (htiBuffer[0].distance - minPlatformDistance) * deltaMovement.normalized : deltaMovement;

            }
        }
        rb.position = rb.position + deltaMovement;
    }
}
