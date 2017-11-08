using System.Collections.Generic;
using UnityEngine;

public class DebugDistance : MonoBehaviour 
{
    public Transform target1;
    public Transform target2;

    private void Update()
    {
        if (target1 && target2)
            Debug.Log(Vector3.Distance(target1.position,target2.position));
    }


}
