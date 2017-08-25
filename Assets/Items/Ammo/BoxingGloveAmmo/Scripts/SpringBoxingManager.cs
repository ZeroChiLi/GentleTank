using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class SpringBoxingManager : AmmoBase
    {
        public Collider glove;                  // 拳套
        public GameObject spring;               // 弹簧
        public SpringJoint baseAnchor;          // 弹簧锚点

        private Vector3 lastGlovePos;           // 上一帧拳套位置（本地）
        private Vector3 gloveStartPos;          // 起始拳套位置（本地）
        private Vector3 springStartScale;       // 弹簧起始缩放（本地）

        private float scaleFactor = 200f;       // 弹簧缩放随拳套位置变化的缩放因子

        /// <summary>
        /// 初始化记录位置信息
        /// </summary>
        private void Start()
        {
            durability = int.MaxValue;
            gloveStartPos = glove.transform.localPosition;
            lastGlovePos = gloveStartPos;
            springStartScale = spring.transform.localScale;
        }

        /// <summary>
        /// 更新拳套是否启用BoxCollider（仅出拳时有效，不动或者回来时无效），更新弹簧缩放
        /// </summary>
        private void Update()
        {
            if (glove.transform.localPosition.z == lastGlovePos.z)
                return;

            if (glove.transform.localPosition.z < gloveStartPos.z)
                ResetSpringGlove();

            if (lastGlovePos.z > glove.transform.localPosition.z)
                glove.enabled = false;
            else if (lastGlovePos.z < glove.transform.localPosition.z)
                glove.enabled = true;

            lastGlovePos = glove.transform.localPosition;
            UpdateSpringScale();
        }

        /// <summary>
        /// 重置弹簧拳到原始位置
        /// </summary>
        private void ResetSpringGlove()
        {
            glove.transform.localPosition = gloveStartPos;
            spring.transform.localScale = springStartScale;
            if(ammoRb != null)
                ammoRb.Sleep();
        }

        /// <summary>
        /// 更新弹簧缩放
        /// </summary>
        private void UpdateSpringScale()
        {
            spring.transform.localScale = new Vector3(springStartScale.x, springStartScale.y, springStartScale.z + scaleFactor * (glove.transform.localPosition.z - gloveStartPos.z));
        }

        protected override IEnumerator OnCollision(Collider other)
        {
            yield return null;
        }

        protected override IEnumerator OnCrashed()
        {
            yield return null;
        }
    }
}