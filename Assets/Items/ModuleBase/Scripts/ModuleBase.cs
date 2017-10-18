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

    public string moduleName;
    public GameObject prefab;
    public Sprite preview;
    public ModuleAnchors anchors;

    public abstract string GetProperties();
}
