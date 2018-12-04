using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Item",
    menuName = "Cat's Ship/Asset Storage/Item",
    order = 0)]
[Serializable]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Transform content;
    public string mountPath;
    public int requiredGems;
}
