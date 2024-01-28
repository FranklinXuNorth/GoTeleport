using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThrowerTower : MonoBehaviour
{
    float time;
    Config config;


    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        config = FindAnyObjectByType<Config>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time >= config.towerDuration)
        {
            Destroy(this.gameObject);
        }
    }
}
