using UnityEngine;

public class CatchTextureCameraRig : MonoBehaviour 
{
    [Range(0.001f,0.1f)]
    public float duration = 0.05f;
    public new Camera camera;
    public Transform stage;
    public DrawOutline drawOutline;

    private Transform lastParent;
    private Vector3 lastPosision;
    private Quaternion lastRotation;

    /// <summary>
    /// 渲染目标到纹理
    /// </summary>
    /// <param name="target">捕获目标</param>
    /// <param name="targetTexture">捕获到的纹理</param>
    public void RenderTarget(Transform target,RenderTexture targetTexture)
    {
        lastParent = target.parent;
        lastPosision = target.localPosition;
        lastRotation = target.localRotation;
        SetParentLocalPositionAndRotation(target, stage, Vector3.zero, Quaternion.identity);

        drawOutline.RenderToTexture(target.gameObject, targetTexture);
        //camera.targetTexture = targetTexture;
        //camera.Render();
        //camera.targetTexture = null;

        SetParentLocalPositionAndRotation(target, lastParent, lastPosision, lastRotation);
    }

    /// <summary>
    /// 设置父对象、相对父对象的位置和旋转值
    /// </summary>
    private void SetParentLocalPositionAndRotation(Transform target,Transform parent,Vector3 position,Quaternion rotation)
    {
        target.SetParent(parent);
        target.localPosition = position;
        target.localRotation = rotation;
    }

}
