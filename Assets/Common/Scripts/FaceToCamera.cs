using UnityEngine;

/// <summary>
/// 面向当前镜头
/// </summary>
public class FaceToCamera : MonoBehaviour 
{
    private void FixedUpdate()
    {
        if (Camera.current == null)
            return;
        transform.rotation = Camera.current.transform.rotation;
    }
}
