using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public InputState inputState;

    public float speed;
    public float headingSpeed;
    public float maxMovementStrength;
    public float maxSpeed;
    [Range(0f, 1f)]
    public float sidewaysCancelFactor;

    private Rigidbody2D rb;
    private Vector3 right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        right = Vector3.right;
    }

    void FixedUpdate()
    {
        if (!inputState.IsEnabled)
        {
            return;
        }

        if (!inputState.HasInput)
        {
            return;
        }

        rb.angularVelocity = 0f;

        var value = inputState.JoyStick.value;

        var movementStrength = Mathf.Clamp((value).magnitude, 0f, maxMovementStrength);
        var force = movementStrength / maxMovementStrength;
        var direction = (value).normalized;
        rb.AddForce(direction * (force * speed));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        var sidewaysVelocity = Vector3.Project(rb.velocity, transform.right);
        rb.AddForce(sidewaysVelocity * -sidewaysCancelFactor);

        rb.angularVelocity = 0f;

        var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, headingSpeed * Time.deltaTime);
    }
}
