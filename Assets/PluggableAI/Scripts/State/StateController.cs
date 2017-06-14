using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Complete;

[RequireComponent(typeof(NavMeshAgent))]
public class StateController : MonoBehaviour
{
    public State currentState;                              //当前状态
    public State remainState;                               //保持当前状态
    public EnemyStats enemyStats;                           //敌人状态
    public Transform eyes;                                  //眼睛：拿来观察状态变化
    public Rigidbody aiRigidbody;                           //AI的刚体

    [HideInInspector] public NavMeshAgent navMeshAgent;     //导航组件
    [HideInInspector] public List<Transform> wayPointList;  //所有巡逻点
    [HideInInspector] public int nextWayPoint;              //下一个巡逻点
    [HideInInspector] public Transform chaseTarget;         //追踪目标
    [HideInInspector] public float stateTimeElapsed;        //状态变化时间间隔

    private TankShooting tankShooting;                      //射击
    private TankHealth tankHealth;                          //判断是否受伤
    private bool aiActive;                                  //AI是否有效
    private State startState;                               //初始状态，每次复活后重置

    private void Awake()
    {
        tankShooting = GetComponent<TankShooting>();
        tankHealth = GetComponent<TankHealth>();
        startState = currentState;
        SetupNavigation();
    }

    private void OnEnable()
    {
        currentState = startState;                          //复活的时候重置状态
        nextWayPoint = Random.Range(0, wayPointList.Count); //随即巡逻点
    }

    private void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);                     //更新状态
    }

    //初始化导航的变量
    private void SetupNavigation()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.speed = enemyStats.navSpeed;
        navMeshAgent.angularSpeed = enemyStats.navAngularSpeed;
        navMeshAgent.acceleration = enemyStats.navAcceleration;
        navMeshAgent.stoppingDistance = enemyStats.navStopDistance;
        navMeshAgent.radius = enemyStats.navRadius;
        navMeshAgent.height = enemyStats.navHeight;
    }

    //设置巡逻点还有是否设置AI并且是否激活导航
    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
    {
        wayPointList = wayPointsFromTankManager;
        aiActive = aiActivationFromTankManager;
        if (aiActive)
            navMeshAgent.enabled = true;
        else
            navMeshAgent.enabled = false;
    }

    //转换到下一个状态
    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    //退出改变状态时调用
    private void OnExitState()
    {
        stateTimeElapsed = 0;
        SetHurt(false);
    }

    //返回是否过了时间间隔
    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    //开火
    public void Fire()
    {
        tankShooting.Fire(enemyStats.attackForce, enemyStats.attackRate);
    }

    //是否被攻击了
    public bool GetHurt()
    {
        return tankHealth.getHurt;
    }

    //设置再感受到伤害
    public void SetHurt(bool hurt)
    {
        tankHealth.getHurt = hurt;
    }

    private void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        }
    }
}