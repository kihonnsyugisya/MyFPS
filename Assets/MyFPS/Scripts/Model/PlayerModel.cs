using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using System;

public class PlayerModel : MonoBehaviour
{
    
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float walkAnimationSpeed = 2.2f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float runAnimatonSpeed = 1f;
    [SerializeField] private float rotateSpeed = 0.7f;
    [SerializeField] private float walkInputRange = 0.65f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private int bullerFlyingDistance = 500;

    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick rotateJoystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Transform eye;
    [SerializeField] private Transform Aim;
    public RectTransform AimPoint;

    [HideInInspector] public Animator animator;

    private new Rigidbody rigidbody;
    public bool isAiming = false;

    public float distToGround;
    public bool isGrounded;

    public ItemManager itemManager;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        jumpButton.OnClickAsObservable().Subscribe(_=> OnClickJumpButton()).AddTo(this);
    }

    private void FixedUpdate()
    {
        float vertical = moveJoystick.Vertical;
        float horizontal = moveJoystick.Horizontal;
        float tilt = Mathf.Abs(vertical) > Mathf.Abs(horizontal) ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
        var translation = transform.forward * (vertical * Time.deltaTime);
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, rotateJoystick.Horizontal * rotateSpeed, 0);
        float moveSpeed;
        float animSpeed;

        if (isAiming)
        {
            const float SPEED_LIMIT = 0.45f;
            animSpeed = walkAnimationSpeed * SPEED_LIMIT;
            moveSpeed = walkSpeed * SPEED_LIMIT;
        }
        else if (tilt < walkInputRange)
        {
            animSpeed = walkAnimationSpeed;
            moveSpeed = walkSpeed;
            //Debug.Log("walkspeed適用中");
        }
        else
        {
            vertical *= 2f;
            horizontal *= 1.2f; 
            animSpeed = runAnimatonSpeed;
            moveSpeed = runSpeed;
            //Debug.Log("runspeed適用中");
        }

        translation += transform.right * (horizontal * Time.deltaTime);
        translation *= moveSpeed;
        translation += rigidbody.position;
        
        animator.SetFloat("Vertical", vertical,0.1f,Time.deltaTime);
        animator.SetFloat("Horizontal", horizontal,0.1f,Time.deltaTime);

        animator.SetFloat("WalkSpeed", animSpeed);

        rigidbody.MovePosition(translation);
        rigidbody.rotation = rotation;

        const float ROTATE_LIMIT = 0.1f;
        eye.transform.Translate(0,rotateJoystick.Vertical * rotateSpeed * ROTATE_LIMIT,0);
        Vector3 camAngle = eye.position;

        float upLange = isAiming ?4.5f :2.5f;
        float downLange = isAiming ?-3.5f :1.5f;        
        if(camAngle.y > upLange) camAngle.y = upLange;
        if(camAngle.y < downLange) camAngle.y = downLange;
        eye.transform.position = camAngle;


        //Debug.Log(cam.transform.localEulerAngles.x);
        //Debug.Log("vert " + joystick.Vertical);
        //Debug.Log("horizon " + joystick.Horizontal);

        //Debug.Log(tilt);

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1f, 1f, 1f, 0f, 0f);     // LookAtの調整
        if (isAiming) animator.SetLookAtPosition(new Vector3(Aim.position.x, Aim.position.y - 1.7f, Aim.position.z));
        else animator.SetLookAtPosition(Aim.position);

    }

    private void OnClickJumpButton()
    {
        //rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //Debug.Log("dddffs");

        //ジャンプ、エイム、ジョイスティック等の操作系UIはviewにわけること        
        //PlaySwitchWeapon();
        PlayAiming();        
    }


    public void PlaySwitchWeapon()
    {
        animator.SetTrigger("SwitchWeapon");

    }

    public void PlayAiming()
    {
        isAiming = !isAiming;
        float w = isAiming ? 1f : 0f;
        animator.SetLayerWeight(2,w);
        animator.SetBool("Aiming", isAiming);

    }

    public void PlayHasGun()
    {
        animator.SetLayerWeight(1, 1f);
    }
    public void OnclickGunShoot(GunItemData gunItemData,GunItem gunItem)
    {
        if (gunItem.magazineSize <= 0)
        {
            OnpointerUpGunShoot(gunItem);
        }
        else 
        {
            if (!gunItem.gunEffect.activeSelf) gunItem.gunEffect.SetActive(true);
            GameObject bullet = Instantiate(gunItem.bulletObj, gunItem.gunPoint.position, Quaternion.identity);
            //bullet.GetComponent<Bullet>().playerID =
            bullet.GetComponent<Rigidbody>().AddForce(bullerFlyingDistance * gunItemData.atkPoint * (GetAimPoint() - gunItem.gunPoint.position).normalized);
            //gunItem.magazineSize--;
        }               
    }

    public void OnpointerUpGunShoot(GunItem gunItem)
    {
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
        Ray ray = new Ray(Camera.main.transform.position,(GetWorldPositionFromAimPoint() - Camera.main.transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // もしRayを投射して何らかのコライダーに衝突したら
        {
            if (hit.collider.CompareTag("Item")) return GetWorldPositionFromAimPoint();
            string name = hit.collider.gameObject.name; // 衝突した相手オブジェクトの名前を取得
            Debug.Log(name); // コンソールに表示
        }
        Debug.DrawRay(ray.origin,ray.direction * 30,Color.blue,10f);
        Debug.Log(hit.point);
        return hit.point;
    }

}