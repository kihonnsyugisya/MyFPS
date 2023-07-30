using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;

public class GunModel : MonoBehaviourPunCallbacks
{

    [SerializeField] private int bullerFlyingDistance = 500;

    [HideInInspector] public RectTransform AimPoint;
    [HideInInspector] public Transform shoulderWeaponPoint;
    [HideInInspector] public Transform handWeaponPoint;
    [HideInInspector] public BoolReactiveProperty hasHandWeapon = new(false);
    [HideInInspector] public List<GunItem> gunitemHolder = new();
    [HideInInspector] public ReactiveCollection<GunItemData> gunItemSlot = new();
    [HideInInspector] public int currentGunItemSlotIndex = 0;
    [HideInInspector] public BoolReactiveProperty canReload = new(false);
    [HideInInspector]
    public Dictionary<BulletType, IntReactiveProperty> bulletHolder = new()
    {
        { BulletType.Short, new IntReactiveProperty(10) },
        { BulletType.Long, new IntReactiveProperty(2) },
        { BulletType.Shot, new IntReactiveProperty(8) },
    };
    [HideInInspector] public Dictionary<int, Item> handItemDic = new();
    [HideInInspector] public Dictionary<int, Item> shoulderItemDic = new();

    private void Start()
    {
        foreach (Transform handItem in handWeaponPoint.transform)
        {
            Item item = handItem.GetComponent<Item>();
            handItemDic.Add(item.itemId, item);
        }
        foreach (Transform shoulderItem in shoulderWeaponPoint.transform)
        {
            Item item = shoulderItem.GetComponent<Item>();
            shoulderItemDic.Add(item.itemId, item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnclickGunShoot()
    {
        GunItem gunItem = GetCurrentGunItem();
        GunItemData gunItemData = GetCurrentGunItemData();
        if (gunItem.magazineSize.Value <= 0)
        {
            OnpointerUpGunShoot();
        }
        else
        {
            if (!gunItem.gunEffect.activeSelf) gunItem.gunEffect.SetActive(true);
            photonView.RPC(nameof(BulletFire),RpcTarget.All,gunItem.gunPoint.position,200f,GetAimPoint());
            gunItem.magazineSize.Value--;
        }
    }

    public GameObject testBullet;
    [PunRPC]
    private void BulletFire(Vector3 instantiatePoint,float atkPoint,Vector3 aimPoint)
    {
        GameObject bullet = Instantiate(testBullet, instantiatePoint, Quaternion.identity);        
        bullet.GetComponent<Rigidbody>().AddForce(bullerFlyingDistance * atkPoint * ((aimPoint - instantiatePoint).normalized));
    }


    public void OnpointerUpGunShoot()
    {
        if (!hasHandWeapon.Value) return;
        GunItem gunItem = GetCurrentGunItem();
        if (gunItem.gunEffect) gunItem.gunEffect.SetActive(false);
    }

    public Vector3 GetWorldPositionFromAimPoint()
    {
        //UI座標からスクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, AimPoint.position);

        //ワールド座標
        Vector3 result = Vector3.zero;

        //スクリーン座標→ワールド座標に変換
        RectTransformUtility.ScreenPointToWorldPointInRectangle(AimPoint, screenPos, Camera.main, out result);

        return result;
    }

    private Vector3 GetAimPoint()
    {
        Ray ray = new Ray(Camera.main.transform.position, (GetWorldPositionFromAimPoint() - Camera.main.transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // もしRayを投射して何らかのコライダーに衝突したら
        {
            Debug.Log(hit.collider.name + " " + hit.collider.gameObject.layer);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.blue, 10f);
            return hit.point;
        }
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.blue, 10f);
        return GetWorldPositionFromAimPoint();
    }

    [PunRPC]
    private void ShareShowWeapon(bool isCarry, int itemID, int stageID, Vector3 itemPos)
    {
        StageItemManager.RemoveItem(stageID);
        Dictionary<int, Item> targetDic;
        if (isCarry) targetDic = shoulderItemDic;
        else targetDic = handItemDic;

        if (gunItemSlot.Count == 2)
        {
            foreach (Transform handItem in handWeaponPoint.transform) handItem.gameObject.SetActive(false);
            Transform hasItem = StageItemManager.stageItemInfo[gunitemHolder[0].stageId].transform;
            hasItem.position = itemPos;
            hasItem.gameObject.SetActive(true);
        }

        targetDic[itemID].gameObject.SetActive(true);
        targetDic[itemID].stageId = stageID;
    }

    public void GetGunItem(GunItem getGunItem,GunItemData getGunItemData)
    {

        if (gunItemSlot.Count == 2)
        {
            photonView.RPC(nameof(ShareShowWeapon),RpcTarget.All,false,getGunItem.itemId,getGunItem.stageId,getGunItem.transform.position);
            gunitemHolder[currentGunItemSlotIndex] = getGunItem;
            gunItemSlot[currentGunItemSlotIndex] = getGunItemData;
        }
        else if (gunItemSlot.Count == 1)
        {
            photonView.RPC(nameof(ShareShowWeapon), RpcTarget.All, true, getGunItem.itemId,getGunItem.stageId,Vector3.zero);
            gunitemHolder.Add(getGunItem);
            gunItemSlot.Add(getGunItemData);
        }
        else
        {
            photonView.RPC(nameof(ShareShowWeapon), RpcTarget.All, false, getGunItem.itemId,getGunItem.stageId,Vector3.zero);
            gunitemHolder.Add(getGunItem);
            gunItemSlot.Add(getGunItemData);
        }
        hasHandWeapon.Value = true;
    }



    public GunItemData GetCurrentGunItemData()
    {
        return gunItemSlot[currentGunItemSlotIndex];
    }

    public GunItem GetCurrentGunItem()
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

        //ここで同期処理いる

    }

    [PunRPC]
    private void ShaerSwitchWeapon()
    {
        //これをやる
    }

    public void ReloadGun()
    {
        BulletType bulletType = gunItemSlot[currentGunItemSlotIndex].bulletType;
        int needBullets = gunItemSlot[currentGunItemSlotIndex].magazineSize - gunitemHolder[currentGunItemSlotIndex].magazineSize.Value;
        if (needBullets > bulletHolder[bulletType].Value)
        {
            gunitemHolder[currentGunItemSlotIndex].magazineSize.Value += bulletHolder[bulletType].Value;
            bulletHolder[bulletType].Value = 0;
        }
        else
        {
            bulletHolder[bulletType].Value -= needBullets;
            gunitemHolder[currentGunItemSlotIndex].magazineSize.Value += needBullets;
        }
    }

    public void CheckCanReload()
    {
        GunItemData currentGunData = gunItemSlot[currentGunItemSlotIndex];
        GunItem currentGunItem = gunitemHolder[currentGunItemSlotIndex];
        if (bulletHolder[currentGunData.bulletType].Value > 0 && currentGunItem.magazineSize.Value < currentGunData.magazineSize)
        {
            canReload.Value = true;
        }
        else canReload.Value = false;
    }

}
