using System;
using System.Collections.Generic;
using UnityEngine;

public class GemCompassManager : MonoBehaviour
{
    public GameObject compassPrefab;
    public float compassSize = 0.5f;

    void Start()
    {
        var gems = GameObject.FindObjectsOfType<Gem>();
        foreach (var gem in gems)
        {
            var compass = Instantiate(this.compassPrefab.transform);
            compass.SetParent(transform);
            compass.transform.localScale = Vector3.one * this.compassSize;
            var comp = compass.GetComponent<CompassPointer>();
            comp.orTargetTransform = gem.transform;
        }
    }
}
