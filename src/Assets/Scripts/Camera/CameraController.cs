using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public TransformState targetState;
    public float cameraDistance;
    public float chaseSpeed;

    void Start () { }

    void LateUpdate ()
    {
        if (!targetState)
        {
            return;
        }
        transform.position = new Vector3 (
            targetState.position.x,
            targetState.position.y,
            targetState.position.z - cameraDistance);
    }
}
