using UnityEngine;

namespace Widget
{
    public class ArrowBillboard : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        private Material arrowMaterial;
        public Material ArrowMaterial { get { return arrowMaterial = arrowMaterial == null ? meshRenderer.material : arrowMaterial; } }

        public void Setup(Vector3 pos,Color color)
        {
            transform.position = pos;
            ArrowMaterial.SetColor("_Color", color);
        }
    }
}