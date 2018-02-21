using CameraRig;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Skill
{
    public class Aim : MonoBehaviour
    {
        public AimMode aimMode;                         // 当前瞄准模型
        public Image groundAimImage;                    // 显示在世界区域的瞄准图片

        public Vector3 HitPosition { get { return inputHitPos; } }                  //获取指中目标位置
        public GameObject HitGameObject { get { return inputHitGameObject; } }      //获取指中对象
        public PlayerManager HitPlayer { get { return inputHitGameObject == null ? null : inputHitGameObject.GetComponentInParent<PlayerManager>(); } }

        protected bool aimEnable = true;                // 瞄准是否有效

        private Image aimImage;                         // 当前瞄准图片
        private Vector3 inputHitPos;                    // 鼠标射线射到的点
        private GameObject inputHitGameObject;          // 射线射到的物体
        private TagWithColor tagWithColor;              // 射到物体的标签对应瞄准模型

        private DrawOutline outline;             // 当前相机对应描边特效

        /// <summary>
        /// 获取图片组件
        /// </summary>
        private void Awake()
        {
            aimImage = GetComponent<Image>();
            aimImage.color = aimMode.normalColor;
            SetAimMode(aimMode);
        }

        /// <summary>
        /// 获取描边组件
        /// </summary>
        private void Start()
        {
            outline = MainCameraRig.Instance.camera.GetComponentInChildren<DrawOutline>();
        }

        /// <summary>
        /// 更新瞄准获取的对象值
        /// </summary>
        private void Update()
        {
            SetPosition(Input.mousePosition);
            RaycastObject();
            UpdateAimColor();
        }

        /// <summary>
        /// 在消失时顺便把描边效果清除掉
        /// </summary>
        private void OnDisable()
        {
            outline.targets.Clear();
        }

        /// <summary>
        /// 以鼠标位置从屏幕射线
        /// </summary>
        private void RaycastObject()
        {
            RaycastHit info;
            if (Physics.Raycast(MainCameraRig.Instance.camera.ScreenPointToRay(gameObject.transform.position), out info, 200))
            {
                inputHitPos = info.point;
                inputHitGameObject = info.collider.gameObject;
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void UpdateAimColor()
        {
            if (aimEnable == false)                                 // 如果失效，设置为失效颜色
            {
                aimImage.color = aimMode.disableColor;
                return;
            }
            if (inputHitGameObject == null)
                return;
            tagWithColor = aimMode.GetTagWithColorByTag(inputHitGameObject.tag); // 如果模型有定义该标签的颜色，修改之
            if (tagWithColor != null)
            {
                aimImage.color = tagWithColor.color;
                outline.outlineColor = aimImage.color;

                if (!outline.targets.Contains(HitPlayer.gameObject))
                    outline.targets.Add(HitPlayer.gameObject);
            }
            else
            {
                aimImage.color = aimMode.normalColor;                   // 都没有就改成默认颜色
                outline.targets.Clear();
            }
        }

        /// <summary>
        /// 设置当前瞄准模式
        /// </summary>
        /// <param name="aimMode">瞄准模式</param>
        public void SetAimMode(AimMode aimMode)
        {
            this.aimMode = aimMode;
            aimImage.rectTransform.sizeDelta = Vector2.one * aimMode.screenSpriteRadius * 2;
            aimImage.enabled = aimMode.showInScreen;
            aimImage.sprite = aimMode.screenSprite;
            groundAimImage.rectTransform.sizeDelta = Vector2.one * aimMode.groundSpriteRadius * 2;
            groundAimImage.enabled = aimMode.showInGround;
            groundAimImage.sprite = aimMode.groundSprite;
        }

        /// <summary>
        /// 设置对象激活状态
        /// </summary>
        /// <param name="active">是否激活</param>
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            groundAimImage.gameObject.SetActive(active);
        }

        /// <summary>
        /// 设置瞄准图片是否有效
        /// </summary>
        /// <param name="enable">是否有效</param>
        public void SetEnable(bool enable)
        {
            aimEnable = enable;
        }

        /// <summary>
        /// 设置瞄准图片位置
        /// </summary>
        /// <param name="position">位置</param>
        public void SetPosition(Vector3 position)
        {
            gameObject.transform.position = position;
            groundAimImage.transform.position = inputHitPos + Vector3.up * 0.5f;
        }
    }
}
