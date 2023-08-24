using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDataBase itemDataBase;
    public GunModel gunModel;

    [HideInInspector] public GameObject itemInfoPlateObj;
    [HideInInspector] public List<ItemInfoPlate> dispItemPlateList = new();

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
        UnDispItemInfoPlate(item);
        //photonView.RPC(nameof(RemoveItem),RpcTarget.AllBuffered,item.stageId);
    }

    private GunItemData GetGunItemData(int itemID)
    {
        GunItemData gunItemData = itemDataBase.gunItemDataBase[itemID];
        return gunItemData;
    }


    public void DispItemInfoPlate(Item item)
    {
        if (!item.itemInfoPlate)
        {
            Transform parent = item.transform;
            Vector3 pos = parent.position;
            pos.y += 0.7f;
            GameObject instance = Instantiate(itemInfoPlateObj, pos, Quaternion.identity, parent);

            if (instance.TryGetComponent(out ItemInfoPlate iip))
            {
                item.itemInfoPlate = iip;
                iip.itemIcon.sprite = GetItemSourceData(item).itemeIcon;
                iip.itemName.text = item.name;
                iip.pickUpButton.onClick.AddListener(() => GetItem(item));
                iip.uniqueID = iip.gameObject.GetInstanceID();
                dispItemPlateList.Add(iip);
            }
        }


        if (item.itemInfoPlate.gameObject.activeSelf) return;
        else { 
            item.itemInfoPlate.gameObject.SetActive(true);
            dispItemPlateList.Add(item.itemInfoPlate);
        }
    }

    public void UnDispItemInfoPlate(Item item)
    {
        //foreach (var itemPlate in dispItemPlates) Destroy(itemPlate);
        if (!item.itemInfoPlate) return;
        item.itemInfoPlate.gameObject.SetActive(false);
        foreach (ItemInfoPlate itemPlate in dispItemPlateList.ToArray()) 
        { 
            if (item.itemInfoPlate.uniqueID == itemPlate.uniqueID) dispItemPlateList.Remove(itemPlate);
        }
    }

    public void UnDispItemInfoPlate()
    {
        foreach (var item in dispItemPlateList)
        {
            item.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("nanndeyanenn");
        if (!other.CompareTag("Item")) return;
        if (other.TryGetComponent(out Item item))
        {
            //if (currentDispItemInfoPlateID == other.GetInstanceID()) return;
            //currentDispItemInfoPlateID = other.GetInstanceID();
            DispItemInfoPlate(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Item")) return;
        if (other.TryGetComponent(out Item item))
        {
            UnDispItemInfoPlate(item);
        }

    }



}


