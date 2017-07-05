using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class StateController : MonoBehaviour
{
    public State currentState;                              // 当前状态
    public State remainState;                               // 保持当前状态
    public AIStats defaultStats;                            // 默认状态信息
    public Transform eyes;                                  // 眼睛：拿来观察状态变化
    public PointList wayPointList;                          // 所有巡逻点

    [HideInInspector]
    public int playerID;                  // 玩家ID
    [HideInInspector]
    public Rigidbody rigidbodySelf;       // 自己的刚体
    [HideInInspector]
    public Collider colliderSelf;         // 自己的Collider
    [HideInInspector]
    public NavMeshAgent navMeshAgent;     // 导航组件
    [HideInInspector]
    public Transform chaseTarget;         // 追踪目标

    private int nextWayPointIndex;                          // 下一个巡逻点
    public Point NextWayPoint { get { return wayPointList[nextWayPointIndex]; } }

    private State startState;                               // 初始状态，每次复活后重置
    private AllTeamsManager allTeamsManager;                // 所有团队管理器
    private float stateTimeElapsed;                         // 计时器，每次调用CheckIfCountDownElapsed加一个Time.delta

    private void Awake()
    {
        rigidbodySelf = GetComponent<Rigidbody>();
        colliderSelf = GetComponent<Collider>();
        startState = currentState;
    }

    private void OnEnable()
    {
        SetupNavigation();
        currentState = startState;                          //复活的时候重置状态
    }

    private void Update()
    {
        currentState.UpdateState(this);                     //更新状态
    }

    //初始化导航的变量
    private void SetupNavigation()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.radius = defaultStats.navRadius;
        navMeshAgent.height = defaultStats.navHeight;
        navMeshAgent.speed = defaultStats.navSpeed.GetRandomValue();
        navMeshAgent.angularSpeed = defaultStats.navAngularSpeed.GetRandomValue();
        navMeshAgent.acceleration = defaultStats.navAcceleration.GetRandomValue();
        navMeshAgent.stoppingDistance = defaultStats.navStopDistance.GetRandomValue();
    }

    //设置巡逻点
    public void SetupAI(int playerID, bool aiEnable, AllTeamsManager teamsManager)
    {
        SetPlayerID(playerID);
        allTeamsManager = teamsManager;
        navMeshAgent.enabled = aiEnable;
        GetNewNextWayPoint(true);
    }

    //设置玩家ID
    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
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

    //获取下一个目标巡逻点
    public Point GetNewNextWayPoint(bool isRandom)
    {
        if (isRandom)
            nextWayPointIndex = wayPointList.GetRandomDifferenceIndex(nextWayPointIndex, 0, wayPointList.Count);
        else
            nextWayPointIndex = (nextWayPointIndex + 1) % wayPointList.Count;
        return wayPointList[nextWayPointIndex];
    }

    //开火
    public void Fire()
    {
        GetComponent<TankShooting>().Fire(defaultStats.attackForce.GetRandomValue(), defaultStats.attackRate.GetRandomValue(), defaultStats.attackDamage.GetRandomValue());
    }

    //是否被攻击了
    public bool GetHurt()
    {
        return GetComponent<TankHealth>().getHurt;
    }

    //设置是否感受到伤害
    public void SetHurt(bool hurt)
    {
        GetComponent<TankHealth>().getHurt = hurt;
    }

    //通过碰撞体判断是否队友
    public bool IsTeamMate(Collider collider)
    {
        return allTeamsManager.IsTeammate(playerID, collider.gameObject.GetComponent<StateController>().playerID);
    }

    public bool IsMyself(Collider collider)
    {
        return collider == colliderSelf;
    }

    //private void OnDrawGizmos()
    //{
    //    if (currentState != null && eyes != null)
    //    {
    //        Gizmos.color = currentState.sceneGizmoColor;
    //        Gizmos.DrawWireSphere(eyes.position, 1);
    //    }
    //}
}