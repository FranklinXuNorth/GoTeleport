using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void ShakeCamera(float duration, float strength)
    {
        Vector3 originalPosition = transform.localPosition;
        LeanTween.value(gameObject, originalPosition, originalPosition + strength * Vector3.left , duration)
            .setOnUpdate((Vector3 val) => {
                transform.localPosition = val;
            })
            .setEase(LeanTweenType.easeShake) // 使用震动效果的缓动函数
            .setOnComplete(() => {
                transform.localPosition = originalPosition; // 震动结束后，将摄像机位置重置
            });
    }
}
