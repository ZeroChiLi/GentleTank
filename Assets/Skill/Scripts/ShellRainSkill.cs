using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShellRainSkill : Skill
{
    public ObjectPool shellPool;            //炮弹池
    [Range(0, 10f)]
    public float skillDelay = 1.5f;         //技能释放延迟
    [Range(1, 100)]
    public int skillLevel = 1;              //技能等级
    [Range(1, 100f)]
    public float attackRadius = 5f;         //技能攻击范围半径
    [Range(0, 1f)]
    public float attackRate = 0.5f;         //技能每次释放频率
    [Range(0, 100f)]
    public float attackDamage = 30f;        //每一粒炮弹最大伤害
    public Image warnningAreaImage;    //警告区域

    /// <summary>
    /// 技能效果
    /// </summary>
    public override IEnumerator SkillEffect()
    {
        ShowWarnningArea(inputHitPos);
        yield return new WaitForSeconds(skillDelay);
        for (int i = 0; i < skillLevel; i++)
            yield return CreateShell(inputHitPos, Random.insideUnitCircle * attackRadius);
        yield return HideWarnningArea(1f);

    }

    /// <summary>
    /// 每隔一段时间创建炮弹
    /// </summary>
    /// <param name="randomCircle">创建的XZ坐标</param>
    /// <returns></returns>
    private IEnumerator CreateShell(Vector3 inputPosition, Vector2 randomCircle)
    {
        //创建炮弹 从上而下
        GameObject shell = shellPool.GetNextObjectActive();
        shell.transform.position = new Vector3(inputPosition.x + randomCircle.x, 20f, inputPosition.z + randomCircle.y);
        shell.transform.rotation = Quaternion.Euler(new Vector3(180f, 0, 0));
        shell.GetComponent<Rigidbody>().velocity = new Vector3(0, -20f, 0);
        shell.GetComponent<Shell>().maxDamage = attackDamage;
        yield return new WaitForSeconds(Random.Range(0, attackRate));
    }

    private void ShowWarnningArea(Vector3 position)
    {
        warnningAreaImage.gameObject.SetActive(true);
        warnningAreaImage.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
        warnningAreaImage.rectTransform.sizeDelta = new Vector2(attackRadius * 2, attackRadius * 2);
    }

    private IEnumerator HideWarnningArea(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        warnningAreaImage.gameObject.SetActive(false);
    }
}
