using UnityEngine;


public class WalkingMove : MonoBehaviour
{
    private Animator animator;
    private Camera cam;

    [SerializeField] private float speed = 3.0f;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine) return;
        
        // 上下左右キーで移動
        

        if (Input.GetKey("up"))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            animator.SetBool("Walking", true);
        }
        else if (Input.GetKey("down"))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
            animator.SetBool("Walking", true);
        }
        else if (Input.GetKey("right"))
        {
            transform.position += transform.right * speed * Time.deltaTime;
            animator.SetBool("Walking", true);
        }
        else if (Input.GetKey("left"))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
            


        // マウスドラッグで視点移動
        Vector3 cameraAngle;
        if (Input.GetMouseButton(0))
        {
            float x_Rotation = Input.GetAxis("Mouse X");
            float y_Rotation = Input.GetAxis("Mouse Y");
            x_Rotation = x_Rotation * 3;
            y_Rotation = y_Rotation * 3;
            this.transform.Rotate(0, x_Rotation, 0);
            cam.transform.Rotate(-y_Rotation, 0, 0);
            cameraAngle = cam.transform.localEulerAngles;
            if (cameraAngle.x < 315 && cameraAngle.x > 180)
            {
                cameraAngle.x = 315;
            }
            if (cameraAngle.x > 45 && cameraAngle.x < 180)
            {
                cameraAngle.x = 45;
            }
            cameraAngle.y = 0;
            cameraAngle.z = 0;
            cam.transform.localEulerAngles = cameraAngle;
        }

        
    }
}
