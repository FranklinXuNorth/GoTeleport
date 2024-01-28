using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator3 : MonoBehaviour
{
    private Config config;
    private CubeManager cubeManager;

    public GameObject lavaTile;
    public GameObject hintTile;

    List<BoxCollider> volumns;
    List<Vector3Int> genPoints;

    AudioSource audioSource;
    public AudioClip audioClip;

    // update
    float time;

    bool canDie = false;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;

        audioSource = GetComponent<AudioSource>();

        genPoints = new List<Vector3Int>();
        config = FindAnyObjectByType<Config>();
        cubeManager = FindAnyObjectByType<CubeManager>();
        volumns = new List<BoxCollider>();

        // get all the boxcolliders
        foreach (Transform child in this.transform)
        {
            GameObject childObject = child.gameObject;
            BoxCollider boxcoll = childObject.GetComponent<BoxCollider>();
            if (boxcoll != null)
            {
                volumns.Add(boxcoll);
            }
        }
        GenerateLavaArea();
        GenerateHint();

        time = Time.time;

    }

    private void Update()
    {
        if (Time.time - time >= config.lavaHintTime && !canDie)
        {
            GenerateLava();
            audioSource.clip = audioClip;
            audioSource.volume = 1.0f;
            audioSource.Play();
            time = Time.time;
            canDie = true;
        }
         
        if (canDie && Time.time - time >= 2f)
        {
            Destroy(this.gameObject);
        }

        
    }

    void GenerateLavaArea()
    {
        int number = Random.Range(config.lavaEasyNum, config.lavaHardNum);
        List<int> selected = new List<int>();

        while (number > 0)
        {
            int randomIndex = Random.Range(0, volumns.Count);            
            if (selected.Contains(randomIndex))
            {
                continue;
            }

            selected.Add(randomIndex);
            BoxCollider selectedArea = volumns[randomIndex];

            // select the cubes(level 1) that can generate laser emitters
            foreach (Vector3Int point in cubeManager.GetAllCubePos().Keys)
            {
                if (selectedArea.bounds.Contains(point) && !genPoints.Contains(point) && point.y == 0)
                {
                    genPoints.Add(point);
                }
            }

            number--;
        }
    }

    void GenerateHint()
    {
        foreach (Vector3 point in genPoints)
        {
            Instantiate<GameObject>(hintTile, point, Quaternion.identity);
        }
    }

    void GenerateLava()
    {
        foreach (Vector3 point in genPoints)
        {
            Instantiate<GameObject>(lavaTile, point, Quaternion.identity);
        }        
    }
}

