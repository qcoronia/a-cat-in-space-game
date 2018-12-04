using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PrefabList",
    menuName = "Cat's Ship/Asset Storage/Prefab List",
    order = 0)]
public class PrefabList : ScriptableObject
{
    public List<PrefabInfo> Prefabs = new List<PrefabInfo>();
}

[Serializable]
public class PrefabInfo
{
    public string Code;
    public Transform Prefab;
}
