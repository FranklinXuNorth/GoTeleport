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

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time >= 5)
        {
            Destroy(this.gameObject);
        }
    }
}