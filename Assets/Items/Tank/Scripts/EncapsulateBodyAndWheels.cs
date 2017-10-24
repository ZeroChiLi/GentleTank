using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EncapsulateBodyAndWheels : MonoBehaviour
{
    public BoxCollider targetCollider;
    public List<BoxCollider> targets = new List<BoxCollider>();

    private void Update()
    {
        if (targetCollider == null || targets == null || targets.Count <= 0)
            return;
        Encapsulate(ref targetCollider);
    }

    public void Encapsulate(ref BoxCollider collider)
    {
        Bounds bounds = new Bounds();
        bounds = targets[0].bounds;
        for (int i = 0; i < targets.Count; i++)
            if (targets[i] != null)
                bounds.Encapsulate(targets[i].bounds);
        collider.center = bounds.center;
        collider.size = bounds.size;
    }


}
