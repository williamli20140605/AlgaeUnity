using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    // 设定目标宽高比
    private float targetAspectRatio = 16.0f / 9.0f;
    private float focusScale = 2f;

    void Start()
    {
        AdjustCameraAspectRatio();
    }

    void AdjustCameraAspectRatio()
    {
        Camera cam = GetComponent<Camera>();
        // 计算当前屏幕的宽高比
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;
        // 计算需要的缩放比例
        float scaleHeight = windowAspectRatio / targetAspectRatio;

        // 根据屏幕比例调整相机的rect
        if (scaleHeight < 1.0f)
        {
            float adjustedHeight = scaleHeight * focusScale;
            float verticalPadding = (1.0f - adjustedHeight) / 2.0f;
            cam.rect = new Rect(0, verticalPadding, 1.0f, adjustedHeight);
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            float adjustedWidth = scaleWidth * focusScale;
            float horizontalPadding = (1.0f - adjustedWidth) / 2.0f;
            cam.rect = new Rect(horizontalPadding, 0, adjustedWidth, 1.0f);
        }
    }
}
