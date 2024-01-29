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
  Config config;

  // components

  [HideInInspector] public static List<GameObject> playerObjects;
  [HideInInspector] private GameObject cameraObject;
  [HideInInspector] private PostProcessVolume postProcessVolume;
  [HideInInspector] public int currentPlayerIndex;
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
  [HideInInspector] public float DASHING_TIME;

  // variables
  [HideInInspector] private bool isOnGround;
  [HideInInspector] private bool isDashing;
  [HideInInspector] private float dashingTime;
  [HideInInspector] public static float teleportBulletMoment;

  // input
  [HideInInspector] private Vector3 moveDirectionController;
  [HideInInspector] private bool dashController;
  [HideInInspector] private bool teleportController;
  [HideInInspector] public bool isKeyboardPlayer;

  // collision
  [HideInInspector] private float COLLISION_UPDATE_RADIUS;
  [HideInInspector] private LayerMask objectLayer;

  // particle effect
  public GameObject particleDash;
  public GameObject teleportA;
  public GameObject teleportB;

  // teleport threshold
  private int teleportTime;
  private int maxTeleportTime;
  private float teleportRefillTime;
  private float time;

  // Audio
  AudioSource audioSource;
  AudioClip clip1;
  public AudioClip clip2;
  bool dashAudioTrigger = false;

  // Start is called before the first frame update
  void Start()
  {
    // instantiate config
    config = FindAnyObjectByType<Config>();
    audioSource = GetComponent<AudioSource>();

    clip1 = audioSource.clip;

    // initialize components
    cameraObject = GameObject.Find("Camera");
    objectLayer = LayerMask.GetMask("Default");
    rgbd = GetComponent<Rigidbody>();
    meshRenderer = GetComponent<MeshRenderer>();
    collider = GetComponent<BoxCollider>();
    postProcessVolume = cameraObject.GetComponent<PostProcessVolume>();
    vignette = postProcessVolume.profile.GetSetting<Vignette>();
    chromaticAberration = postProcessVolume.profile.GetSetting<ChromaticAberration>();
    particleDash.SetActive(false);
    teleportB.SetActive(false);

    teleportTime = 1;
    maxTeleportTime = config.maxTeleportTime;
    teleportRefillTime = config.teleportRefillTime;
    Debug.Log("teleportRefillTime: " + teleportRefillTime + "I DON'T GET WHY THIS IS ALWAYS 10 SO I AM OVERRIDE IT");
    teleportRefillTime = 3.0f;
    time = Time.time;

    if (playerObjects == null)
      playerObjects = new List<GameObject>();

    // remove all null objects
    for (int i = 0; i < playerObjects.Count; i++)
    {
      if (playerObjects[i] == null)
      {
        playerObjects.RemoveAt(i);
        i--;
      }
    }

    // add self to playerObjects
    playerObjects.Add(gameObject);
    currentPlayerIndex = playerObjects.Count - 1;


    // set constants
    MOVE_SPEED_MAX = 10;
    DASH_SPEED_MIN = 30;
    DASH_SPEED_MAX = 35;
    DASH_IMPULSE = 10;
    DASH_COOLDOWN = 200;
    DASHING_TIME = 200;
    SLOW_DOWN = 0.8f;
    TELEPORT_BULLET_MOMENT_MAX = 600;
    UP_IMPULSE = 75;
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
    // material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

    if (currentPlayerIndex % 2 == 0) // WARNING: could be more than 1 even if we only have 2 controllers
                                     // Blue
      material.color = new Color(0.2f, 0.2f, 1f);
    else
      // Red
      material.color = new Color(1f, 0.2f, 0.2f);

    meshRenderer.material = material;

    // set emission color
    Color emissionColor = material.color;
    emissionColor.a = 1;
    emissionColor *= 0.5f;
    material.SetColor("_EmissionColor", emissionColor);
  }

  public void KeyboardInputPlayer1()
  {
    if (!isKeyboardPlayer)
      return;
    // player1 use WASD, LShift, F
    Vector3 md = Vector3.zero;
    if (Input.GetKey(KeyCode.W))
    {
      md += FORWARD;
    }
    if (Input.GetKey(KeyCode.S))
    {
      md -= FORWARD;
    }
    if (Input.GetKey(KeyCode.A))
    {
      md -= RIGHT;
    }
    if (Input.GetKey(KeyCode.D))
    {
      md += RIGHT;
    }
    if (md.magnitude > 0)
    {
      md.Normalize();
    }
    moveDirectionController = md;
    dashController = Input.GetKeyDown(KeyCode.LeftShift);
    teleportController = Input.GetKeyDown(KeyCode.F);
  }

  public void KeyboardInputPlayer2()
  {
    if (!isKeyboardPlayer)
      return;
    // player2 use P;l', RShift, K
    Vector3 md = Vector3.zero;
    if (Input.GetKey(KeyCode.P))
    {
      md += FORWARD;
    }
    if (Input.GetKey(KeyCode.Semicolon))
    {
      md -= FORWARD;
    }
    if (Input.GetKey(KeyCode.L))
    {
      md -= RIGHT;
    }
    if (Input.GetKey(KeyCode.Quote))
    {
      md += RIGHT;
    }
    if (md.magnitude > 0)
    {
      md.Normalize();
    }
    moveDirectionController = md;
    dashController = Input.GetKeyDown(KeyCode.Return);
    teleportController = Input.GetKeyDown(KeyCode.K);
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
    if (isKeyboardPlayer)
      return;
    // moveDirectionController = Vector3.zero; // don't reset move direction
    dashController = false;
    teleportController = false;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("DieWall"))
    {
      Health health = GetComponent<Health>();
      health.dropHealth();
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
    teleportA.SetActive(true);
    teleportB.SetActive(true);
    ParticleSystem particleSystemA = teleportA.GetComponent<ParticleSystem>();
    ParticleSystem particleSystemB = teleportB.GetComponent<ParticleSystem>();

    particleSystemA.Play();
    transform.position = otherPos;
    particleSystemB.Play();
    theOtherPlayer.transform.position = pos;

    transform.rotation = Quaternion.Euler(otherRotation);
    theOtherPlayer.transform.rotation = Quaternion.Euler(rotation);

    rgbd.velocity = otherVelocity;
    theOtherPlayer.GetComponent<Rigidbody>().velocity = velocity;

    rgbd.angularVelocity = otherAngularVelocity;
    theOtherPlayer.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

    teleportBulletMoment = TELEPORT_BULLET_MOMENT_MAX;

    audioSource.clip = clip1;
    audioSource.Play();

  }

  // Update is called once per frame
  void Update()
  {
    if (currentPlayerIndex % 2 == 0)
    {
      KeyboardInputPlayer1();
    }
    else
    {
      KeyboardInputPlayer2();
    }

    // died if drops out of map
    if (transform.position.y <= -0.5f)
    {
      Health health = GetComponent<Health>();
      health.dropHealth();
      this.transform.position = new Vector3(0f, 2f, 0f);
    }

    // refill teleporting time every period of time
    RefillTeleport();


    // Vector3 moveDirection = getMovementInput();
    Vector3 moveDirection = moveDirectionController;
    if (teleportController)
    {
      if (teleportTime > 0)
      {
        teleportTime--;
        Teleport();
      }
    }


    // slow down time if in bullet time
    if (teleportBulletMoment > 0)
    {
      // Time.timeScale = Mathf.Min(SLOW_DOWN, ((TELEPORT_BULLET_MOMENT_MAX - teleportBulletMoment) / TELEPORT_BULLET_MOMENT_MAX));
      Time.timeScale = 0.3f;

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
    rgbd.constraints = isDashing ? RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation
     : RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
    // enable emission when is dashing, can only dashing it is not dashing
    if (dashingTime < -DASH_COOLDOWN)
    {
      meshRenderer.material.EnableKeyword("_EMISSION");
    }
    if (dashController && dashingTime < -DASH_COOLDOWN)
    {
      // set velocity to 0
      rgbd.velocity = Vector3.zero;
      dashingTime = DASHING_TIME; // 0.20 seconds
    }
    if (dashingTime > 0)
    {
      if (!dashAudioTrigger)
      {
        audioSource.clip = clip2;
        audioSource.Play();
        dashAudioTrigger = true;
      }


      particleDash.SetActive(true);
      dashingTime -= Time.deltaTime * 1000;
      if (dashingTime < 0)
      {
        dashAudioTrigger = false;
        particleDash.SetActive(false);
        dashingTime = 0;
        meshRenderer.material.DisableKeyword("_EMISSION");
        // rgbd.velocity = 0.1f * rgbd.velocity; // set velocity to near 0
      }
      else
      { // is dashing
        // add force only if speed is smaller than max dash speed
        if (rgbd.velocity.magnitude < DASH_SPEED_MAX)
          rgbd.AddForce(moveDirection * DASH_IMPULSE, ForceMode.Impulse);
        if (rgbd.velocity.magnitude >= MOVE_SPEED_MAX)
        {
          Vector3 dir = rgbd.velocity;
          dir.Normalize();
          rgbd.velocity = dir * MOVE_SPEED_MAX;
        }
      }
    }
    else
    {
      dashingTime -= Time.deltaTime * 1000;
    }


    // add force to player if move speed isn't at max
    if (rgbd.velocity.magnitude < MOVE_SPEED_MAX)
    {
      rgbd.AddForce(moveDirection, ForceMode.Impulse);
      if (rgbd.velocity.magnitude >= MOVE_SPEED_MAX)
      {
        Vector3 dir = rgbd.velocity;
        dir.Normalize();
        rgbd.velocity = dir * MOVE_SPEED_MAX;
      }
    }



    // reset input
    ResetInput();
  }

  public bool GetDashStatus()
  {
    return (dashingTime > 0);
  }

  private void RefillTeleport()
  {
    // if filled then no need to count down
    if (teleportTime == maxTeleportTime)
      time = Time.time;

    if (teleportTime < maxTeleportTime && (Time.time - time) >= teleportRefillTime)
    {
      teleportTime += 1;
      time = Time.time;
    }
  }

  public int GetRestTeleportTime()
  {
    return teleportTime;
  }
}
