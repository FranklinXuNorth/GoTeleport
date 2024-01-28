using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LavaTile : MonoBehaviour
{
    Config config;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        config = FindAnyObjectByType<Config>();
        time = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<PlayerMovement>().GetDashStatus())
            {
                Health health = other.gameObject.GetComponent<Health>();
                health.dropHealth();
            }
        }
    }

    void Update()
    {
        if (Time.time - time >= 3f)
        {
            Destroy(this.gameObject);
        }
    }
}
