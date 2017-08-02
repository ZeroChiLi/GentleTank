using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CountDownState
{
    None,
    Counting,
    End
}

public class CountDown : Photon.MonoBehaviour
{
    public Text messageTest;                                    // 游戏主信息文本 
    public float startDelay = 3f;                               // 开始游戏延迟

    private bool getDelayTime;                                  // 已经获取了同步延时时间s
    private float delayTimeRemain;                              // 剩余延迟时间
    static private CountDownState currentState = CountDownState.None;  // 当前倒计时状态

    /// <summary>
    /// 初始化剩余时间
    /// </summary>
    private void Awake()
    {
        delayTimeRemain = startDelay;
    }

    /// <summary>
    /// 更新倒计时
    /// </summary>
    private void FixedUpdate()
    {
        currentState = CountDownState.Counting;
        delayTimeRemain -= Time.fixedDeltaTime;
        messageTest.text = Mathf.CeilToInt(delayTimeRemain).ToString();
        if (delayTimeRemain < 0f)
        {
            messageTest.text = string.Empty;
            delayTimeRemain = 0;
            currentState = CountDownState.End;
            enabled = false;
        }
    }

    /// <summary>
    /// 是否正式开始了游戏
    /// </summary>
    /// <returns>是否正式开始了游戏</returns>
    static public bool IsStartGame()
    {
        return currentState == CountDownState.End;
    }

    /// <summary>
    /// 设置剩余时间
    /// </summary>
    /// <param name="remainTime"></param>
    public void SetReaminTime(float remainTime)
    {
        getDelayTime = true;
        delayTimeRemain = remainTime;
    }

    /// <summary>
    /// 同步开始延迟
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (PhotonNetwork.isMasterClient)
                stream.SendNext(delayTimeRemain - PhotonNetwork.GetPing() / 1000f);
        }
        else
        {
            if (!PhotonNetwork.isMasterClient)     // 获取主客户端当前剩余时间，再减去双方延迟
                SetReaminTime((float)stream.ReceiveNext() - PhotonNetwork.GetPing() / 1000f);
        }
    }
}
