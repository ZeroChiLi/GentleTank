using UnityEngine;

[System.Serializable]
public class Point
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Quaternion rotation { get { return Quaternion.Euler(eulerAngles); } set { eulerAngles = value.eulerAngles; } }

    public Point() { }
    public Point(Vector3 position, Quaternion rotation) { this.position = position; this.rotation = rotation; }
    public Point(Vector3 position, Vector3 eulerAngles) { this.position = position; this.eulerAngles = eulerAngles; }
}

