using System;
using System.Collections;
using UnityEngine;

public class FixedLifetime : MonoBehaviour
{

    public float lifetime;

    void Start ()
    {
        Destroy (gameObject, lifetime);
    }
}
