using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCollider : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rgbd = other.gameObject.GetComponent<Rigidbody>();
            rgbd.AddForce(Vector3.up * 200f, ForceMode.Impulse);
        }
    }
}
