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

    [Header("Thrower Max and Min")]
    public int minThrower;
    public int maxThrower;

    [Header("Generate Time")]
    public float lazerMinTime;
    public float lazerMaxTime;

    public float towerMinTime;
    public float towerMaxTime;
    public float towerDuration;

    public float lavaMinTime;
    public float lavaMaxTime;

    [Header("Generate Parameters")]
    public int lazerEasyNum;
    public int lazerHardNum;

    public int towerEasyNum;
    public int towerHardNum;

    public int lavaEasyNum;
    public int lavaHardNum;




}
