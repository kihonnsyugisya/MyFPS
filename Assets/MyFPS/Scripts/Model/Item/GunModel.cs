using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GunModel : MonoBehaviour
{

    [SerializeField] private int bullerFlyingDistance = 500;
    public RectTransform AimPoint;
    public Transform shoulderWeaponPoint;
    public Transform handWeaponPoint;

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



    // Start is called before the first frame update
    void Start()
    {
        
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
            GameObject bullet = Instantiate(gunItem.bulletObj, gunItem.gunPoint.position, Quaternion.identity);
            //bullet.GetComponent<Bullet>().playerID =
            bullet.GetComponent<Rigidbody>().AddForce(bullerFlyingDistance * gunItemData.atkPoint * ((GetAimPoint() - gunItem.gunPoint.position).normalized));
            gunItem.magazineSize.Value--;
        }
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

    public void ShowWeapon(GunItem gunItem, bool isCarry)
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

    public void GetGunItem(GunItem getGunItem,GunItemData getGunItemData)
    {
        if (gunItemSlot.Count == 2)
        {
            ShowWeapon(getGunItem, false);
            gunitemHolder[currentGunItemSlotIndex] = getGunItem;
            gunItemSlot[currentGunItemSlotIndex] = getGunItemData;
        }
        else if (gunItemSlot.Count == 1)
        {
            ShowWeapon(getGunItem, true);
            gunitemHolder.Add(getGunItem);
            gunItemSlot.Add(getGunItemData);
        }
        else
        {
            ShowWeapon(getGunItem, false);
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
