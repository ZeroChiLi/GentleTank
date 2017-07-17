using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class TagWithColor
{
    public string tag;
    public Color color;
}


[CreateAssetMenu(menuName = "Aim Model/Default Aim Model")]
public class AimMode : ScriptableObject
{
    public List<TagWithColor> tagColorList = new List<TagWithColor>();

    
}
