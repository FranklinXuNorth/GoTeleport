using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    // instantiate 
    Config config;

    public GameObject[] keys;
    bool isTriggered = true;
    bool onceTriggered = false;

    public UnityEvent onTriggered;

    private BoxCollider bxcoll;

    private void Start()
    {
        bxcoll = GetComponent<BoxCollider>();
        config = FindAnyObjectByType<Config>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onceTriggered) 
            isTriggered = true;

        foreach (GameObject key in keys)
        {
            if (key != null)
            {
                isTriggered = false;
                break;
            }
        }
        

        if (isTriggered && !onceTriggered)
        {
            onTriggered.Invoke();
            isTriggered = false;
            onceTriggered = true;
        }
    }

    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }

    public void moveDown()
    {
        float moveDist = this.transform.localScale.y * (2f/3f);
        Vector3 pos = transform.position;
        Vector3 target = new Vector3(pos.x, pos.y - moveDist, pos.z);

        LeanTween.move(this.gameObject, target, config.doorOpenTime)
            .setEase(LeanTweenType.easeOutCubic);
    }
} 
