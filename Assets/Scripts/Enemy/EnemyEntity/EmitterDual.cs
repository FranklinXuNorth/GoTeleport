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
                lineRenderer.SetPosition(0, new Vector3(emitter1Pos.x, 1.5f, emitter1Pos.z));
                lineRenderer.SetPosition(1, new Vector3(emitter2Pos.x, 1.5f, emitter2Pos.z));

                isShooting = true;
                time = Time.time;
            }

            if (Time.time - time >= 2f && isShooting)
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
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        return lineRenderer;
    }

    public void SetEmitterPos(Vector3 pos1, Vector3 pos2)
    {
        emitter1Pos = pos1;
        emitter2Pos = pos2;

        isReady = true;
    }
}
