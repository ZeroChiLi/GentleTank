using UnityEngine;

public class WorldToScreenPos : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 64, 0);    // 偏移量

    private void OnGUI()
    {
        if (Camera.current == null)
            return;
        transform.position = Camera.current.WorldToScreenPoint(target.transform.position) + offset;
    }
}
