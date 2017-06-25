using System.Collections;
using UnityEngine;

public class ThunderSkill : Skill
{
    public ObjectPool shellPool;            //炮弹池
    [Range(1, 100)]
    public int skillLevel = 1;              //技能等级
    [Range(1, 100)]
    public float attackRadius = 5f;         //技能攻击范围半径
    [Range(0, 1f)]
    public float attackRate = 0.5f;         //技能每次释放频率

    /// <summary>
    /// 技能效果
    /// </summary>
    public override IEnumerator SkillEffect()
    {
        for (int i = 0; i < skillLevel; i++)
            yield return CreateShell(inputHitPos, Random.insideUnitCircle * attackRadius);
    }

    /// <summary>
    /// 每隔一段时间创建炮弹
    /// </summary>
    /// <param name="randomCircle">创建的XZ坐标</param>
    /// <returns></returns>
    public IEnumerator CreateShell(Vector3 inputPosition, Vector2 randomCircle)
    {
        //创建炮弹 从上而下
        GameObject shell = shellPool.GetNextObjectActive();
        shell.transform.position = new Vector3(inputPosition.x + randomCircle.x, 20f, inputPosition.z + randomCircle.y);
        shell.transform.rotation = Quaternion.Euler(new Vector3(180f, 0, 0));
        shell.GetComponent<Rigidbody>().velocity = new Vector3(0, -20f, 0);
        yield return new WaitForSeconds(Random.Range(0, attackRate));
    }
}
