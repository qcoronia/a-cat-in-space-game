using System;
using UnityEngine;

// Need to resort to code since animation has buggy rotation
public class FlailingLimb : MonoBehaviour
{
    public float currentIntensity;
    public bool flipAngle;
    public float flailAngleMin;
    public float flailAngleMax;

    [Range(0f, 360f)]
    public float defaultOffset = 0f;

    [Space]
    public IntensityProfile[] profiles;

    private float middleAngle;
    private float angleSize;
    private float loopCounter;
    private float offsetAngle;

    void Start()
    {
        middleAngle = Mathf.Lerp(flailAngleMin, flailAngleMax, 0.5f);
        angleSize = Math.Abs(flailAngleMin - flailAngleMax);

        offsetAngle = defaultOffset;
    }

    void Update()
    {
        var intensityForThisFrame = currentIntensity * Time.deltaTime;
        var angleStep = Mathf.Repeat(loopCounter + offsetAngle, 360f);
        offsetAngle = 0;
        var actualStep = angleStep + intensityForThisFrame;
        loopCounter = Mathf.Repeat(actualStep, 360f);
        var sine = Mathf.Sin(loopCounter) * (flipAngle ? -1 : 1);
        var scaledSine = sine * (angleSize * 0.5f);
        var actualOffset = middleAngle + scaledSine;

        transform.localRotation = Quaternion.AngleAxis(actualOffset, Vector3.forward);
    }

    public void SetOffset(float newOffsetAngle)
    {
        offsetAngle = newOffsetAngle;
    }

    public void SetMovementIntensity(float intensity)
    {
        currentIntensity = intensity;
    }

    public void SetProfile(string name)
    {
        for (var i = 0; i < profiles.Length; i++)
        {
            if (profiles[i].name == name)
            {
                currentIntensity = profiles[i].intensity;
                break;
            }
        }
    }
}

[Serializable]
public class IntensityProfile
{
    public string name;
    public float intensity;
}
