using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Heal Skill")]
public class HealSkill : Skill
{
    public ObjectPool healPool;             //治愈特效池
    [Range(1, 100)]
    public int skillLevel = 5;              //技能等级（次数）
    [Range(1, 100)]
    public float healVolume = 20f;          //治愈血量
    [Range(0, 10f)]
    public float healRate = 0.5f;             //技能每次释放频率

    private WaitForSeconds healWaitTime;        //每次治愈时间间隔一样

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        healWaitTime = new WaitForSeconds(healRate);
    }

    /// <summary>
    /// 技能效果
    /// </summary>
    /// <returns></returns>
    public override IEnumerator SkillEffect()
    {
        GameObject player = AllSkillManager.Instance.aim.HitGameObject;
        TankHealth tankHealth = player.GetComponent<TankHealth>();
        for (int i = 0; i < skillLevel; i++)
            yield return HealPlayer(player, tankHealth);
    }

    /// <summary>
    /// 治愈玩家
    /// </summary>
    /// <param name="transform">玩家的位置</param>
    /// <returns></returns>
    private IEnumerator HealPlayer(GameObject player, TankHealth tankHealth)
    {
        //如果死掉了或血条没有激活就直接结束治疗
        if (!player.activeInHierarchy || tankHealth.enabled == false)
            yield break;
        //显示治愈特效
        healPool.GetNextObject().transform.position = player.transform.position;
        //加血
        tankHealth.GainHeal(healVolume);
        yield return healWaitTime;
    }

    /// <summary>
    /// 只有点击了玩家且包含血量组件才释放技能
    /// </summary>
    /// <returns>返回是否点击玩家</returns>
    public override bool ReleaseCondition()
    {
        return AllSkillManager.Instance.aim.HitGameObject != null && AllSkillManager.Instance.aim.HitGameObject.GetComponent<TankHealth>() != null;
    }
}
