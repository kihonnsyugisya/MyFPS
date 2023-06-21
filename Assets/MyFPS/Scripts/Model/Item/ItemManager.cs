using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDataBase itemDataBase;
    public GunItemData[] gunItemSlot = new GunItemData[2];
    public int currentGunItem = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GunItemData GetGunItemData(int itemID)
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

    public void UseItem()
    { }
}
