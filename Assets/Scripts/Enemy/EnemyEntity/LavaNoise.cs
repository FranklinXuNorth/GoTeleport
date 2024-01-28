using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaNoise : MonoBehaviour
{

    float noiseMultiplier = 0.1f;
    float scaleMultiplier = 2;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        noiseMultiplier = Random.Range(0.4f, 0.5f);
        scaleMultiplier = Random.Range(1.5f,2f);
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //noise sampling moves linearly
        float noiseOffset = Time.time;

        //sample the noise relative to the position
        float noise = Mathf.PerlinNoise(noiseMultiplier + noiseOffset, noiseMultiplier);

        //change the z scale (pointy axis)
        Vector3 scale = transform.localScale;
        scale.y = noise * scaleMultiplier;
        transform.localScale = scale;

        if (Time.time - time >= 3f)
        {
            Destroy(this.gameObject);
        }
    }

    
}
