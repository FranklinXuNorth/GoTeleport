using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorWave : MonoBehaviour
{
    public GameObject prefabB;
    public GameObject[,] objectArray;

    public int PositionX = 0;
    public int PositionY = 0;
    public int PositionZ = 0;

    public int ROWS = 40;
    public int COLS = 20;
    //between prefab
    public float distance = 2;

    public float noiseMultiplier = 0.1f;
    public float scaleMultiplier = 2;

    

    private GameObject containerB;


    // Start is called before the first frame update
    void Start()
    {
        containerB = new GameObject("containerB");

        //initialize the 2D array
        //array have a fixed lenght
        objectArray = new GameObject[ROWS, COLS];

        //nested iteration
        //2D arrangement of objects
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                GameObject go = Instantiate(prefabB, containerB.transform);
                //position them in a row with 2 units as distance
                go.transform.localPosition = new Vector3(c * distance+PositionX, PositionY, r * distance+PositionZ);

                //add each object to the array
                objectArray[r,c] = go;

            }
        }

        //center the container, moving all the prefab inside
        containerB.transform.position = new Vector2(-COLS*distance/2, -10);

    }

    // Update is called once per frame
    void Update()
    {
        //iterate through the array
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                GameObject go = objectArray[r, c];

                //noise sampling moves linearly
                float noiseOffset = Time.time;
                
                //sample the noise relative to the position
                float noise = Mathf.PerlinNoise(r * noiseMultiplier+ noiseOffset, c * noiseMultiplier);
                
                //change the z scale (pointy axis)
                Vector3 scale = go.transform.localScale;
                scale.z = noise * scaleMultiplier;
                go.transform.localScale = scale;
            }
        }
    }
}
