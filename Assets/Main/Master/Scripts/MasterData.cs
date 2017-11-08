using System.Collections.Generic;
using UnityEngine;
using GameSystem.AI;

[CreateAssetMenu(menuName = "MasterData")]
public class MasterData : ScriptableObject
{
    public string masterName = "Master";
    public TeamManager team;
    public TankAssembleManager selectedTank;
    public GameObject standardPerfab;
    public Color representColor;
    public bool isAI;
    public AIState aiState;
    public int level = 1;
    public int money = 100;
    public float weightLimit = 50f;

}
