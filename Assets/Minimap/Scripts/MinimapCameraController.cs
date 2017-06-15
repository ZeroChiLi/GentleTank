using UnityEngine;

public class MinimapCameraController : MonoBehaviour 
{
    public Vector3 distance = new Vector3(0, 50, 0);
    public GameObject target;

    private void Update()
    {
        if (target == null)
            return;
        transform.position = target.transform.position + distance;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

}
