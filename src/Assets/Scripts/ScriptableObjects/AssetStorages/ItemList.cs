using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ItemList",
    menuName = "Cat's Ship/Asset Storage/Item List",
    order = 0)]
public class ItemList : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public Item GetByName(string name)
    {
        foreach (var item in items)
        {
            if (item.name == name)
            {
                return item;
            }
        }

        return null;
    }
}
