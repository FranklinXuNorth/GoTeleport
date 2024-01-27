using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Portable : MonoBehaviour
{
    public UnityEvent<Vector3, Vector3, Vector3> onSelected;
    protected Config config;

    Vector3 forceDirection;
    protected Rigidbody rgbd;
    protected BoxCollider bxcoll;

    public bool is_OnFloor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        config = FindAnyObjectByType<Config>();
        rgbd = GetComponent<Rigidbody>();
        bxcoll = GetComponent<BoxCollider>();
        forceDirection = Vector3.zero;
    }

    public virtual void transportObject(Vector3 playerPosition, Vector3 incomingForce, Vector3 rigidbodyVelocity)
    {
        rgbd.velocity = rigidbodyVelocity;
        this.transform.position = playerPosition;
        forceDirection = incomingForce;
    }

    protected virtual void Update()
    {
        if (transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }

        if (!is_OnFloor)
        {
            if (forceDirection != Vector3.zero)
            {
                forceDirection.Normalize();
                this.transform.position += forceDirection * config.playerMoveSpeedMax * Time.deltaTime;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            is_OnFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            is_OnFloor = false;
        }
    }

    
}
