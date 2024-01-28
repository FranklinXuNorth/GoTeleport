using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestoyWhenEnd : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portable") || other.gameObject.CompareTag("Player"))
        {
            Vector3 force = other.transform.position - this.transform.position;
            force.Normalize();

            Rigidbody rgbd = other.gameObject.GetComponent<Rigidbody>();
            rgbd.AddForce(force * 30f, ForceMode.Impulse);
            rgbd.AddForce(Vector3.up * 200f, ForceMode.Impulse);
        }
    }
}
