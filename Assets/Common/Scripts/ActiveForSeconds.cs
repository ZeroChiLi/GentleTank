using UnityEngine;

/// <summary>
/// 一段时间后，自动设置为不激活状态
/// </summary>
public class ActiveForSeconds : MonoBehaviour 
{
    public float lifeTime = 5f;

    private float currentTime;

    protected void OnEnable()
    {
        currentTime = 0f;
    }

    protected void LateUpdate()
    {
        if (currentTime > lifeTime)
            gameObject.SetActive(false);
        currentTime += Time.deltaTime;
    }

}
