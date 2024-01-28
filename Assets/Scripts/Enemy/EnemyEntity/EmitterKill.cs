using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterKill : MonoBehaviour
{
    float existingTime = 3.5f;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time >= existingTime)
        {
            Destroy(this.gameObject);
        }
    }
}
