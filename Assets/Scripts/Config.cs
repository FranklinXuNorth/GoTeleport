using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    [Header("Player")]
    public float playerJumpSpeed;
    public float playerMoveSpeedMax;

    [Header("Camera")]
    public float cameraFollowSpeed;

    [Header("Thrower")]
    public float throwerFiringTime;

    [Header("Doors")]
    public float doorOpenTime;    

}
