using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RoamMovement : MonoBehaviour
{
    [Header("Speed")]
    public float speed = 3f;
    public float speedDamping = 0.3f;
    public float turningSpeed = 3f;
    public float turnAwaySpeed = 6f;

    [Header("Checkpoint")]
    public float checkpointArrivalError = 0.5f;
    public float checkpointTimeout = 5f;

    [Header("Roaming")]
    public float roamTravelDistance = 10f;

    [Header("References")]
    public TransformState roamNear;

    private Rigidbody2D rb;
    private Vector3 roamCheckpoint;
    private float checkpointTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNewRoamingPoint();
    }

    void Update()
    {
        var direction = CalculateHeading();
        var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        var actualTurningSpeed = Vector3.Dot(transform.up, direction) < 0.3f ?
            turnAwaySpeed :
            turningSpeed;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            actualTurningSpeed * Time.deltaTime
        );

        if (Vector3.Distance(roamCheckpoint, transform.position) < checkpointArrivalError)
        {
            SetNewRoamingPoint();
        }

        checkpointTime -= Time.deltaTime;
        if (checkpointTime <= 0f)
        {
            SetNewRoamingPoint();
        }
    }

    public void SetNewRoamingPoint()
    {
        var rocks = GameObject.FindGameObjectsWithTag("Rock");
        if (rocks.Length > 0)
        {
            var nearestRock = rocks[0].transform;
            for (var i = 0; i < rocks.Length; i++)
            {
                var thisDistance = Vector3.Distance(
                    transform.position,
                    rocks[i].transform.position
                );
                var currentNearest = Vector3.Distance(
                    transform.position,
                    nearestRock.position
                );
                if (thisDistance < currentNearest)
                {
                    nearestRock = rocks[i].transform;
                }
            }

            roamCheckpoint = nearestRock.position;
        }
        else
        {
            roamCheckpoint = roamNear.position + new Vector3(
                Random.Range(-1f, 1f) * roamTravelDistance,
                Random.Range(-1f, 1f) * roamTravelDistance,
                0f
            );
        }

        checkpointTime = checkpointTimeout;
    }

    public void SetRoamNearTarget(TransformState transformState)
    {
        roamNear = transformState;
    }

    void FixedUpdate()
    {
        if (!rb)
        {
            return;
        }

        if (!roamNear)
        {
            return;
        }

        var targetPos = transform.position + (transform.up * speed);
        targetPos = Vector2.Lerp(transform.position, targetPos, speedDamping * Time.deltaTime);

        rb.MovePosition(targetPos);
    }

    //void OnDrawGizmos()
    //{
    //    if (roamNear)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(transform.position, roamCheckpoint);
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
        if (roamNear)
        {
            return (roamCheckpoint - transform.position).normalized;
        }
        else
        {
            return transform.forward;
        }
    }
}
