using System;
using System.Collections;
using UnityEngine;

public class CompassPointer : MonoBehaviour
{
    public TransformState target;
    public Transform orTargetTransform;
    public TransformState agent;
    public float appearOnDistance;
    public float orbitRadius;

    private bool isShown = false;
    private Transform icon;
    private Vector3 staticTargetPos;

    void Start()
    {
        icon = transform.Find("Icon");
        if (!orTargetTransform)
        {
            return;
        }

        staticTargetPos = agent.position;
        if (!target)
        {
            if (!orTargetTransform)
            {
                return;
            }

            staticTargetPos = orTargetTransform.position;
        }
    }

    void LateUpdate()
    {
        if (!agent)
        {
            return;
        }

        var targetPos = staticTargetPos;
        if (!target)
        {
            if (!orTargetTransform)
            {
                return;
            }

            targetPos = staticTargetPos;
        }

        var shouldShow = Vector3.Distance(targetPos, agent.position) > appearOnDistance;
        if (shouldShow != isShown)
        {
            ToggleVisuals(shouldShow);
            isShown = shouldShow;
        }

        var offsetFromAgent = targetPos - agent.position;
        var clampedOffset = Vector3.ClampMagnitude(offsetFromAgent, orbitRadius);
        transform.position = agent.position + clampedOffset;
        transform.rotation = Quaternion.LookRotation(transform.forward, offsetFromAgent.normalized);
        icon.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    public void ToggleVisuals(bool show)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(show);
        }
    }
}
