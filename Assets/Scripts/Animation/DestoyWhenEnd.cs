using System.Collections;
using System.Collections.Generic;
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
            Destroy(other.gameObject);
        }
    }
}
