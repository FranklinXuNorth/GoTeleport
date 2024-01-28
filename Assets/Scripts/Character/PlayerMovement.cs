using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  // components

  [HideInInspector] public static List<GameObject> playerObjects;
  [HideInInspector] private GameObject cameraObject;
  [HideInInspector] private PostProcessVolume postProcessVolume;
  [HideInInspector] private int currentPlayerIndex;
  [HideInInspector] public MeshRenderer meshRenderer;
  [HideInInspector] public Rigidbody rgbd;
  [HideInInspector] public BoxCollider collider;
  [HideInInspector] public Vignette vignette;
  [HideInInspector] public ChromaticAberration chromaticAberration;

  // constants
  [HideInInspector] public Vector3 RIGHT;
  [HideInInspector] public Vector3 FORWARD;
  [HideInInspector] public float MOVE_SPEED_MAX; // used only to calculate is dashing
  [HideInInspector] public float DASH_SPEED_MIN;
  [HideInInspector] public float DASH_SPEED_MAX;
  [HideInInspector] public float DASH_IMPULSE;
  [HideInInspector] public float DASH_COOLDOWN;
  [HideInInspector] public float TELEPORT_BULLET_MOMENT_MAX;
  [HideInInspector] public float SLOW_DOWN;
  [HideInInspector] public float UP_IMPULSE;

  // variables
  [HideInInspector] private bool isOnGround;
  [HideInInspector] private bool isDashing;
  [HideInInspector] private float dashingTime;
  [HideInInspector] public static float teleportBulletMoment;

  // input
  [HideInInspector] private Vector3 moveDirectionController;
  [HideInInspector] private bool dashController;
  [HideInInspector] private bool teleportController;

  // collision
  [HideInInspector] private float COLLISION_UPDATE_RADIUS;
  [HideInInspector] private LayerMask objectLayer;
  [HideInInspector] private List<Collider> disabledColliders;

  // Start is called before the first frame update
  void Start()
  {
    // initialize components
    cameraObject = GameObject.Find("Camera");
    objectLayer = LayerMask.GetMask("Default");
    rgbd = GetComponent<Rigidbody>();
    meshRenderer = GetComponent<MeshRenderer>();
    collider = GetComponent<BoxCollider>();
    postProcessVolume = cameraObject.GetComponent<PostProcessVolume>();
    vignette = postProcessVolume.profile.GetSetting<Vignette>();
    chromaticAberration = postProcessVolume.profile.GetSetting<ChromaticAberration>();
    disabledColliders = new List<Collider>();

    // add self to playerObjects
    if (playerObjects == null)
      playerObjects = new List<GameObject>();
    playerObjects.Add(gameObject);
    currentPlayerIndex = playerObjects.Count - 1;

    // set constants
    MOVE_SPEED_MAX = 10;
    DASH_SPEED_MIN = 15;
    DASH_SPEED_MAX = 20;
    DASH_IMPULSE = 20;
    DASH_COOLDOWN = 1000;
    SLOW_DOWN = 0.8f;
    TELEPORT_BULLET_MOMENT_MAX = 800;
    UP_IMPULSE = 100;
    RIGHT = cameraObject.transform.right;
    Vector3 temp = cameraObject.transform.forward;
    FORWARD = new Vector3(temp.x, 0, temp.z);
    FORWARD.Normalize();

    // set status
    isOnGround = true;

    // set framerate
    Time.fixedDeltaTime = 0.02f * SLOW_DOWN;

    // set random albedo
    Material material = meshRenderer.material;
    material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    meshRenderer.material = material;

    // set emission color
    Color emissionColor = material.color;
    emissionColor.a = 1;
    emissionColor *= 0.5f;
    material.SetColor("_EmissionColor", emissionColor);
  }

  public void OnMove(InputAction.CallbackContext ctx)
  {
    // print gamepad id
    // Debug.Log(ctx.control.device.deviceId);
    Vector2 moveDirection2D = ctx.ReadValue<Vector2>();
    Vector3 moveDirection3D = new Vector3(moveDirection2D.x, 0, moveDirection2D.y);
    moveDirection3D.Normalize();
    moveDirectionController = moveDirection3D;
  }

  public void OnTeleport(InputAction.CallbackContext ctx)
  {
    teleportController = ctx.ReadValueAsButton();
  }

  public void OnDash(InputAction.CallbackContext ctx)
  {
    dashController = ctx.ReadValueAsButton();
  }

  private void ResetInput()
  {
    // moveDirectionController = Vector3.zero; // don't reset move direction
    dashController = false;
    teleportController = false;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.gameObject.CompareTag("Floor"))
    {
      // give up impulse if is dashing
      if (dashingTime > 0)
      {
        Debug.Log("dashingTime > 0");
        rgbd.AddForce(Vector3.up * UP_IMPULSE, ForceMode.Impulse);
      }
    }
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
  private void DisableCollision()
  {

    Collider[] hitColliders = Physics.OverlapSphere(transform.position, COLLISION_UPDATE_RADIUS, objectLayer);
    foreach (var hitCollider in hitColliders)
    {
      if (hitCollider.transform.position.y <= transform.position.y - transform.localScale.y / 2)
      {
        Physics.IgnoreCollision(hitCollider, collider, true);
        disabledColliders.Add(hitCollider);
      }
      else
      {
        Physics.IgnoreCollision(hitCollider, collider, false);
      }
    }
  }

  private void EnableCollision()
  {
    foreach (var hitCollider in disabledColliders)
    {
      if (hitCollider != null)
        Physics.IgnoreCollision(hitCollider, collider, false);
    }
    disabledColliders.Clear();
  }

  private void Teleport()
  {
    GameObject theOtherPlayer = playerObjects[(currentPlayerIndex + 1) % playerObjects.Count];

    Vector3 pos = transform.position;
    Vector3 otherPos = theOtherPlayer.transform.position;

    Vector3 rotation = transform.rotation.eulerAngles;
    Vector3 otherRotation = theOtherPlayer.transform.rotation.eulerAngles;

    Vector3 velocity = rgbd.velocity;
    Vector3 otherVelocity = theOtherPlayer.GetComponent<Rigidbody>().velocity;

    Vector3 angularVelocity = rgbd.angularVelocity;
    Vector3 otherAngularVelocity = theOtherPlayer.GetComponent<Rigidbody>().angularVelocity;

    // switch the two
    transform.position = otherPos;
    theOtherPlayer.transform.position = pos;

    transform.rotation = Quaternion.Euler(otherRotation);
    theOtherPlayer.transform.rotation = Quaternion.Euler(rotation);

    rgbd.velocity = otherVelocity;
    theOtherPlayer.GetComponent<Rigidbody>().velocity = velocity;

    rgbd.angularVelocity = otherAngularVelocity;
    theOtherPlayer.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

    teleportBulletMoment = TELEPORT_BULLET_MOMENT_MAX;
  }

  // Update is called once per frame
  void Update()
  {
    // died if drops out of map
    if (transform.position.y <= -2)
      this.transform.position = new Vector3(0f, 2f, 0f);

    // Vector3 moveDirection = getMovementInput();
    Vector3 moveDirection = moveDirectionController;
    if (teleportController)
      Teleport();

    // slow down time if in bullet time
    if (teleportBulletMoment > 0)
    {
      // Time.timeScale = Mathf.Min(SLOW_DOWN, ((TELEPORT_BULLET_MOMENT_MAX - teleportBulletMoment) / TELEPORT_BULLET_MOMENT_MAX));
      Time.timeScale = 0.5f;
      teleportBulletMoment -= Time.deltaTime * 1000;
      if (teleportBulletMoment < 0)
      {
        teleportBulletMoment = 0;
        Time.timeScale = 1;
      }

      // Vignette intensity 0.6~0.0
      float vignetteIntensity = 0.4f * (teleportBulletMoment / TELEPORT_BULLET_MOMENT_MAX) + 0.2f;
      vignette.intensity.value = vignetteIntensity;
    }
    else
    {
      vignette.intensity.value = 0;
    }

    // if speed is greater than max, set dashing to true
    isDashing = rgbd.velocity.magnitude > DASH_SPEED_MIN;
    // freeze y position and rotation when is dashing
    rgbd.constraints = isDashing ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None;
    // enable emission when is dashing, can only dashing it is not dashing
    if (dashingTime < -DASH_COOLDOWN)
    {
      meshRenderer.material.EnableKeyword("_EMISSION");
    }
    if (dashController && dashingTime < -DASH_COOLDOWN)
    {
      // set velocity to 0
      rgbd.velocity = Vector3.zero;
      dashingTime = 200; // 0.20 seconds
    }
    if (dashingTime > 0)
    {

      dashingTime -= Time.deltaTime * 1000;
      if (dashingTime < 0)
      {
        EnableCollision();
        rgbd.useGravity = true;
        dashingTime = 0;
        meshRenderer.material.DisableKeyword("_EMISSION");
        rgbd.velocity = 0.1f * rgbd.velocity; // set velocity to near 0
      }
      else
      { // is dashing
        // update collision to avoid collide with floor
        DisableCollision();
        rgbd.useGravity = false;

        // add force only if speed is smaller than max dash speed
        if (rgbd.velocity.magnitude < DASH_SPEED_MAX)
          rgbd.AddForce(moveDirection * DASH_IMPULSE, ForceMode.Impulse);
        // disable collision when is dashing time is greater than 0
      }
    }
    else
    {
      dashingTime -= Time.deltaTime * 1000;
    }

    // add force to player if move speed isn't at max
    if (rgbd.velocity.magnitude < MOVE_SPEED_MAX)
      rgbd.AddForce(moveDirection * 1.2f, ForceMode.Impulse);

    // reset input
    ResetInput();
  }
}
