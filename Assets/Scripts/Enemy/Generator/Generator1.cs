using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public class Generator1 : Detector1
{
    // enemies to generate
    public GameObject[] enemyPrefabs;

    // boolean space to trim
    public BoxCollider[] trimSpaces;


    protected override void Start()
    {
        base.Start();


        DetectOffsetSpace();
    }


    void DetectOffsetSpace()
    {
        if (enemyPrefabs.Length == 0)
            return;

        // temp list for deletion
        List<Vector3Int> pointDeletion = new List<Vector3Int>();
        List<Vector3Int> pointGen = new List<Vector3Int>();

        // select one random enemy game object
        int randomIndex;
        randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject genEnemy = enemyPrefabs[randomIndex];

        // get its detect boundary

        BoxCollider tempBoundary = genEnemy.GetComponent<BoxCollider>();
        int offsetX = Mathf.FloorToInt(tempBoundary.size.x / 2) + 1;
        int offsetZ = Mathf.FloorToInt(tempBoundary.size.z / 2) + 1;
        
        // if has void space or no cube in offset range,
        // add point to deletion list
        foreach (Vector3Int genPoints in allGenPointsXZ.Keys)
        {
            //Instantiate<GameObject>(testingObject, new Vector3Int(genPoints.x, 0, genPoints.z), Quaternion.identity);

            bool hasVoid = false;
            bool hasCube = false;

            // detect
            for (int posX = (int)(genPoints.x - offsetX); posX <= (int)(genPoints.x + offsetX); posX += 2)
            {
                for (int posZ = (int)(genPoints.z - offsetZ); posZ <= (int)(genPoints.z + offsetZ); posZ += 2)
                {
                    
                    if (!allGenPointsXZ.ContainsKey(new Vector3Int(posX, 0, posZ)))
                    {
                        hasVoid = true;
                    }
                }
            }

            if (hasVoid)
            {
                pointDeletion.Add(genPoints);
            }
        }

        foreach (Vector3Int point in pointDeletion)
        {
            allGenPointsXZ.Remove(point);
        }

        // if intersect with boolean space, keep
        if (trimSpaces.Length != 0)
        {
            foreach (BoxCollider booleanSpace in trimSpaces)
            {
                foreach (Vector3Int genPoints in allGenPointsXZ.Keys)
                {
                    if (booleanSpace.bounds.Contains(genPoints))
                    {
                        pointGen.Add(genPoints);
                    }
                }
            }
        }
                
        

        // adjust the height (default = 0f)

        // this is used for testing if this works fine
        foreach (Vector3Int point in pointGen)
        {
            Instantiate<GameObject>(testingObject, new Vector3Int(point.x, 0, point.z), Quaternion.identity);
        }
    }

}
