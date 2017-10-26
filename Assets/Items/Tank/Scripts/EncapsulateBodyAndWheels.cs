using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EncapsulateBodyAndWheels : MonoBehaviour
{
    public BoxCollider targetCollider;
    public BoxCollider head;
    public List<BoxCollider> targets = new List<BoxCollider>();

    private void Update()
    {
        if (targetCollider == null)
            return;
        //Encapsulate(ref targetCollider);
        //AdjustHeadToBody(targetCollider.bounds);
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

    public void AdjustHeadToBody(Bounds body)
    {
        float diff = body.max.y - head.bounds.min.y;
        if (diff < 0)
            return;
        head.center += new Vector3(0, diff / 2f, 0);
        head.size -= new Vector3(0, diff, 0);
    }


}
