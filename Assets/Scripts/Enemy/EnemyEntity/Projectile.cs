using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    Config config;

    Rigidbody rgbd;
    SphereCollider spcoll;
    public UnityEvent<Vector3> onLaunch;
    public UnityEvent onExplode;

    Vector3 target;

    float homingFactor;

    public GameObject explosion;

    private void Start()
    {
        config = FindAnyObjectByType<Config>();
        rgbd = GetComponent<Rigidbody>();
        spcoll = GetComponent<SphereCollider>();
        target = GameObject.FindGameObjectWithTag("Player").transform.position;
        homingFactor = 0f;
    }

    private void Update()
    {
        float distance = (target - transform.position).magnitude;

        if (this.transform.position.y <= 0) 
        {
            onExplode.Invoke();
        }

        /*
        if (target.y < transform.position.y)
        {
            homingFactor = (distance * 1 / config.homingParameter);

            Vector3 newTarget = target - transform.position;
            Vector3 homingForce = newTarget.normalized * homingFactor;
            rgbd.AddForce(homingForce, ForceMode.Force);
        }
        else
        {
            homingFactor = (distance * 1 / (config.homingParameter * 2.5f));

            Vector3 newTarget = target - transform.position;
            Vector3 homingForce = newTarget.normalized * (homingFactor);
            rgbd.AddForce(homingForce, ForceMode.Force);
        }
        */

        
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        onExplode.Invoke();
    }


    public void Launch(Vector3 force)
    {
        rgbd = this.gameObject.GetComponent<Rigidbody>();
        rgbd.velocity = force;
    }

    public void playExplosion()
    {
        Instantiate<GameObject>(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
