using System;
using UnityEngine;

public class StretchingObject : MonoBehaviour
{
    public float minLength;
    public float maxLength;

    private Vector3 initialScale;
    private float targetLength;

    void OnEnable()
    {
        this.initialScale = transform.localScale;
    }

    void Update()
    {
        if (this.isActiveAndEnabled)
        {
            this.targetLength = UnityEngine.Random.Range(this.minLength, this.maxLength);

            transform.localScale = new Vector3(
                this.initialScale.x,
                this.targetLength,
                this.initialScale.z
            );
        }
    }
}
