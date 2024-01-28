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
    public GameObject parentObject;
    List<BoxCollider> trimSpaces;

    // points that can generate
    List<Vector3Int> pointGen;


    protected override void Start()
    {
        base.Start();

        trimSpaces = new List<BoxCollider>();
        pointGen = new List<Vector3Int>();

        foreach (Transform child in parentObject.transform)
        {
            GameObject childObject = child.gameObject;
            trimSpaces.Add(child.GetComponent<BoxCollider>());
        }

        DetectOffsetSpace();
        RanGenerate();
        Destroy(this.gameObject);
    }


    void DetectOffsetSpace()
    {
        if (enemyPrefabs.Length == 0)
            return;

        // temp list for deletion
        List<Vector3Int> pointDeletion = new List<Vector3Int>();

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
            for (int posX = (int)(genPoints.x - offsetX); posX <= (int)(genPoints.x + offsetX); posX += 4)
            {
                for (int posZ = (int)(genPoints.z - offsetZ); posZ <= (int)(genPoints.z + offsetZ); posZ += 4)
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
        if (trimSpaces.Count != 0)
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

        /*
        // this is used for testing if this works fine
        foreach (Vector3Int point in pointGen)
        {
            Instantiate<GameObject>(testingObject, new Vector3Int(point.x, 0, point.z), Quaternion.identity);
        }
        */
    }


    void RanGenerate()
    {
        int number = Random.Range(config.towerEasyNum, config.towerHardNum);
        while (number > 0)
        {
            int randomIndex = Random.Range(0, pointGen.Count);
            int randomY = Random.Range(-1, 4);
            Vector3 randomPoint = pointGen[randomIndex];

            Vector3 randomGenPos = new Vector3(randomPoint.x, randomY, randomPoint.z);
            Instantiate<GameObject>(enemyPrefabs[0], randomGenPos, Quaternion.identity);
            number--;
        }
    }

    

}
