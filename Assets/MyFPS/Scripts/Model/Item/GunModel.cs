using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModel : MonoBehaviour
{

    [SerializeField] private int bullerFlyingDistance = 500;
    public RectTransform AimPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnclickGunShoot(GunItemData gunItemData, GunItem gunItem)
    {
        Debug.Log(gunItemData.magazineSize);
        Debug.Log(gunItemData.itemName);

        if (gunItem.magazineSize.Value <= 0)
        {
            OnpointerUpGunShoot(gunItem);
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

    public void OnpointerUpGunShoot(GunItem gunItem)
    {
        if (!gunItem.gunEffect) return;
        gunItem.gunEffect.SetActive(false);

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
}
