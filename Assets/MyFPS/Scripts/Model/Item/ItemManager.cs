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

    [HideInInspector] public List<GunItemData> gunItemSlot = new();
    [HideInInspector] public List<GameObject> dispItemPlates = new();
    [HideInInspector] public int currentGunItemSlotIndex = 0;
    [HideInInspector] public GunItem currentGunItem;

    //???????????0?hand?1?shoulder?????


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
                if (gunItemSlot.Count == 2)
                {
                    gunItemSlot[currentGunItemSlotIndex] = GetGunItemData(item.itemId);
                    ShowWeapon(item.transform, true);
                }
                else if(gunItemSlot.Count == 1)
                {
                    gunItemSlot.Add(GetGunItemData(item.itemId));
                    ShowWeapon(item.transform, true);
                }
                else
                {
                    gunItemSlot.Add(GetGunItemData(item.itemId));
                    ShowWeapon(item.transform, false);
                    SetCurrentGunItemData();
                }
                hasHandWeapon.Value = true;
                break;
        }
        UnDispItemInfoPlate();
    }

    private (int index,bool isOverRide) GetGunItemSlotState()
    {
        int index;
        bool isOverRide = false;
        if (gunItemSlot.Count == 2) index = 0;
        else if (!gunItemSlot[1]) index = 1;
        else {
            index = currentGunItemSlotIndex;
            isOverRide = true;
        }
        Debug.Log(index);
        return (index,isOverRide);
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


    public void ShowWeapon(Transform getItem,bool isCarry)
    {
        Transform targetPoint;
        if (isCarry) targetPoint = shoulderWeaponPoint;
        else targetPoint = handWeaponPoint;

        if (targetPoint.childCount != 0) 
        {
            Transform hasWeapon;
            hasWeapon = targetPoint.GetChild(0).transform;
            hasWeapon.position = getItem.position;
            hasWeapon.parent = null;
        }
        getItem.SetParent(targetPoint);
        getItem.localPosition = getItem.localEulerAngles = Vector3.zero;
        
    }

    public void SetCurrentGunItemData()
    {
        currentGunItem = handWeaponPoint.GetComponentInChildren<GunItem>();
        currentGunItem.magazineSize ??= gunItemSlot[0].magazineSize;
    }

}
