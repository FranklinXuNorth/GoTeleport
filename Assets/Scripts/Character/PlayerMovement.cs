using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
  // components
  public GameObject cameraObject;
  MeshRenderer meshRenderer;
  Rigidbody rgbd;
  BoxCollider collider;

  // constants
  Vector3 RIGHT;
  Vector3 FORWARD;
  float MOVE_SPEED_MAX; // used only to calculate is dashing
  float DASH_SPEED_MIN;
  float DASH_SPEED_MAX;
  float DASH_IMPULSE;

  // variables
  bool isOnGround;
  bool isDashing;
  float dashingTime;

  // Start is called before the first frame update
  void Start()
  {
    // initialize components
    rgbd = GetComponent<Rigidbody>();
    meshRenderer = GetComponent<MeshRenderer>();
    collider = GetComponent<BoxCollider>();

    // set constants
    MOVE_SPEED_MAX = 10;
    DASH_SPEED_MIN = 15;
    DASH_SPEED_MAX = 20;
    DASH_IMPULSE = 10;
    RIGHT = cameraObject.transform.right;
    Vector3 temp = cameraObject.transform.forward;
    FORWARD = new Vector3(temp.x, 0, temp.z);
    FORWARD.Normalize();

    // set status
    isOnGround = true;
  }

  private void OnCollisionStay(Collision collision)
  {
    if (collision.collider.gameObject.CompareTag("Floor"))
    {
      isOnGround = true;
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (collision.collider.gameObject.CompareTag("Floor"))
    {
      isOnGround = false;
    }
  }

  private Vector3 getMovementInput()
  {
    Vector3 moveDirection = Vector3.zero;

    # region keyboard input
    if (Input.GetKey(KeyCode.A))
    {
      moveDirection -= RIGHT;
    }
    else if (Input.GetKey(KeyCode.D))
    {
      moveDirection += RIGHT;
    }
    else if (Input.GetKey(KeyCode.W))
    {
      moveDirection += FORWARD;
    }
    else if (Input.GetKey(KeyCode.S))
    {
      moveDirection -= FORWARD;
    }
    #endregion

    #region gamepad input
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    moveDirection = FORWARD * vertical + RIGHT * horizontal;
    #endregion

    moveDirection.Normalize();
    return moveDirection;
  }

  // Update is called once per frame
  void Update()
  {
    // if speed is greater than max, set dashing to true
    isDashing = rgbd.velocity.magnitude > DASH_SPEED_MIN;
    // freeze y position and rotation when is dashing
    rgbd.constraints = isDashing ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None;
    // enable emission when is dashing
    if (isDashing)
      meshRenderer.material.EnableKeyword("_EMISSION");
    else
      meshRenderer.material.DisableKeyword("_EMISSION");


    Vector3 moveDirection = getMovementInput();
    if (Input.GetKeyDown(KeyCode.Space))
    {
      // set velocity to 0
      rgbd.velocity = Vector3.zero;
      dashingTime = 500; // 0.5 seconds
    }
    if (dashingTime > 0)
    {
      dashingTime -= Time.deltaTime * 1000;
      // add force only if speed is smaller than max dash speed
      if (rgbd.velocity.magnitude < DASH_SPEED_MAX)
        rgbd.AddForce(moveDirection * DASH_IMPULSE, ForceMode.Impulse);
      // disable collision when is dashing time is greater than 0
      // collider.isTrigger = true;
    } else {
      // collider.isTrigger = false;
    }

    

    // add force to player if move speed isn't at max
    if (rgbd.velocity.magnitude < MOVE_SPEED_MAX)
      rgbd.AddForce(moveDirection, ForceMode.Impulse);
  }
}
