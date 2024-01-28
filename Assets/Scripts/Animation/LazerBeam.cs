using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour
{

    LineRenderer lineRenderer;
    public float maxLineWidth = 1f;
    public float animationDuration = 0.5f;

    private float timeElapsed;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // ���㶯������
        timeElapsed += Time.deltaTime;
        float progress = timeElapsed / animationDuration;

        // �����������
        float newWidth = Mathf.PingPong(progress * maxLineWidth + 0.3f, maxLineWidth);
        lineRenderer.startWidth = newWidth;
        lineRenderer.endWidth = newWidth;

        // ����ʱ�䣬ѭ������
        if (timeElapsed > animationDuration)
        {
            timeElapsed = 0;
        }
    }
}
