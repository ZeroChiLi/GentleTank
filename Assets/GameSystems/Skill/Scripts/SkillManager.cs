using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Skill
{
    public class SkillManager : AttackManager
    {
        public bool needReady = true;                   // 需要准备（两次点击后才释放技能），否则一次直接释放
        public Slider slider;                           // 滑动条
        public Image fullImage;                         // 滑动条图片
        public Image emptyImage;                        // 滑动条背景图片
        public SkillObject skill;                       // 技能

        protected bool isReady = false;                 // 是否准备释放技能（第一次点击）
        protected bool gamePlaying = false;             // 是否正在游戏中

        private Image buttonImage;                                      // 按钮图片
        private Color normalColor = Color.white;                        // 默认颜色
        private Color hightLightColor = new Color(1, 1, 0.45f);         // 高亮颜色
        private Color pressedColor = new Color(1, 0.27f, 0.27f);        // 点击颜色
        private Color disableColor = new Color(0.46f, 0.46f, 0.46f);    // 失效颜色
        private Coroutine currentSkillCoroutine;                         // 当前技能释放时的协程
        private bool lastFrameIsCooling;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Start()
        {
            buttonImage = GetComponent<Image>();
            slider.maxValue = skill.coolDownTime;
            coolDownTime = skill.coolDownTime;
            buttonImage.color = disableColor;
        }

        /// <summary>
        /// 通过外部初始化技能管理器
        /// </summary>
        public void InitSkillManager(SkillObject skill, Sprite fullSprite, Sprite emptySprite)
        {
            this.skill = skill;
            fullImage.sprite = fullSprite;
            emptyImage.sprite = emptySprite;
        }

        /// <summary>
        /// 鼠标移入按钮时响应
        /// </summary>
        public void OnPointerEnter()
        {
            if (!IsTimeUp || isReady)       //如果还在冷却时间，保持灰色；如果还在准备状态，保持高亮。
                return;
            buttonImage.color = hightLightColor;
        }

        /// <summary>
        /// 鼠标移出按钮时响应
        /// </summary>
        public void OnPointerExit()
        {
            if (!IsTimeUp || isReady)
                return;
            buttonImage.color = normalColor;
        }

        /// <summary>
        /// 鼠标点击按钮时通知AllSkillManager
        /// </summary>
        public void OnPointerClick()
        {
            AllSkillManager.Instance.SkillManangerClicked(this);
        }

        /// <summary>
        /// 更新冷却时间，同时改变按钮颜色和滑动条长度
        /// </summary>
        protected void Update()
        {
            if (GameRound.Instance.CurrentGameState != GameState.Playing)
                return;

            if (!IsTimeUp)
            {
                buttonImage.color = disableColor;
                slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, CDTimer.GetPercent());
                lastFrameIsCooling = true;
            }
            else
            {
                if (lastFrameIsCooling)
                {
                    buttonImage.color = normalColor;
                    slider.value = slider.maxValue;
                }
                lastFrameIsCooling = false;
            }
        }

        /// <summary>
        /// 是否可以进入准备状态，冷却时间过了
        /// </summary>
        /// <returns></returns>
        public bool CanReady()
        {
            return IsTimeUp;
        }

        /// <summary>
        /// 准备释放技能状态
        /// </summary>
        public void Ready()
        {
            isReady = true;
            buttonImage.color = pressedColor;
        }

        /// <summary>
        /// 取消准备释放技能状态
        /// </summary>
        public void Cancel()
        {
            isReady = false;
            buttonImage.color = normalColor;
        }

        /// <summary>
        /// 设置技能能否使用
        /// </summary>
        /// <param name="enable">是否激活技能</param>
        /// <returns>返回是否激活</returns>
        public void SetSkillEnable(bool enable)
        {
            if (enable)
            {
                buttonImage.color = normalColor;
                CDTimer.Start();
            }
            else if (!enable)
            {
                slider.value = slider.minValue;
                CDTimer.Reset(coolDownTime, true);
                buttonImage.color = disableColor;
                isReady = false;
                Stop();             // 停止掉技能协程
            }
        }

        /// <summary>
        /// 是否可释放技能（冷却时间结束，已经点击了技能，鼠标不在按钮上面）
        /// </summary>
        /// <returns>返回True，可以释放技能</returns>
        public bool CanRelease()
        {
            if (!needReady)     // 如果不需要准备，那就自动设置为准备状态
                Ready();
            return IsTimeUp && isReady && skill.ReleaseCondition();
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        public void Release()
        {
            isReady = false;
            CDTimer.Start();
            Cancel();
            if (skill != null)
                currentSkillCoroutine = StartCoroutine(skill.SkillEffect());
        }

        /// <summary>
        /// 终止当前技能释放
        /// </summary>
        public void Stop()
        {
            if (currentSkillCoroutine != null)
                StopCoroutine(currentSkillCoroutine);
        }

        protected override void OnAttack(params object[] values)
        {
            Release();
        }
    }
}
