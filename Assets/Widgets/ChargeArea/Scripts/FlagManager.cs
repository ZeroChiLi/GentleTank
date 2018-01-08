using UnityEngine;

namespace Widget.ChargeArea
{
    public class FlagManager : MonoBehaviour
    {
        public GameObject flagPole;             // 旗杆，通过旋转旗杆达到旗帜面对镜头
        public MeshRenderer flagMeshRender;     // 旗帜的渲染网格

        private Material flagMaterial;          // 旗帜的材质，拿来改颜色

        /// <summary>
        /// 旋转旗杆，使旗帜面对镜头
        /// </summary>
        private void Awake()
        {
            flagMaterial = flagMeshRender.material;
        }

        /// <summary>
        /// 更新旗杆旋转
        /// </summary>
        private void FixedUpdate()
        {
            if (Camera.current != null)
                flagPole.transform.rotation = Quaternion.Euler(0f, Camera.current.transform.rotation.eulerAngles.y, 0f);
        }

        /// <summary>
        /// 设置旗帜颜色
        /// </summary>
        /// <param name="color">旗帜颜色</param>
        public void SetFlagColor(Color color)
        {
            flagMaterial.color = color;
        }

    }
}
