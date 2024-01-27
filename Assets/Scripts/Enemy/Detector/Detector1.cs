using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Detector1 : MonoBehaviour
{
    protected Config config;

    // cube manager 
    protected CubeManager cubeManager;

    // a trigger collider to set the boundary of detection
    private BoxCollider boundary;
    private Transform boundTransform;
    float boundX; float boundZ;

    /// <summary>
    /// a dictionary to store all the position of the cubes
    /// </summary>
    private Dictionary<Vector3Int, List<Vector3Int>> allCubePos;

    /// <summary>
    /// a dictionary to store all the possible generating position
    /// Vector3: position  bool: valid?
    /// </summary>
    protected Dictionary<Vector3Int, bool> allGenPointsXZ;


    // just for testing 
    public GameObject testingObject;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        config = FindAnyObjectByType<Config>();

        // instantiate cube manager
        cubeManager = FindAnyObjectByType<CubeManager>();
        allCubePos = cubeManager.GetAllCubePos();

        // instantiate the dictionary
        allGenPointsXZ = new Dictionary<Vector3Int, bool>();

        // instantiate boundary
        boundary = GetComponent<BoxCollider>();
        boundTransform = boundary.transform;
        boundX = boundary.bounds.size.x;
        boundZ = boundary.bounds.size.z;

        // find all possible points within the boundary
        DetectPossiblePoints();

    }


    /// <summary>
    /// this is to detect and select every possible points for enemy generation
    /// </summary>
    void DetectPossiblePoints()
    {
        // 2d scanning XZ plane
        int minX = (int)(boundTransform.position.x - boundX / 2 + 1);
        int maxX = (int)(boundTransform.position.x + boundX / 2 - 1);
        int minZ = (int)(boundTransform.position.z - boundZ / 2 + 1);
        int maxZ = (int)(boundTransform.position.z + boundZ / 2 - 1);

        // find all the void space(space with no columns of cubes)
        for (int posX = minX; posX < maxX; posX+=2)
        {
            for (int posZ = minZ; posZ < maxZ; posZ+=2)
            {
                Vector3Int pointXZ = new Vector3Int((int)posX, 0, (int)posZ);

                if (!allCubePos.ContainsKey(pointXZ))
                {
                    allGenPointsXZ[pointXZ] = true;
                    /*
                    // this is used for testing if this works fine
                    Instantiate<GameObject>(testingObject, new Vector3(posX, 1f, posZ), Quaternion.identity);
                    */
                }
            }
        }
    }



}
