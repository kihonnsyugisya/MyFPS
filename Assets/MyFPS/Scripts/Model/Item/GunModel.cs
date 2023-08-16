using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;

public class GunModel : MonoBehaviourPunCallbacks
{

    //[SerializeField] private int bullerFlyingDistance = 500;
    [SerializeField] private int bullerFlyingDistance = 5;


    [HideInInspector] public RectTransform AimPoint;
    [HideInInspector] public Transform shoulderWeaponPoint;
    [HideInInspector] public Transform handWeaponPoint;
    [HideInInspector] public BoolReactiveProperty hasHandWeapon = new(false);

    [HideInInspector] public Dictionary<int, GunItem> handItemDic = new();
    [HideInInspector] public Dictionary<int, GunItem> shoulderItemDic = new();

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
    [HideInInspector] public ObjectPools objectPool;

    private void Start()
    {
        foreach (Transform handItem in handWeaponPoint.transform)
        {
            GunItem item = handItem.GetComponent<GunItem>();
            handItemDic.Add(item.itemId, item);
        }
        foreach (Transform shoulderItem in shoulderWeaponPoint.transform)
        {
            GunItem item = shoulderItem.GetComponent<GunItem>();
            shoulderItemDic.Add(item.itemId, item);
        }
    }

    public void OnclickGunShoot()
    {
        GunItem gunItem = GetCurrentGunItem();
        GunItemData gunItemData = GetCurrentGunItemData();
        GunItem viewGunItem = GetCurrentViewGunItem();
        if (gunItem.magazineSize.Value <= 0)
        {
            OnpointerUpGunShoot();
        }
        else
        {
            if (!viewGunItem.gunEffect.activeSelf) viewGunItem.gunEffect.SetActive(true);
            photonView.RPC(nameof(BulletFire),RpcTarget.All,gunItem.gunPoint.position,200f,GetAimPoint(),(int)GetCurrentGunItemData().bulletType);
            gunItem.magazineSize.Value--;
        }
    }

    [PunRPC]
    private void BulletFire(Vector3 instantiatePoint,float atkPoint,Vector3 aimPoint,int bulletType)
    {
        //GameObject bullet = Instantiate(testBullet, instantiatePoint, Quaternion.identity);
        Bullet bullet = objectPool.GetPool(bulletType).Get();
        bullet.transform.localPosition = instantiatePoint;
        bullet.rigid.AddForce(bullerFlyingDistance * atkPoint * ((aimPoint - instantiatePoint).normalized));
    }


    public void OnpointerUpGunShoot()
    {
        if (!hasHandWeapon.Value) return;
        GunItem viewGunItem = GetCurrentViewGunItem();
        if (viewGunItem.gunEffect) viewGunItem.gunEffect.SetActive(false);
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
    private void ShareShowWeapon(bool isCarry, bool isChange ,int getItemID, int getItemStageID, int hasItemStageID ,int magazineSize , Vector3 itemPos)
    {
        StageItemManager.RemoveItem(getItemStageID);
        Dictionary<int, GunItem> targetDic;
        if (isCarry) targetDic = shoulderItemDic;
        else targetDic = handItemDic;

        if (isChange)
        {
            foreach (Transform handItem in handWeaponPoint.transform) handItem.gameObject.SetActive(false);

            Transform hasItem = StageItemManager.stageItemInfo[hasItemStageID].transform;
            hasItem.position = itemPos;
            hasItem.gameObject.SetActive(true);
            hasItem.GetComponent<GunItem>().magazineSize.Value = magazineSize;
        }

        targetDic[getItemID].gameObject.SetActive(true);
        targetDic[getItemID].stageId = getItemStageID;
    }

    public void GetGunItem(GunItem getGunItem,GunItemData getGunItemData)
    {
        if (gunItemSlot.Count == 2)
        {
            photonView.RPC(nameof(ShareShowWeapon),RpcTarget.All, false, true, getGunItem.itemId,　getGunItem.stageId, GetCurrentGunItem().stageId, GetCurrentGunItem().magazineSize.Value, getGunItem.transform.position);
            gunitemHolder[currentGunItemSlotIndex] = getGunItem;
            gunItemSlot[currentGunItemSlotIndex] = getGunItemData;
        }
        else if (gunItemSlot.Count == 1)
        {
            photonView.RPC(nameof(ShareShowWeapon), RpcTarget.All, true, false, getGunItem.itemId,getGunItem.stageId, 404, 404, Vector3.zero);
            gunitemHolder.Add(getGunItem);
            gunItemSlot.Add(getGunItemData);
        }
        else
        {
            photonView.RPC(nameof(ShareShowWeapon), RpcTarget.All, false, false, getGunItem.itemId, getGunItem.stageId, 404, 404, Vector3.zero);
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

    private GunItem GetCurrentShoulderGunItem()
    {
        int index = currentGunItemSlotIndex == 0 ?1 :0 ;
        return gunitemHolder[index];
    }

    private GunItem GetCurrentViewGunItem()
    {
        return handItemDic[GetCurrentGunItem().itemId];
    }

    public void SwitchWeapon()
    {
        photonView.RPC(nameof(ShareSwitchWeapon), RpcTarget.All, GetCurrentGunItem().itemId, GetCurrentShoulderGunItem().itemId);
    }

    [PunRPC]
    private void ShareSwitchWeapon(int handItemID, int shoulderItemID)
    {
        handItemDic[shoulderItemID].gameObject.SetActive(false);
        handItemDic[handItemID].gameObject.SetActive(true);
        shoulderItemDic[handItemID].gameObject.SetActive(false);
        shoulderItemDic[shoulderItemID].gameObject.SetActive(true);
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
