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
            .setEase(LeanTweenType.easeShake) // ʹ����Ч���Ļ�������
            .setOnComplete(() => {
                transform.localPosition = originalPosition; // �𶯽����󣬽������λ������
            });
    }
}
