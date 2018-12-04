using System;
using UnityEngine;

public class DestroyWhenTooFar : MonoBehaviour
{
    public TransformState positionReferenceState;
    public bool fromStaticPosition;
    public Vector2 staticReferencePosition;
    public float maxDistance;

    void Update ()
    {
        var referencePosition = staticReferencePosition;
        if (!fromStaticPosition)
        {
            if (!positionReferenceState)
            {
                return;
            }

            referencePosition = positionReferenceState.position;
        }

        if (Vector2.Distance (referencePosition, transform.position) > maxDistance)
        {
            Destroy (gameObject);
        }
    }
}
