using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Config config;
    public GameObject camera_Object;
    // rigidbody
    Rigidbody rgbd;

    // factors
    float jump_Speed;
    float move_Speed_Max;
    bool is_OnGround;

    float current_Speed;

    // directions
    Vector3 right;
    Vector3 forward;
    Vector3 gravity;

    Vector3 moveDirection;


    

    // Start is called before the first frame update
    void Start()
    {
        config = FindAnyObjectByType<Config>();
        rgbd = GetComponent<Rigidbody>();

        // set values
        jump_Speed = config.playerJumpSpeed;
        move_Speed_Max = config.playerMoveSpeedMax;

        // set directions
        right = camera_Object.transform.right;
        Vector3 temp = camera_Object.transform.forward;
        forward = new Vector3(temp.x, 0, temp.z);
        forward.Normalize();

        moveDirection = Vector3.zero;

        // set status
        is_OnGround = true;

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            is_OnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            is_OnGround = false;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if (is_OnGround)
        {
            moveDirection = Vector3.zero;

            // jump
            if (Input.GetKeyDown(KeyCode.Space))
                rgbd.AddForce(Vector3.up * jump_Speed, ForceMode.Impulse);

            #region gamepad input
            /*
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            moveDirection = forward * vertical + right * horizontal;
            */
            #endregion
        }

        // move
        #region keyboard input
        bool isInputAD = true;
        bool isInputWS = true;
        // move
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            moveDirection -= right;
        }

        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            moveDirection += right;
        }

        else isInputAD = false;


        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            moveDirection += forward;
        }


        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            moveDirection -= forward;
        }

        else isInputWS = false;

        // edge case: acceleration out of range
        if (current_Speed >= move_Speed_Max)
        {
            current_Speed = move_Speed_Max;
        }

        if (!isInputAD && !isInputWS)
        {

        }
        #endregion

        moveDirection.Normalize();
        this.transform.position += moveDirection * move_Speed_Max * Time.deltaTime;

        // click to emit a ray
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            RaycastHit[] rayHits = Physics.RaycastAll(mousePosition, camera_Object.transform.forward, 200f);

            

            foreach(RaycastHit rayHit in rayHits)
            {
                if (rayHit.collider.gameObject != null && rayHit.collider.gameObject.CompareTag("Portable"))
                {
                    GameObject target = rayHit.collider.gameObject;
                    Portable portable = target.GetComponent<Portable>();

                    Vector3 positionTemp = target.transform.position;
                    portable.onSelected.Invoke(this.transform.position, moveDirection, rgbd.velocity);
                    this.gameObject.transform.position = positionTemp;
                }
            }

        }       

    }

   

}
