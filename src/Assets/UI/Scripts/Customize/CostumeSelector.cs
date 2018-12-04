using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CostumeSelector : MonoBehaviour
{

    public Profile profile;
    public ItemList itemList;

    public Transform catalog;
    public Transform catalogButtonPrefab;
    public Sprite catalogButtonLockedIcon;
    public Text profileGemsText;
    public GameObject unlockDialog;
    public Image unlockDialogImage;
    public Text unlockDialogCost;
    public Button unlockDialogUnlockButton;

    private Item tryToUnlockItem;

    void Start()
    {
        profileGemsText.text = profile.Gems.ToString();
        unlockDialog.SetActive(false);
        BuildCatalog();
    }

    void OnEnable()
    {
        profileGemsText.text = profile.Gems.ToString();
        unlockDialog.SetActive(false);
    }

    public void BuildCatalog()
    {
        var existingItemList = new List<Item>();
        for (var i = 0; i < itemList.items.Count; i++)
        {
            var item = itemList.items[i];
            if (item != null)
            {
                existingItemList.Add(item);
            }
        }

        existingItemList.ForEach(item =>
        {
            var isUnlocked = profile.UnlockedItems.Any(e => e.name == item.name);

            var button = Instantiate(catalogButtonPrefab);
            button.SetParent(catalog);
            button.localScale = Vector3.one;

            var btn = button.GetComponent<Button>();
            if (isUnlocked)
            {
                btn.onClick.AddListener(() =>
                {
                    SendMessageUpwards("OnCatalogItemSelected", item);
                });
            }
            else
            {
                btn.onClick.AddListener(() =>
                {
                    ShowUnlockDialog(item);
                });
                //btn.interactable = isUnlocked;
            }

            var icn = button.Find("Icon").GetComponent<Image>();
            icn.sprite = isUnlocked ? item.icon : catalogButtonLockedIcon;
        });

        var noOfItems = catalog.childCount;
        var noOfSpaces = Math.Max(0, noOfItems);
        var gridLayout = catalog.GetComponent<GridLayoutGroup>();
        var preferredWidth = (gridLayout.cellSize.x * noOfItems) + (gridLayout.spacing.x * noOfSpaces);
        var rectTransform = catalog.transform as RectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth);
    }

    /// <summary>
    /// Called via Broadcast from CatalogItemButton
    /// </summary>
    /// <param name="item"></param>
    public void OnCatalogItemSelected(Item item)
    {
        var profileItemsValue = new List<Item>(profile.Items);
        profile.Items.Clear();
        for (var i = 0; i < profileItemsValue.Count; i++)
        {
            var profileItem = profileItemsValue[i];
            if (profileItem != null)
            {
                profile.Items.Add(profileItem);
            }
        }

        if (profile.Items.Any(e => e.name == item.name))
        {
            profile.Items = profile.Items
                .Where(e => e.name != item.name)
                .ToList();
            profile.OnItemRemoved.Invoke(item);
        }
        else
        {
            if (profile.Items.Any(e => e.mountPath == item.mountPath))
            {
                profile.Items = profile.Items
                    .Where(e => e.mountPath != item.mountPath)
                    .ToList();
            }
            profile.Items.Add(item);
            profile.OnItemAdded.Invoke(item);
        }
    }

    public void ShowUnlockDialog(Item item)
    {
        var itemImage = this.itemList.items.FirstOrDefault(e => e.name == item.name);
        if (itemImage != null)
        {
            tryToUnlockItem = itemImage;
            unlockDialogImage.sprite = itemImage.icon;
            unlockDialogCost.text = itemImage.requiredGems.ToString();
            unlockDialogUnlockButton.interactable = itemImage.requiredGems <= profile.Gems;
            unlockDialog.SetActive(true);
        }
    }

    public void CancelUnlockDialog()
    {
        tryToUnlockItem = null;
        unlockDialog.SetActive(false);
    }

    public void UnlockItem()
    {
        profile.Gems -= tryToUnlockItem.requiredGems;
        profile.Items.Add(tryToUnlockItem);
        profile.UnlockedItems.Add(tryToUnlockItem);
        profile.Apply();
        catalog.transform.ClearChildren();
        BuildCatalog();
        OnCatalogItemSelected(tryToUnlockItem);
        tryToUnlockItem = null;
        unlockDialog.SetActive(false);
    }
}