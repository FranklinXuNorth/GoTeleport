using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject playerObject;
    Config config;
    Vector3 playerPosition;

    float cameraFollowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        config = FindAnyObjectByType<Config>();

        playerPosition = playerObject.transform.position;
        cameraFollowSpeed = config.cameraFollowSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerObject.transform.position;
        transform.position = Vector3.Lerp(transform.position, playerPosition, cameraFollowSpeed * Time.deltaTime);
    }
}
