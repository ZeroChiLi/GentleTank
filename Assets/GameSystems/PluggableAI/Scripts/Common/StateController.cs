using UnityEngine;
using UnityEngine.AI;

namespace GameSystem.AI
{
    public class StateController : MonoBehaviour
    {
        public State currentState;                              // 当前状态
        public State remainState;                               // 保持当前状态
        public AIStats defaultStats;                            // 默认状态信息
        public Transform eyes;                                  // 眼睛：拿来观察状态变化
        public Rigidbody rigidbodySelf;                         // 自己的刚体
        public Collider colliderSelf;                           // 自己的Collider
        public NavMeshAgent navMeshAgent;                       // 导航组件
        public PointList wayPointList;                          // 所有巡逻点

        [HideInInspector]
        public Transform chaseTarget;                           // 追踪目标
        public ObjectPreferences statePrefs;                     // 用于每次状态信息使用时

        private PlayerManager playerManager;                    // 玩家信息
        private HealthManager healthManager;                    // 玩家血量管理器
        private AttackManager attackManager;                    // 玩家攻击管理器
        private State startState;                               // 初始状态，每次复活后重置

        private int nextWayPointIndex;                          // 下一个巡逻点
        public Point NextWayPoint { get { return wayPointList[nextWayPointIndex]; } }

        /// <summary>
        /// 获取组件
        /// </summary>
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthManager = GetComponent<HealthManager>();
            attackManager = GetComponent<AttackManager>();
            rigidbodySelf = GetComponent<Rigidbody>();
            colliderSelf = GetComponent<Collider>();
            startState = currentState;
            statePrefs = new ObjectPreferences();
        }

        /// <summary>
        /// 配置导航信息，重置状态
        /// </summary>
        private void OnEnable()
        {
            SetupNavigation();
            currentState = startState;                          //复活的时候重置状态
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void Update()
        {
            currentState.UpdateState(this);                     //更新状态
        }

        /// <summary>
        /// 初始化导航的变量
        /// </summary>
        private void SetupNavigation()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = true;
            navMeshAgent.speed = defaultStats.navSpeed.GetRandomValue();
            navMeshAgent.angularSpeed = defaultStats.navAngularSpeed.GetRandomValue();
            navMeshAgent.acceleration = defaultStats.navAcceleration.GetRandomValue();
            navMeshAgent.stoppingDistance = defaultStats.navStopDistance.GetRandomValue();
        }

        /// <summary>
        /// 配置AI
        /// </summary>
        /// <param name="enable">是否可用</param>
        public void SetupAI(bool enable)
        {
            enabled = enable;
            navMeshAgent.enabled = enable;
            UpdateNextWayPoint(true);
        }

        /// <summary>
        /// 转换到下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                OnExitState();
            }
        }

        /// <summary>
        /// 改变状态后调用
        /// </summary>
        private void OnExitState()
        {
            statePrefs.Clear();
        }


        /// <summary>
        /// 更新下一个目标巡逻点
        /// </summary>
        /// <param name="isRandom">是否随机</param>
        /// <returns>返回下一个巡逻点</returns>
        public Point UpdateNextWayPoint(bool isRandom)
        {
            if (isRandom)
                nextWayPointIndex = wayPointList.GetRandomDifferenceIndex(nextWayPointIndex, 0, wayPointList.Count);
            else
                nextWayPointIndex = (nextWayPointIndex + 1) % wayPointList.Count;
            return wayPointList[nextWayPointIndex];
        }

        /// <summary>
        /// 攻击
        /// </summary>
        public void Attack()
        {
            attackManager.Attack(defaultStats.attackForce.GetRandomValue(), defaultStats.attackRate.GetRandomValue(), defaultStats.attackDamage.GetRandomValue());
        }

        /// <summary>
        /// 是否被攻击了
        /// </summary>
        /// <returns>被攻击状态</returns>
        public bool IsFeelPain()
        {
            return healthManager.IsFeelPain;
        }

        /// <summary>
        /// 通过碰撞体判断是否队友
        /// </summary>
        /// <param name="collider">碰撞的物体</param>
        /// <returns>是否是队友</returns>
        public bool IsTeamMate(Collider collider)
        {
            return playerManager.IsTeammate(collider.GetComponent<PlayerManager>());
        }

        /// <summary>
        /// 碰撞是否是自己
        /// </summary>
        /// <param name="collider">碰撞的物体</param>
        /// <returns>是否是自己</returns>
        public bool IsMyself(Collider collider)
        {
            return collider == colliderSelf;
        }

    }
}