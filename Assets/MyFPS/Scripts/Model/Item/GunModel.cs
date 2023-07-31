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
    //[HideInInspector] public List<GunItem> gunitemHolder = new();
    //[HideInInspector] public ReactiveCollection<GunItemData> gunItemSlot = new();
    [HideInInspector] public BoolReactiveProperty canReload = new(false);
    [HideInInspector]
    public Dictionary<BulletType, IntReactiveProperty> bulletHolder = new()
    {
        { BulletType.Short, new IntReactiveProperty(10) },
        { BulletType.Long, new IntReactiveProperty(2) },
        { BulletType.Shot, new IntReactiveProperty(8) },
    };
    [HideInInspector] public Dictionary<int, GunItem> handItemDic = new();
    [HideInInspector] public Dictionary<int, GunItem> shoulderItemDic = new();

    [HideInInspector] public ReactiveDictionary<WeaponPoint,GunItemData> gunItemDataSlot = new();
    [HideInInspector] public Dictionary<WeaponPoint, GunItem> gunItemSlot = new();


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
    private void ShareShowWeapon(bool isCarry, bool isChange ,int getItemID, int getItemStageID, int hasItemStageID , Vector3 itemPos)
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
        }

        targetDic[getItemID].gameObject.SetActive(true);
        targetDic[getItemID].stageId = getItemStageID;
    }

    public void GetGunItem(GunItem getGunItem,GunItemData getGunItemData)
    {

        if (gunItemSlot.Count == 2)
        {
            photonView.RPC(nameof(ShareShowWeapon),RpcTarget.All,false,true,getGunItem.itemId,getGunItem.stageId,GetCurrentGunItem().stageId,getGunItem.transform.position);
            //gunitemHolder[currentGunItemSlotIndex] = getGunItem;
            //gunItemSlot[currentGunItemSlotIndex] = getGunItemData;
            gunItemSlot[WeaponPoint.Hand] = getGunItem;
            gunItemDataSlot[WeaponPoint.Hand] = getGunItemData;
        }
        else if (gunItemSlot.Count == 1)
        {
            photonView.RPC(nameof(ShareShowWeapon), RpcTarget.All, true,false,getGunItem.itemId,getGunItem.stageId,404,Vector3.zero);
            //gunitemHolder.Add(getGunItem);
            //gunItemSlot.Add(getGunItemData);
            gunItemSlot.Add(WeaponPoint.Shoulder, getGunItem);
            gunItemDataSlot.Add(WeaponPoint.Shoulder, getGunItemData);
        }
        else
        {
            photonView.RPC(nameof(ShareShowWeapon), RpcTarget.All, false,false,getGunItem.itemId,getGunItem.stageId,404,Vector3.zero);
            //gunitemHolder.Add(getGunItem);
            //gunItemSlot.Add(getGunItemData);
            gunItemSlot.Add(WeaponPoint.Hand, getGunItem);
            gunItemDataSlot.Add(WeaponPoint.Hand,getGunItemData);
        }
        hasHandWeapon.Value = true;
    }

    public GunItemData GetCurrentGunItemData()
    {
        return gunItemDataSlot[WeaponPoint.Hand];
    }

    public GunItem GetCurrentGunItem()
    {
        return gunItemSlot[WeaponPoint.Hand];
    }

    public void SwitchWeapon()
    {
        var dataTemp = gunItemDataSlot[WeaponPoint.Hand];
        gunItemDataSlot[WeaponPoint.Hand] = gunItemDataSlot[WeaponPoint.Shoulder];
        gunItemDataSlot[WeaponPoint.Shoulder] = dataTemp;

        var itemTemp = gunItemSlot[WeaponPoint.Hand];
        gunItemSlot[WeaponPoint.Hand] = gunItemSlot[WeaponPoint.Shoulder];
        gunItemSlot[WeaponPoint.Shoulder] = itemTemp;
        photonView.RPC(nameof(ShareSwitchWeapon), RpcTarget.All, gunItemSlot[WeaponPoint.Hand].itemId, gunItemSlot[WeaponPoint.Shoulder].itemId);
    }

    [PunRPC]
    private void ShareSwitchWeapon(int handItemID, int shoulderItemID)
    {
        handItemDic[shoulderItemID].gameObject.SetActive(false);
        handItemDic[handItemID].gameObject.SetActive(true);
        shoulderItemDic[handItemID].gameObject.SetActive(false);
        shoulderItemDic[shoulderItemID].gameObject.SetActive(true);
    }

    private int GetShoulderItemIndex(int currentItemIndex)
    {
        return currentItemIndex == 0 ? 1 : 0;
    }

    public void ReloadGun()
    {
        BulletType bulletType = gunItemDataSlot[WeaponPoint.Hand].bulletType;
        int needBullets = gunItemDataSlot[WeaponPoint.Hand].magazineSize - gunItemSlot[WeaponPoint.Hand].magazineSize.Value;
        if (needBullets > bulletHolder[bulletType].Value)
        {
            gunItemSlot[WeaponPoint.Hand].magazineSize.Value += bulletHolder[bulletType].Value;
            bulletHolder[bulletType].Value = 0;
        }
        else
        {
            bulletHolder[bulletType].Value -= needBullets;
            gunItemSlot[WeaponPoint.Hand].magazineSize.Value += needBullets;
        }
    }

    public void CheckCanReload()
    {
        GunItemData currentGunData = gunItemDataSlot[WeaponPoint.Hand];
        GunItem currentGunItem = gunItemSlot[WeaponPoint.Hand];
        if (bulletHolder[currentGunData.bulletType].Value > 0 && currentGunItem.magazineSize.Value < currentGunData.magazineSize)
        {
            canReload.Value = true;
        }
        else canReload.Value = false;
    }

}

public enum WeaponPoint
{
    Hand,Shoulder
}