using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCollider : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            if (!playerMovement.GetDashStatus())
            {
                Health health = other.gameObject.GetComponent<Health>();
                health.dropHealth();
            }
            
        }
    }
}
