using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDataBase itemDataBase;
    public GunItemData[] gunItemSlot = new GunItemData[2];
    public int currentGunItem = 0;
    private List<GameObject> dispItemPlates = new();

    [SerializeField] private GameObject itemInfoPlateObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private ItemSourceData GetItemSourceData(ItemType itemType,int itemID)
    {
        List<ItemSourceData> result = itemType switch
        {
            ItemType.GUN => new(itemDataBase.gunItemDataBase),
            _ => new(),
        };
        return result[itemID];
    }


    private GunItemData GetGunItemData(int itemID)
    {
        return itemDataBase.gunItemDataBase[itemID];
    }

    public void GetItem(ItemType itemType, int itemID)
    {
        switch (itemType)
        {
            case ItemType.GUN:
                gunItemSlot[currentGunItem] = GetGunItemData(itemID);
                break;
        }
    }

    public void DispItemInfoPlate(Transform item,ItemSourceData itemData)
    {
        Vector3 pos = item.position;
        pos.y += 0.7f; 
        GameObject instance = Instantiate(itemInfoPlateObj,pos,Quaternion.identity,item);
        dispItemPlates.Add(instance);
        if (instance.TryGetComponent(out ItemInfoPlate iip))
        {
            iip.itemIcon.sprite = itemData.itemeIcon;
            iip.itemName.text = itemData.itemName;
        }

    }

    public void UnDispItemInfoPlate()
    {
        foreach (var itemPlate in dispItemPlates) Destroy(itemPlate);
    }

    public void UseItem()
    { }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!other.CompareTag("Item")) return;
        if (other.TryGetComponent(out Item item))
        {
            ItemSourceData i = GetItemSourceData(item.itemType,item.itemId);
            DispItemInfoPlate(other.transform,i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Item")) return;
        UnDispItemInfoPlate();

    }
}
