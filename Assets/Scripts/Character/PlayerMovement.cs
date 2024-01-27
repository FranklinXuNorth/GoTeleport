using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
  // components
  Config config;
  public GameObject cameraObject;
  Rigidbody rgbd;

  // constants
  Vector3 RIGHT;
  Vector3 FORWARD;
  float JUMP_SPEED;
  float MOVE_SPEED_MAX; // not used

  // variables
  bool isOnGround;

  // Start is called before the first frame update
  void Start()
  {
    // initialize components
    config = FindAnyObjectByType<Config>();
    rgbd = GetComponent<Rigidbody>();

    // set constants
    JUMP_SPEED = config.playerJumpSpeed;
    MOVE_SPEED_MAX = config.playerMoveSpeedMax;
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
    if (isOnGround && Input.GetKeyDown(KeyCode.Space))
    {
      rgbd.AddForce(Vector3.up * JUMP_SPEED, ForceMode.Impulse);
      // TODO: add gamepad jump
    }

    Vector3 moveDirection = getMovementInput();
    Debug.Log(moveDirection);

    // add force to player if move speed isn't at max
    if (rgbd.velocity.magnitude < config.playerMoveSpeedMax)
      rgbd.AddForce(moveDirection, ForceMode.Impulse);
  }
}
