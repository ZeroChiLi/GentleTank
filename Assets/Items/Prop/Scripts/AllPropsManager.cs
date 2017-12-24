using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPropsManager : MonoBehaviour
{
    public Points spawnPoints;              // 产生道具的所有点位置
    public bool spawnAtStart = true;        // 是否在开始时就产生道具
    public float period = 10f;              // 产生道具的周期
    public int maxPropCount = 3;            // 同一时间最大道具数量
    public bool randomProp = true;          // 是否随机产生道具，false：按顺序。
    public ObjectPool parachutePool;        // 降落伞对象池
    public float fallDistance = 10f;        // 降落距离
    public Vector3 connectOffset;           // 降落伞和道具连接偏移（以道具为中心）
    public ObjectPool[] propPools;          // 所有道具

    private CountDownTimer timer;
    public CountDownTimer Timer { get { return timer = timer ?? new CountDownTimer(period); } }

    private int currentPropIndex = -1; 
    private Dictionary<PropBase, Point> propPointDict = new Dictionary<PropBase, Point>();
    private List<Point> emptyPoint = new List<Point>();
    private bool isStart =false;

    private void Start()
    {
        if (propPools.Length <= 0)
        {
            enabled = false;
            Debug.LogWarning("PropPools Is Empty!");
        }
    }

    private void Update()
    {
        if (GameRound.Instance.CurrentGameState != GameState.Playing)
        {
            // 非游戏回合进行时，不做道具产生
            isStart = false;
            Clean();
            return;
        }
        else if(!isStart && spawnAtStart && propPointDict.Count < maxPropCount)
        {
            // 游戏回合开始时，做初始化
            SpawnProp();
            isStart = true;
            return;
        }

        // 周期产生道具。到达最大数量的时候，不继续产生，当吃掉一个道具，且计时器已经过完周期时，会立即再产生一个道具
        if (Timer.IsTimeUp && propPointDict.Count < maxPropCount)
            SpawnProp();
    }

    /// <summary>
    /// 产生道具
    /// </summary>
    private void SpawnProp()
    {
        Point currentPoint = GetEmptyPoint();
        if (currentPoint == null)
            return;
        Timer.Start();  // 确定有位置后重新计时
        PropBase currentProp = GetProp();
        propPointDict.Add(currentProp, currentPoint);                   // 道具添加到已使用字典里
        currentProp.OnTouchFinishedEvent += PropOnTouchFinishedEvent;  // 添加道具被使用的监听事件
        StartCoroutine(FallProp(parachutePool.GetNextObject().GetComponent<ParachuteManager>(), currentProp, currentPoint));
    }

    /// <summary>
    /// 获取道具
    /// </summary>
    private PropBase GetProp()
    {
        if (randomProp)
            return propPools[Random.Range(0, propPools.Length)].GetNextObject().GetComponent<PropBase>();
        else
        {
            currentPropIndex += 1;
            currentPropIndex %= propPools.Length;
            return propPools[currentPropIndex].GetNextObject().GetComponent<PropBase>();
        }
    }

    /// <summary>
    /// 获取未被使用的点
    /// </summary>
    private Point GetEmptyPoint()
    {
        emptyPoint.Clear();
        for (int i = 0; i < spawnPoints.Count; i++)
            if (!propPointDict.ContainsValue(spawnPoints[i]))
                emptyPoint.Add(spawnPoints[i]);
        if (emptyPoint.Count == 0 || emptyPoint.Count <= spawnPoints.Count - maxPropCount)
            return null;
        return emptyPoint[Random.Range(0, emptyPoint.Count)];
    }

    /// <summary>
    /// 掉落道具的协程
    /// </summary>
    private IEnumerator FallProp(ParachuteManager parachute, PropBase prop, Point endPoint)
    {
        parachute.transform.rotation = endPoint.rotation;
        parachute.endPos = endPoint.position + connectOffset;
        parachute.startPos = parachute.endPos + new Vector3(0, fallDistance, 0);
        parachute.Play();
        prop.transform.rotation = endPoint.rotation;
        while (parachute.isActiveAndEnabled)
        {
            prop.transform.position = parachute.transform.position - connectOffset;
            yield return null;
        }
        prop.transform.position = endPoint.position;
    }

    /// <summary>
    /// 道具被使用完后，释放该点的使用权。
    /// </summary>
    /// <param name="prop">完成使命的道具</param>
    private void PropOnTouchFinishedEvent(PropBase prop)
    {
        if (propPointDict.ContainsKey(prop))
        {
            propPointDict.Remove(prop);
            prop.OnTouchFinishedEvent -= PropOnTouchFinishedEvent;
        }
    }

    private void Clean()
    {
        timer = null;
        foreach (var item in propPointDict)
            item.Key.gameObject.SetActive(false);
        propPointDict.Clear();
    }
}
