using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicsController : MonoBehaviour
{
    [Header("Physics Input Paramaters")]
    [SerializeField] private float gravityConstantModifier = 1f;
    [SerializeField] private float maxPlatformAngleToMakeGrounded = 45;
    [SerializeField] private float minPlatformDistance = 0.01f;
    private float minMovementDistance = 0.001f;
    private float minConstToBeNotGrounded = 3f;

    [Header("Physics Control Parameters")]
    public bool isGravityVer = true;
    protected Vector2 velocity;
    protected Vector2 platformNormal;
    protected Vector2 gravityDirectionModifier = new Vector2(0, -1);
    protected float gravityDirCorrecter = 1f;
    protected bool isGrounded = false;
    
    [Header("Initializations")]
    protected ContactFilter2D contactFilter;
    protected Rigidbody2D rb;

    protected void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected void FixedUpdate()
    {
        velocity += gravityConstantModifier * Physics2D.gravity.magnitude * Time.deltaTime * gravityDirectionModifier;
        isGrounded = (Mathf.Abs(VectorAxisForGravityDir(velocity)) > minConstToBeNotGrounded * gravityConstantModifier * Physics2D.gravity.magnitude * Time.deltaTime) ? false : isGrounded;
        platformNormal = (!isGrounded) ? Vector2.zero : platformNormal;
        isGravityVer = (gravityDirectionModifier.x != 0) ? false : true;
        gravityDirCorrecter = (gravityDirectionModifier.x + gravityDirectionModifier.y == -1) ? 1 : -1;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 platformParallel = new Vector2(platformNormal.y, -platformNormal.x) * gravityDirCorrecter;

        Vector2 deltaMovement = VectorAxisForMovementDir(deltaPosition) * platformParallel;
        MovementPhysicsControl(deltaMovement, false, isGravityVer, gravityDirCorrecter);
        deltaMovement = VectorAxisForGravityDir(deltaPosition) * ((isGravityVer) ? Vector2.up : Vector2.right);
        MovementPhysicsControl(deltaMovement, true, isGravityVer, gravityDirCorrecter);
    }

    protected virtual void Update() { }

    protected float VectorAxisForGravityDir(Vector2 inputVector) => (isGravityVer) ? inputVector.y : inputVector.x;
    protected float VectorAxisForMovementDir(Vector2 inputVector) => (isGravityVer) ? inputVector.x : inputVector.y;

    protected void MovementPhysicsControl(Vector2 deltaMovement, bool isGravityAxisMovementControlOn, bool isGravityVer, float gravityDirCorrecter)
    {
        float moveDistance = deltaMovement.magnitude;
        if (moveDistance > minMovementDistance)
        {
            RaycastHit2D[] htiBuffer = new RaycastHit2D[2];
            rb.Cast(deltaMovement, contactFilter, htiBuffer, moveDistance + minPlatformDistance);
            if (htiBuffer[0].collider != null)
            {
                Vector2 currentPlatformNormal = htiBuffer[0].normal;
                if (isGravityAxisMovementControlOn && VectorAxisForGravityDir(currentPlatformNormal) * gravityDirCorrecter > Mathf.Sin(maxPlatformAngleToMakeGrounded))
                {
                    isGrounded = true;
                    platformNormal = currentPlatformNormal;
                    if (isGravityVer) { currentPlatformNormal.x = 0; } else { currentPlatformNormal.y = 0; }
                }
                float projection = Vector2.Dot(velocity, currentPlatformNormal);
                if (projection < 0) { velocity -= projection * currentPlatformNormal; }
                deltaMovement = (moveDistance > htiBuffer[0].distance - minPlatformDistance) ? (htiBuffer[0].distance - minPlatformDistance) * deltaMovement.normalized : deltaMovement;
            }
        }
        rb.position = rb.position + deltaMovement;
    }
}
