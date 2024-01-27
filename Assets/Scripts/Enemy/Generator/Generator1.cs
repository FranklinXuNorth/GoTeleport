using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator1 : Detector1
{
    // enemies to generate
    public GameObject[] enemyPrefabs;

    protected override void Start()
    {
        base.Start();

        DetectOffsetSpace();
        
    }


    void DetectOffsetSpace()
    {
        if (enemyPrefabs.Length == 0)
            return;

        // select one random enemy game object
        int randomIndex;
        randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject genEnemy = enemyPrefabs[randomIndex];

        // get its detect boundary

        BoxCollider tempBoundary = genEnemy?.GetComponent<BoxCollider>();
        int posX = Mathf.FloorToInt(tempBoundary.transform.position.x);
        int posZ = Mathf.FloorToInt(tempBoundary.transform.position.z);
        int offsetX = Mathf.FloorToInt(tempBoundary.size.x / 2);
        int offsetZ = Mathf.FloorToInt(tempBoundary.size.z / 2);


        // detect if
        foreach (Vector3 genPoints in allGenPoints.Keys)
        {
            for (int x = posX - offsetX; x <= posX + offsetX; x += 2)
            {
                for (int z = posZ - offsetZ; x <= posZ + offsetZ; x += 2)
                {
                    
                }
            }
        }
        



        
    }

}
