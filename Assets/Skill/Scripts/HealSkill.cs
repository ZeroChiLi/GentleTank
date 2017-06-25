using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : Skill
{
    public ObjectPool healPool;             //治愈特效池
    [Range(1, 100)]
    public int skillLevel = 1;              //技能等级（次数）
    [Range(1, 100)]
    public float healVolume = 10f;          //治愈血量
    [Range(0, 10f)]
    public float healRate = 1f;             //技能每次释放频率

    private WaitForSeconds waitTime;        //每次治愈时间间隔一样

    /// <summary>
    /// 技能效果
    /// </summary>
    /// <returns></returns>
    public override IEnumerator SkillEffect()
    {
        waitTime = new WaitForSeconds(healRate);
        for (int i = 0; i < skillLevel; i++)
            yield return HealPlayer(inputHitGameObject);
    }

    /// <summary>
    /// 治愈玩家
    /// </summary>
    /// <param name="transform">玩家的位置</param>
    /// <returns></returns>
    private IEnumerator HealPlayer(GameObject player)
    {
        //如果死掉了就直接结束治疗
        if (!player.activeInHierarchy)
            yield break;
        //显示治愈特效
        healPool.GetNextObjectActive().transform.position = player.transform.position;
        //加血
        inputHitGameObject.GetComponent<TankHealth>().GainHeal(healVolume);
        yield return waitTime;
    }

    /// <summary>
    /// 只有点击了玩家且包含血量组件才释放技能
    /// </summary>
    /// <returns>返回是否点击玩家</returns>
    public override bool ReleaseCondition()
    {
        return inputHitGameObject != null && inputHitGameObject.GetComponent<TankHealth>() != null;
    }
}
