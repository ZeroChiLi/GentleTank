using Item.Tank;
using UnityEngine;
using UnityEngine.AI;

namespace GameSystem.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class StateController : MonoBehaviour
    {
        public State currentState;                              // 当前状态
        public State remainState;                               // 保持当前状态
        public AIStats defaultStats;                            // 默认状态信息
        public Transform eyes;                                  // 眼睛：拿来观察状态变化
        public PointList wayPointList;                          // 所有巡逻点

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

        private TankInformation tankInfo;                       // 坦克信息
        private State startState;                               // 初始状态，每次复活后重置
        private float stateTimeElapsed;                         // 计时器，每次调用CheckIfCountDownElapsed加一个Time.delta

        /// <summary>
        /// 获取组件
        /// </summary>
        private void Awake()
        {
            tankInfo = GetComponent<TankInformation>();
            rigidbodySelf = GetComponent<Rigidbody>();
            colliderSelf = GetComponent<Collider>();
            startState = currentState;
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
            navMeshAgent.radius = defaultStats.navRadius;
            navMeshAgent.height = defaultStats.navHeight;
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
            GetNewNextWayPoint(true);
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
            stateTimeElapsed = 0;
            SetHurt(false);
        }

        /// <summary>
        /// 返回是否过了时间间隔
        /// </summary>
        /// <param name="duration">持续时间</param>
        /// <returns>是否过了持续时间</returns>
        public bool CheckIfCountDownElapsed(float duration)
        {
            stateTimeElapsed += Time.deltaTime;
            return (stateTimeElapsed >= duration);
        }

        /// <summary>
        /// 获取下一个目标巡逻点
        /// </summary>
        /// <param name="isRandom">是否随机</param>
        /// <returns>返回下一个巡逻点</returns>
        public Point GetNewNextWayPoint(bool isRandom)
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
            GetComponent<TankShooting>().Fire(defaultStats.attackForce.GetRandomValue(), defaultStats.attackRate.GetRandomValue(), defaultStats.attackDamage.GetRandomValue());
        }

        /// <summary>
        /// 是否被攻击了
        /// </summary>
        /// <returns>被攻击状态</returns>
        public bool GetHurt()
        {
            return GetComponent<TankHealth>().getHurt;
        }

        /// <summary>
        /// 设置是否被攻击
        /// </summary>
        /// <param name="isGetHurt">是否被攻击</param>
        public void SetHurt(bool isGetHurt)
        {
            GetComponent<TankHealth>().getHurt = isGetHurt;
        }

        /// <summary>
        /// 通过碰撞体判断是否队友
        /// </summary>
        /// <param name="collider">碰撞的物体</param>
        /// <returns>是否是队友</returns>
        public bool IsTeamMate(Collider collider)
        {
            return AllTeamsManager.Instance.IsTeammate(tankInfo.playerID, collider.gameObject.GetComponent<TankInformation>().playerID);
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

        //private void OnDrawGizmos()
        //{
        //    if (currentState != null && eyes != null)
        //    {
        //        Gizmos.color = currentState.sceneGizmoColor;
        //        Gizmos.DrawWireSphere(eyes.position, 1);
        //    }
        //}
    }
}