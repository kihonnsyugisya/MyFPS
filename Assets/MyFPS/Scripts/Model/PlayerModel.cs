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

    [HideInInspector] public Joystick moveJoystick;
    [HideInInspector] public Joystick rotateJoystick;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Animator animator;
    [HideInInspector] public BoolReactiveProperty isAiming = new(false);
    [HideInInspector] public List<AudioClip> audioClips = new();
    [HideInInspector] public ItemManager itemManager;
    [HideInInspector] public Transform eye;
    [HideInInspector] public Transform Aim;

    private new Rigidbody rigidbody;

    public BoolReactiveProperty isGrounded = new(false);

   
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

        isGrounded.Value = !(Physics.Raycast(transform.position, -Vector3.up, transform.position.y + 2f) && transform.position.y > 0.4f);

        if (isAiming.Value)
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

        float upLange = isAiming.Value ?4.5f :2.5f;
        float downLange = isAiming.Value ?-3.5f :1.5f;        
        if(camAngle.y > upLange) camAngle.y = upLange;
        if(camAngle.y < downLange) camAngle.y = downLange;
        eye.transform.position = camAngle;

        rigidbody.angularVelocity = Vector3.zero;

    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1f, 1f, 1f, 0f, 0f);     // LookAtの調整
        if (isAiming.Value) animator.SetLookAtPosition(new Vector3(Aim.position.x, Aim.position.y - 1.7f, Aim.position.z));
        else animator.SetLookAtPosition(Aim.position);

    }

    public void PlayJump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
    }

    public void PlayJumpPose(bool isGrounded)
    {
        if (isGrounded) animator.SetLayerWeight(4, 0f);
        else {
            animator.SetLayerWeight(4, 1f);
            animator.CrossFade("Jumping", 0f, 4);
        }
    }

    public void PlaySwitchWeapon()
    {
        animator.SetTrigger("SwitchWeapon");
        
    }

    public void PlayAiming()
    {
        isAiming.Value = !isAiming.Value;
        animator.SetLayerWeight(3, isAiming.Value ? 1f : 0f);
        animator.SetBool("Aiming", isAiming.Value);
    }

    public void PlayHasGun()
    {
        animator.SetLayerWeight(1, 1f);
    }

    private float currentValue;
    private float velocity;

    public void PlayGunHipFire(bool isPlay)
    {
        if (isAiming.Value) return;
        currentValue = Mathf.SmoothDamp(currentValue, isPlay ?1:0, ref velocity, isPlay ?0.005f :0);
        animator.SetLayerWeight(2, currentValue);
    }


    public void ReloadGun()
    {
        audioSource.PlayOneShot(audioClips[0]);
        animator.SetTrigger("Reload");
    }

    public void PlayDead()
    {
        animator.SetTrigger("Dead");
    }

}