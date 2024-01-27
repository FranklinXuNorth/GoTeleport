using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [Header("All Cubes")]
    public GameObject parentObject;

    /// <summary>
    /// a dictionary to store all the cube position
    /// Vector2: XZ position of the cube
    /// List<Vector3>: list of cubes of the same XZ positon, but with different heights(y)
    /// </summary>
    private Dictionary<Vector3Int, List<Vector3Int>> allCubePos;

    // just for test... ignore this
    public GameObject testObject;

    private void Awake()
    {

        allCubePos = new Dictionary<Vector3Int, List<Vector3Int>>();
        
        // sort all entities into groups of same XZ value
        foreach (Transform cube in parentObject.transform)
        {
            // get the XZ position of the cube
            Vector3Int cubePosXZ = new Vector3Int((int)cube.transform.position.x, 0, (int)cube.transform.position.z);

            // if there is no value in the key
            if (!allCubePos.ContainsKey(cubePosXZ))
            {
                List<Vector3Int> pointsSameXZ = new List<Vector3Int>();
                pointsSameXZ.Add(new Vector3Int((int)cube.position.x, 0, (int)cube.position.z));
                allCubePos.Add(cubePosXZ, pointsSameXZ);
            } 
            else
            {
                allCubePos[cubePosXZ].Add(new Vector3Int((int)cube.position.x, 0, (int)cube.position.z));
            }
        }

        #region Tester
        /*
        // testing if it works fine
        foreach (List<Vector3> points in allPoints.Values)
        {
            foreach (Vector3 point in points)
            {
                Instantiate<GameObject>(testObject, point, Quaternion.identity);
            }
        }
        */
        #endregion
    }

    /// <summary>
    /// once called, add point to / delete points from the list and update
    /// </summary>
    /// <param name="pointToAdd"></param>
    /// <param name="pointToDelete"></param>
    public void UpdatePoints(List<Vector3> pointToAdd, List<Vector3> pointToDelete)
    {

    }

    /// <summary>
    /// return the dictionary of all cube positions
    /// </summary>
    /// <returns></returns>
    public Dictionary<Vector3Int, List<Vector3Int>> GetAllCubePos()
    {
        return allCubePos;
    }
}
