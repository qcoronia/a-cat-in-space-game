using System.Collections;
using UnityEngine;

public static class UnityHelpers
{
    public static Vector2 ToVector2 (this Vector3 self)
    {
        return new Vector2 (self.x, self.y);
    }

    public static Vector3 ToVector3 (this Vector2 self, float z = 0f)
    {
        return new Vector3 (self.x, self.y, z);
    }

    public static Transform GetTransformByTag (string tag)
    {
        var obj = GameObject.FindWithTag (tag);
        if (obj)
        {
            return obj.transform;
        }

        return null;
    }

    public static int LoopAround (int value, int min, int max)
    {
        if (value < min)
        {
            return max;
        }

        if (value > max)
        {
            return min;
        }

        return value;
    }

    public static int RandomIndex (int from, int to)
    {
        var rangeSize = Mathf.Abs (from - to);
        var randomFloat = Mathf.Clamp (Random.value * rangeSize, 0f, rangeSize - 1f);
        return from + Mathf.FloorToInt (randomFloat);
    }

    public static void ClearChildren (this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild (i);
            if (child == null)
            {
                continue;
            }

            GameObject.Destroy (child.gameObject);
        }
    }

    public static void SetLayer (this Transform transform, int layer, bool isRecursive = false)
    {
        transform.gameObject.layer = layer;
        if (!isRecursive)
        {
            return;
        }

        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild (i).SetLayer (layer);
        }
    }
}
