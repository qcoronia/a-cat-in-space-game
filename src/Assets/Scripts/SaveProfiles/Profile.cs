using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Profile : ScriptableObject
{
    public string CharacterName = "Cat";
    public List<Item> Items = new List<Item>();
    public List<Item> UnlockedItems = new List<Item>();
    public List<Item> DefaultUnlockedItems = new List<Item>();
    public int ReachedLevel = 0;
    public int Gems = 0;

    [Header("References")]
    public ItemList ItemList;

    [Header("Events")]
    public UnityEvent OnRevert = new UnityEvent();
    public OnProfileItemChangedEvent OnItemAdded = new OnProfileItemChangedEvent();
    public OnProfileItemChangedEvent OnItemRemoved = new OnProfileItemChangedEvent();

    private ProfileModel Model = new ProfileModel();

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("profile"))
        {
            Model.UnlockedItems = DefaultUnlockedItems.Select(e => e.name).ToList();
            PlayerPrefs.SetString("profile", JsonUtility.ToJson(Model));
        }
        Revert();
    }

    public void Apply()
    {
        Model.CharacterName = CharacterName;
        Model.Items = Items.Select(e => e.name).ToList();
        Model.UnlockedItems = UnlockedItems.Select(e => e.name).ToList();
        Model.ReachedLevel = ReachedLevel;
        Model.Gems = Gems;
        PlayerPrefs.SetString("profile", JsonUtility.ToJson(Model));
    }

    public void Revert()
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("profile"), Model);
        CharacterName = Model.CharacterName;
        Items = ItemList.items.Where(e => Model.Items.Contains(e.name)).ToList();
        UnlockedItems = ItemList.items.Where(e => Model.UnlockedItems.Contains(e.name)).ToList();
        ReachedLevel = Model.ReachedLevel;
        Gems = Model.Gems;
        OnRevert.Invoke();
    }
}

public class ProfileModel
{
    public string CharacterName = "Cat";
    public List<string> Items = new List<string>();
    public List<string> UnlockedItems = new List<string>();
    public int ReachedLevel = 0;
    public int Gems = 0;
}

public class OnProfileItemChangedEvent : UnityEvent<Item>
{ }
