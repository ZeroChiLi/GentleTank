
namespace CrossPlatformInput
{
    public class ChargeButtonInput : ButtonInput
    {
        public ChargeButtonMananger chargeButtonManager;
        public new float Value { get { return chargeButtonManager.CurrentValue; } }

        /// <summary>
        /// 登记按钮
        /// </summary>
        private void Awake()
        {
            Register();
        }

        /// <summary>
        /// 按钮按住时更新信息
        /// </summary>
        private void Update()
        {
            if (chargeButtonManager != null && chargeButtonManager.isCharging)
                OnButtonPressed();
        }

        /// <summary>
        /// 配置按钮信息
        /// </summary>
        /// <param name="coolDown">冷却时间</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="rate">数值变化量</param>
        public void Setup(float coolDown, float min, float max, float rate)
        {
            chargeButtonManager.coolDownTime = coolDown;
            chargeButtonManager.minValue = min;
            chargeButtonManager.maxValue = max;
            chargeButtonManager.chargeRate = rate;
            chargeButtonManager.Init();
        }

        public override void ButtonDownHandle() { }

        public override void ButtonPressedHandle() { }

        public override void ButtonUpHandle() { }

    }
}