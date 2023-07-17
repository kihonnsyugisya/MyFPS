using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemManager : MonoBehaviour
{
    public ItemDataBase itemDataBase;
    public Transform shoulderWeaponPoint;
    public Transform handWeaponPoint;
    public BoolReactiveProperty hasHandWeapon = new(false);

    [SerializeField] private GameObject itemInfoPlateObj;

    [HideInInspector] public ReactiveCollection<GunItemData> gunItemSlot = new();
    [HideInInspector] public List<GunItem> gunitemHolder = new();
    [HideInInspector] public List<GameObject> dispItemPlates = new();
    [HideInInspector] public int currentGunItemSlotIndex = 0;
    [HideInInspector] public int bullets = 100;

    //???????????0?hand?1?shoulder?????


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


    private GunItemData GetGunItemData(int itemID)
    {
        Debug.Log("get item");
        GunItemData gunItemData = itemDataBase.gunItemDataBase[itemID];        
        return gunItemData;
    }

    public void GetItem(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.GUN:
                GunItem gunItem = item.GetComponent<GunItem>();
                gunItem.magazineSize ??= 30;
                if (gunItemSlot.Count == 2)
                {
                    ShowWeapon(gunItem, currentGunItemSlotIndex != 0);
                    gunitemHolder[currentGunItemSlotIndex] = gunItem;
                    gunItemSlot[currentGunItemSlotIndex] = GetGunItemData(item.itemId);
                }
                else if(gunItemSlot.Count == 1)
                {
                    ShowWeapon(gunItem, true);
                    gunitemHolder.Add(gunItem);
                    gunItemSlot.Add(GetGunItemData(item.itemId));
                }
                else
                {
                    ShowWeapon(gunItem, false);
                    gunitemHolder.Add(gunItem);
                    gunItemSlot.Add(GetGunItemData(item.itemId));
                }
                hasHandWeapon.Value = true;
                break;
        }
        UnDispItemInfoPlate();
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


    public void ShowWeapon(GunItem gunItem,bool isCarry)
    {
        Transform targetPoint;
        if (isCarry) targetPoint = shoulderWeaponPoint;
        else targetPoint = handWeaponPoint;

        if (targetPoint.childCount != 0) 
        {
            Transform hasWeapon;
            hasWeapon = handWeaponPoint.GetChild(0).transform;
            hasWeapon.position = gunItem.transform.position;
            hasWeapon.parent = null;
        }
        gunItem.transform.SetParent(targetPoint);
        gunItem.transform.localPosition = gunItem.transform.localEulerAngles = Vector3.zero;

    }

    public GunItemData GetGunItemData()
    {
        return gunItemSlot[currentGunItemSlotIndex];
    }

    public GunItem GetGunItem()
    {
        return gunitemHolder[currentGunItemSlotIndex];
    }

    public void SwitchWeapon()
    {
        Transform shoulderItem = shoulderWeaponPoint.GetChild(0);
        Transform handItem = handWeaponPoint.GetChild(0);
        shoulderItem.SetParent(handWeaponPoint);
        handItem.SetParent(shoulderWeaponPoint);
        shoulderItem.localPosition = shoulderItem.localEulerAngles = Vector3.zero;
        handItem.localPosition = handItem.localEulerAngles = Vector3.zero;
    }

}
