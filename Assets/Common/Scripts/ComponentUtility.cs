using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentUtility 
{
    static public void GetUniquelyComponentInParent<T>(Component[] from,ref List<T> targets) 
    {
        targets.Clear();
        T target;
        for (int i = 0; i < from.Length; i++)
        {
            target = from[i].GetComponentInParent<T>();
            if (target == null)
                continue;
            if (!targets.Contains(target))
                targets.Add(target);
        }
    }

    static public void DestroyIfExist<T>(GameObject target) where T: Object
    {
        if (target.GetComponent<T>() != null)
            Object.Destroy(target.GetComponent<T>());
    }

}
