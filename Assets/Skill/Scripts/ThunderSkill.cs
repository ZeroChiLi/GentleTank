using System.Collections;
using UnityEngine;

public class ThunderSkill : Skill
{
    public ObjectPool shellPool;            //炮弹池
    public int skillLevel = 1;              //技能等级
    public float attackRadius = 5f;         //技能攻击范围半径
    [Range(0,1f)]
    public float attackRate = 0.5f;         //技能每次释放频率

    /// <summary>
    /// 技能效果
    /// </summary>
    public override IEnumerator SkillEffect()
    {
        for (int i = 0; i < skillLevel; i++)
            yield return CreateShell(Random.insideUnitCircle * attackRadius);
    }

    /// <summary>
    /// 每隔一段时间创建炮弹
    /// </summary>
    /// <param name="position">创建的XZ坐标</param>
    /// <returns></returns>
    public IEnumerator CreateShell(Vector2 position)
    {
        GameObject shell = shellPool.GetNextObjectActive();
        shell.transform.position = new Vector3(inputHitPos.x + position.x, 20f, inputHitPos.z + position.y);
        shell.transform.rotation = Quaternion.Euler(new Vector3(180f, 0, 0));
        shell.GetComponent<Rigidbody>().velocity = new Vector3(0, -20f, 0);
        yield return new WaitForSeconds(Random.Range(0, attackRate));
    }

}
