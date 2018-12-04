using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChaseMovement : MonoBehaviour
{
    // Parameters
    public float speed = 3f;
    public float speedDamping = 0.3f;
    public float turningSpeed = 3f;
    public float tooFarDistance = 10f;
    public float catchUpSpeed = 8f;
    public float targetPosIntervalError = 0.5f;

    // References
    private TransformState target;
    private Rigidbody2D rb;

    private Vector3 deltaTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var direction = CalculateHeading();
        var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            turningSpeed * Time.deltaTime
        );

        if (!target)
        {
            return;
        }

        if (Vector3.Distance(deltaTarget, target.position) > targetPosIntervalError)
        {
            deltaTarget = target.position;
            var distance = Vector3.Distance(transform.position, target.position);
            var error = targetPosIntervalError * 1.5f;
            if (distance < error)
            {
                var dir = (deltaTarget - transform.position).normalized;
                deltaTarget = target.position + (dir * targetPosIntervalError);
            }
        }
    }

    public void SetTarget(TransformState transformState)
    {
        target = transformState;
    }

    void FixedUpdate()
    {
        if (!rb)
        {
            return;
        }

        if (!target)
        {
            return;
        }

        var optimalSpeed = speed;
        var isTooFar = Vector2.Distance(transform.position.ToVector2(), deltaTarget) > tooFarDistance;
        if (isTooFar)
        {
            optimalSpeed = catchUpSpeed;
        }

        var directionToTarget = GetDirectionToTarget();
        var targetPos = transform.position + (directionToTarget * optimalSpeed);
        targetPos = Vector2.Lerp(transform.position, targetPos, speedDamping * Time.deltaTime);

        rb.MovePosition(targetPos);
    }

    //void OnDrawGizmos()
    //{
    //    if (target)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(transform.position, deltaTarget);
    //    }

    //    if (rb)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawLine(transform.position, transform.position + rb.velocity.ToVector3());
    //    }

    //    var showAttemptTrajectory = true;
    //    if (showAttemptTrajectory)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 3f));
    //    }
    //}

    private Vector3 CalculateHeading()
    {
        var targetHeading = GetDirectionToTarget();

        return targetHeading;
    }

    private Vector3 GetDirectionToTarget()
    {
        if (target)
        {
            return (deltaTarget - transform.position).normalized;
        }
        else
        {
            return transform.forward;
        }
    }
}
