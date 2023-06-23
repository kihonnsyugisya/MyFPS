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

    private ItemSourceData GetItemSourceData(Item item)
    {
        List<ItemSourceData> result = item.itemType switch
        {
            ItemType.GUN => new(itemDataBase.gunItemDataBase),
            _ => new(),
        };
        return result[item.itemId];
    }


    private GunItemData GetGunItemData(int itemID)
    {
        Debug.Log("get item");
        return itemDataBase.gunItemDataBase[itemID];
    }

    public void GetItem(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.GUN:
                gunItemSlot[currentGunItem] = GetGunItemData(item.itemId);
                break;
        }
    }

    public void DispItemInfoPlate(Transform parent,Item item)
    {
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
            DispItemInfoPlate(other.transform,item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Item")) return;
        currentDispItemInfoPlateID = null;
        UnDispItemInfoPlate();

    }
}
