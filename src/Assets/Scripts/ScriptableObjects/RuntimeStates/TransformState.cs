using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TransformState",
    menuName = "Cat's Ship/Runtime States/Transform State",
    order = 0)]
public class TransformState : ScriptableObject
{
    public Vector3 position;
}
