using System;
using UnityEngine;

public class PlayerCostume : MonoBehaviour
{
    public Profile profile;
    public string[] mountPaths;

    void Start()
    {
        profile.OnItemAdded.AddListener(OnProfileItemAdded);
        profile.OnItemRemoved.AddListener(OnProfileItemRemoved);
        profile.OnRevert.AddListener(OnProfileRevert);

        profile.Items.ForEach(item =>
        {
            OnProfileItemAdded(item);
        });
    }

    public void OnProfileItemAdded(Item item)
    {
        var mount = transform.Find(item.mountPath) as Transform;
        if (mount == null)
        {
            return;
        }

        mount.ClearChildren();
        if (item.content == null)
        {
            return;
        }

        var appliedItem = Instantiate(item.content) as Transform;
        appliedItem.SetParent(mount);
        appliedItem.SetLayer(mount.gameObject.layer, true);
        appliedItem.localPosition = Vector3.zero;
        appliedItem.localRotation = Quaternion.identity;
        appliedItem.localScale = Vector3.one;
    }

    public void OnProfileItemRemoved(Item item)
    {
        var mount = transform.Find(item.mountPath) as Transform;
        if (mount == null)
        {
            return;
        }

        mount.ClearChildren();
    }

    public void OnProfileRevert()
    {
        for (var i = 0; i < mountPaths.Length; i++)
        {
            var mount = transform.Find(mountPaths[i]) as Transform;
            if (mount == null)
            {
                continue;
            }

            mount.ClearChildren();
        }

        profile.Items.ForEach(item =>
       {
           OnProfileItemAdded(item);
       });
    }
}
