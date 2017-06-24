using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThunderSkill : Skill
{
    private Vector3 inputHitPos;

    private void Update()
    {
        if (CanRelease())
        {
            aimImage.transform.position = Input.mousePosition;
            //RaycastObject(Input.mousePosition);
        }
    }



    /// <summary>
    /// 技能效果
    /// </summary>
    public override void SkillEffect()
    {
        Debug.Log("Fuck Everything");
        
    }


    private GameObject RaycastObject(Vector2 screenPos)
    {
        RaycastHit info;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(screenPos), out info, 200))
        {
            inputHitPos = info.point;
            //Debug.Log(inputHitPos + "  " + info.collider.gameObject.name);
            return info.collider.gameObject;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(inputHitPos, 1.5f); 
    }
}
