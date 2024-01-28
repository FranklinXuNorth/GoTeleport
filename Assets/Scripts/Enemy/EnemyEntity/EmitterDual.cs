using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterDual : MonoBehaviour
{
    public GameObject beam;
    public GameObject emitter;

    GameObject beamGenerated;
    GameObject emitter1;
    GameObject emitter2;

    public GameObject colliderPrefab;

    private Vector3 emitter1Pos;
    private Vector3 emitter2Pos;

    float time;
    bool isReady = false;
    bool isEmitterGenerated = false;
    bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (!isEmitterGenerated)
            {
                emitter1 = Instantiate<GameObject>(emitter, emitter1Pos, Quaternion.identity);
                emitter2 = Instantiate<GameObject>(emitter, emitter2Pos, Quaternion.identity);
                isEmitterGenerated = true;
            }
            

            if (Time.time - time >= 1.5 && !isShooting)
            {
                beamGenerated = Instantiate<GameObject>(beam, Vector3.zero, Quaternion.identity);
                LineRenderer lineRenderer =  InstantiateBeam(beamGenerated);

                Vector3 startPos = new Vector3(emitter1Pos.x, 2f, emitter1Pos.z);
                Vector3 endPos = new Vector3(emitter2Pos.x, 2f, emitter2Pos.z);

                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPos);                

                Vector3 midPoint = (startPos + endPos) / 2;
                float segmentLength = Vector3.Distance(startPos, endPos);
                float colliderThickness = 0.5f; // 或者根据需要调整碰撞体的厚度

                GameObject colliderObject = Instantiate(colliderPrefab, midPoint, Quaternion.identity, this.transform);
                BoxCollider boxCollider = colliderObject.GetComponent<BoxCollider>();
                boxCollider.size = new Vector3(colliderThickness, colliderThickness, segmentLength);
                boxCollider.transform.LookAt(startPos);

                isShooting = true;
                time = Time.time;
            }

            if (Time.time - time >= 0.5 && isShooting)
            {
                Destroy(this.gameObject);
                Destroy(emitter1);
                Destroy(emitter2);
                Destroy(beamGenerated);
            }
        }
        
    }

    LineRenderer InstantiateBeam(GameObject gameObject)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        

        return lineRenderer;
    }

    public void SetEmitterPos(Vector3 pos1, Vector3 pos2)
    {
        emitter1Pos = pos1;
        emitter2Pos = pos2;

        isReady = true;
    }



}
