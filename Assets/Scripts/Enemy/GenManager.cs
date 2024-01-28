using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenManager : MonoBehaviour
{
    // Config
    Config config;

    // Generators
    public GameObject lazerGenerator;
    public GameObject towerGenerator;
    public GameObject lavaGenerator;

    Dictionary<string, float> times;

    float lavaDuration;
    float lazerDuration;
    float towerDuration;



    // Start is called before the first frame update
    void Start()
    {
        config = FindAnyObjectByType<Config>();
        times = new Dictionary<string, float>();

        times["lazerDuration"] = Time.time;
        times["towerDuration"] = Time.time;
        times["lavaDuration"] = Time.time;

        lazerDuration = Random.Range(config.lazerMinTime, config.lazerMaxTime);
        towerDuration = Random.Range(config.towerMinTime, config.towerMaxTime);
        lavaDuration = Random.Range(config.lavaMinTime, config.lavaMaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        // for lazer
        if (Time.time - times["lazerDuration"] >= lazerDuration)
        {
            Instantiate<GameObject>(lazerGenerator, Vector3.zero, Quaternion.identity);
            lazerDuration = Random.Range(config.lazerMinTime, config.lazerMaxTime);
            times["lazerDuration"] = Time.time;
        }

        // for tower
        if (Time.time - times["towerDuration"] >= towerDuration)
        {
            Instantiate<GameObject>(towerGenerator, Vector3.zero, Quaternion.identity);
            towerDuration = Random.Range(config.towerMinTime, config.towerMaxTime);
            times["towerDuration"] = Time.time;
        }

        // for lava
        if (Time.time - times["lavaDuration"] >= lavaDuration)
        {
            Instantiate<GameObject>(lavaGenerator, Vector3.zero, Quaternion.identity);
            lavaDuration = Random.Range(config.lavaMinTime, config.lavaMaxTime);
            times["lavaDuration"] = Time.time;
        }

    }
}
