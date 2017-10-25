using UnityEngine;

public abstract class ModuleBase : ScriptableObject
{
    [System.Serializable]
    public struct ModuleAnchors
    {
        public Vector3 center;
        public Vector3 forward;
        public Vector3 back;
        public Vector3 left;
        public Vector3 right;
        public Vector3 up;
        public Vector3 down;
    }

    public Vector3 MinAnchor { get { return new Vector3(anchors.left.x, anchors.down.y, anchors.back.z); } }
    public Vector3 MaxAnchor { get { return new Vector3(anchors.right.x, anchors.up.y, anchors.forward.z); } }

    public string moduleName;
    public GameObject prefab;
    public Sprite preview;
    public ModuleAnchors anchors;

    public abstract string GetProperties();

    public Bounds GetBounds()
    {
        Bounds bounds = new Bounds();
        bounds.center = anchors.center;
        bounds.SetMinMax(MinAnchor,MaxAnchor);
        return bounds;
    }
}
