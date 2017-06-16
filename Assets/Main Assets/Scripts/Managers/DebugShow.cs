using UnityEngine;

public class DebugShow : MonoBehaviour 
{
    public PointList[] pointLists;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pointLists.Length; i++)
        {
            if (pointLists[i] != null)
                pointLists[i].DebugDrawPoint();
        }
    }
}
