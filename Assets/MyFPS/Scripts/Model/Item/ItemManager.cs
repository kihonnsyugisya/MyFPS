using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDataBase itemDataBase;
    public GunModel gunModel;

    [SerializeField] private GameObject itemInfoPlateObj;

    [HideInInspector] public List<GameObject> dispItemPlates = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private ItemSourceData GetItemSourceData(Item item)
    {
        List<ItemSourceData> result = item.itemType switch
        {
            ItemType.GUN => new(itemDataBase.gunItemDataBase),
            _ => new(),
        };
        return result[item.itemId];
    }

    public void GetItem(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.GUN:
                GunItem gunItem = item.GetComponent<GunItem>();
                GunItemData gunItemData = GetGunItemData(item.itemId);
                gunModel.GetGunItem(gunItem,gunItemData);
                break;
        }
        UnDispItemInfoPlate();
    }

    private GunItemData GetGunItemData(int itemID)
    {
        GunItemData gunItemData = itemDataBase.gunItemDataBase[itemID];
        return gunItemData;
    }


    public void DispItemInfoPlate(Item item)
    {
        Transform parent = item.transform;
        Vector3 pos = parent.position;
        pos.y += 0.7f; 
        GameObject instance = Instantiate(itemInfoPlateObj,pos,Quaternion.identity,parent);
        dispItemPlates.Add(instance);
        if (instance.TryGetComponent(out ItemInfoPlate iip))
        {
            iip.itemIcon.sprite = GetItemSourceData(item).itemeIcon;
            iip.itemName.text = item.name;
            iip.pickUpButton.onClick.AddListener(()=> GetItem(item));
        }

    }

    public void UnDispItemInfoPlate()
    {
        currentDispItemInfoPlateID = null;
        foreach (var itemPlate in dispItemPlates) Destroy(itemPlate);
    }

    public void UseItem()
    { }

    private int? currentDispItemInfoPlateID;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Item")) return;
        if (other.TryGetComponent(out Item item))
        {
            if (currentDispItemInfoPlateID == other.GetInstanceID()) return;
            currentDispItemInfoPlateID = other.GetInstanceID();
            DispItemInfoPlate(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Item")) return;       
        UnDispItemInfoPlate();

    }



}


