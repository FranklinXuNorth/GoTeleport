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
        // 计算动画进度
        timeElapsed += Time.deltaTime;
        float progress = timeElapsed / animationDuration;

        // 更新线条宽度
        float newWidth = Mathf.PingPong(progress * maxLineWidth + 0.3f, maxLineWidth);
        lineRenderer.startWidth = newWidth;
        lineRenderer.endWidth = newWidth;

        // 重置时间，循环动画
        if (timeElapsed > animationDuration)
        {
            timeElapsed = 0;
        }
    }
}
