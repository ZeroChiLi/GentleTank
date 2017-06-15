using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TankArray")]
public class TankArray : ScriptableObject
{
    public TankManager[] tanks;

    public TankManager this[int index]
    {
        get { return tanks[index]; }

        set { tanks[index] = value; }
    }

    public int Length { get { return tanks.Length; } }

}
