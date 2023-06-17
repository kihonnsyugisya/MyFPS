using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerModel : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float walkAnimationSpeed = 2.2f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float runAnimatonSpeed = 1f;
    [SerializeField] private float rotateSpeed = 0.7f;
    [SerializeField] private float walkInputRange = 0.65f;
    [SerializeField] private float jumpForce;


    public FloatingJoystick joystick;

    [HideInInspector] Animator animator;
    [HideInInspector] public new Rigidbody rigidbody;

    public float distToGround;
    public bool isGrounded; 


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        float vertical = joystick.Vertical;
        float horizontal = joystick.Horizontal;
        float tilt = Mathf.Abs(vertical) > Mathf.Abs(horizontal) ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
        var translation = transform.forward * (vertical * Time.fixedDeltaTime);
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, horizontal * rotateSpeed, 0);
        float moveSpeed;
        float animSpeed;

        if (tilt < walkInputRange)
        {
            animSpeed = walkAnimationSpeed;
            moveSpeed = walkSpeed;
            Debug.Log("walkspeed適用中");
        }
        else
        {
            vertical *= 2f;
            horizontal *= 1.2f;
            animSpeed = runAnimatonSpeed;
            moveSpeed = runSpeed;
            Debug.Log("runspeed適用中");
        }

        translation += transform.right * (horizontal * Time.fixedDeltaTime);
        translation *= moveSpeed;
        translation += rigidbody.position;

        animator.SetFloat("Vertical", vertical, 0.1f, Time.fixedDeltaTime);
        animator.SetFloat("Horizontal", horizontal, 0.1f, Time.fixedDeltaTime);

        animator.SetFloat("WalkSpeed", animSpeed);

        rigidbody.MovePosition(translation);
        //Debug.Log("vert " + joystick.Vertical);
        //Debug.Log("horizon " + joystick.Horizontal);

        //Debug.Log(tilt);


    }

    // Update is called once per frame
    void Update()
    {
    }

}