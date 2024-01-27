using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Generator2 : MonoBehaviour
{
    private Config config;
    private CubeManager cubeManager;

    List<BoxCollider> volumns;
    List<Vector3Int> genPoints;

    public GameObject emitterDual;
    private List<Vector3> allEmitters;

    // update
    float time;
    int index = 0;
    bool canstart = false;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;

        genPoints = new List<Vector3Int>();
        config = FindAnyObjectByType<Config>();
        cubeManager = FindAnyObjectByType<CubeManager>();
        volumns = new List<BoxCollider>();

        allEmitters = new List<Vector3>();


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

        // select the cubes(level 1) that can generate laser emitters
        foreach (Vector3Int point in cubeManager.GetAllCubePos().Keys)
        {
            foreach(BoxCollider box in volumns)
            {
                if (box.bounds.Contains(point) && !genPoints.Contains(point))
                {
                    genPoints.Add(point);
                }
            }
            
        }

        GenerateLaserPath();

    }

    private void Update()
    {
        if (canstart)
        {
            if (Time.time - time >= 0.1 && index < allEmitters.Count)
            {
                Vector3 genPosition = allEmitters[index];
                Vector3 genPosition2 = allEmitters[index+1];
                InstantiateEmitter(genPosition, genPosition2);
                
                time = Time.time;
                index += 2;
            }
        }

        if (index >= allEmitters.Count)
        {
            Destroy(this.gameObject);
        }
    }

    void GenerateLaserPath()
    {
        int number = 3;

        while(number > 0)
        {
            int randomIndex = Random.Range(0, genPoints.Count);
            Vector3Int selectedPoint = genPoints[randomIndex];

            // verticle beam and horizontal beam start and end point;
            Vector3Int pointer = selectedPoint;
            Vector3Int xStart = selectedPoint;
            Vector3Int xEnd = selectedPoint;
            Vector3Int zStart = selectedPoint;
            Vector3Int zEnd = selectedPoint;

            // start x
            while (genPoints.Contains(pointer))
            {
                pointer = new Vector3Int(pointer.x + 2, pointer.y, pointer.z);
            }
            xStart = new Vector3Int(pointer.x - 2, pointer.y, pointer.z);
            pointer = selectedPoint;

            // end x
            while (genPoints.Contains(pointer))
            {
                pointer = new Vector3Int(pointer.x - 2, pointer.y, pointer.z);
            }
            xEnd = new Vector3Int(pointer.x + 2, pointer.y, pointer.z);
            pointer = selectedPoint;

            if (Mathf.Abs(xStart.x - xEnd.x) <= 4)
                continue;

            // start z
            while (genPoints.Contains(pointer))
            {
                pointer = new Vector3Int(pointer.x, pointer.y, pointer.z + 2);
            }
            zStart = new Vector3Int(pointer.x, pointer.y, pointer.z - 2);
            pointer = selectedPoint;

            // end z
            while (genPoints.Contains(pointer))
            {
                pointer = new Vector3Int(pointer.x, pointer.y, pointer.z - 2);
            }
            zEnd = new Vector3Int(pointer.x, pointer.y, pointer.z + 2);

            if (Mathf.Abs(zStart.z - zEnd.z) <= 4)
                continue;

            allEmitters.Add(new Vector3(xStart.x, 1.5f, xStart.z));
            allEmitters.Add(new Vector3(xEnd.x, 1.5f, xEnd.z));

            allEmitters.Add(new Vector3(zStart.x, 1.5f, zStart.z));
            allEmitters.Add(new Vector3(zEnd.x, 1.5f, zEnd.z));

            number--;
        }


        canstart = true;
    }

    

    void InstantiateEmitter(Vector3 pos1, Vector3 pos2)
    {
        GameObject gameObjectTemp = Instantiate<GameObject>(emitterDual, Vector3.zero, Quaternion.identity);
        EmitterDual emitterDualSetting = gameObjectTemp.GetComponent<EmitterDual>();
        emitterDualSetting.SetEmitterPos(pos1, pos2);
    }
    
}
