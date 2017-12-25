using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class AllSkillManager : MonoBehaviour
    {
        public static AllSkillManager Instance;                 // 技能管理单例

        public Aim aim;                                         // 瞄准光标
        public List<SkillObject> skillObjectList;               // 技能对象列表

        private List<SkillManager> skillManagerList;            // 技能通常控制列表
        private bool allSkillEnable = false;                    // 所有技能是否有效
        private SkillManager currentSkill;
        private bool clickedSkillSameFrame;

        /// <summary>
        /// 获取大小位置，每个技能按键大小，距离间隔
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameRound.Instance.OnGameRoundStartEvent.AddListener(() => { SetAllSkillEnable(true); });
            GameRound.Instance.OnGameRoundEndEvent.AddListener(() => { aim.SetActive(false); SetAllSkillEnable(false); });

            skillManagerList = new List<SkillManager>();
            for (int i = 0; i < skillObjectList.Count; i++)
                skillManagerList.Add(skillObjectList[i].CreateSkillButton(transform).GetComponent<SkillManager>());
        }

        /// <summary>
        /// 获取鼠标位置，当点击时，改变技能状态
        /// </summary>
        private void Update()
        {
            //不在回合中就跳出
            if (!GameRound.Instance.IsGamePlaying)
                return;

            // clickedSkillSameFrame避免同一帧在点击按钮时同时释放了技能
            if (!clickedSkillSameFrame && currentSkill != null && Input.GetMouseButtonUp(0))
                SceneOnClicked();
            clickedSkillSameFrame = false;
        }

        /// <summary>
        /// 设置所有技能是否有效
        /// </summary>
        /// <param name="enable">是否有效</param>
        private void SetAllSkillEnable(bool enable)
        {
            if (allSkillEnable == enable)
                return;
            allSkillEnable = enable;
            for (int i = 0; i < skillManagerList.Count; i++)
                skillManagerList[i].SetSkillEnable(enable);
        }

        /// <summary>
        /// 点击了场景（除了技能按钮）
        /// </summary>
        private void SceneOnClicked()
        {
            //已经点击了技能，且满足释放条件（过了冷却时间和满足自定义条件）就释放该技能
            if (currentSkill != null && currentSkill.CanRelease())
            {
                currentSkill.Release();
                aim.SetActive(false);
                currentSkill = null;
            }
        }

        public void SkillManangerClicked(SkillManager target)
        {
            if (currentSkill == null)
            {
                if (target.CanReady())
                {
                    currentSkill = target;
                    currentSkill.Ready();
                    aim.SetActive(true);
                    aim.SetAimMode(currentSkill.skill.aimMode);
                    clickedSkillSameFrame = true;
                }
            }
            else
            {
                currentSkill.Cancel();
                aim.SetActive(false);
                currentSkill = null;
            }
        }
    }
}
