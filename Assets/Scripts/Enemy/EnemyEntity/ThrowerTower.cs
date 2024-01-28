using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThrowerTower : MonoBehaviour
{
    float time;
    Config config;

    bool isOver = false;


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
            isOver = true;
            time = Time.time;
            LeanTween.move(this.gameObject, new Vector3(transform.position.x, transform.position.y - 20f, transform.position.z), 1f)
                .setEase(LeanTweenType.easeOutCubic);
        }

        if (Time.time - time >= 1 && isOver)
        {
            Destroy(this.gameObject);
        }
    }
}
