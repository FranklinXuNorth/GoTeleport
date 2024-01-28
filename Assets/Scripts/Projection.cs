using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection : MonoBehaviour
{
    public GameObject lineRenderObject;
    GameObject lineRenderObject1;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderObject1 = Instantiate<GameObject>(lineRenderObject, Vector3.zero, Quaternion.identity);
        lineRenderer = lineRenderObject1.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(transform.position.x, -5f, transform.position.z);

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
