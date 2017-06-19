using UnityEngine;

// 调试显示所有点的位置
public class DebugShow : MonoBehaviour 
{
    public PointList[] pointLists;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pointLists.Length; i++)
            if (pointLists[i] != null)
                pointLists[i].DebugDrawPoint();
    }
}
