using UnityEngine;

/// <summary>
/// 一段时间后，自动设置为不激活状态
/// </summary>
public class ActiveForSeconds : MonoBehaviour 
{
    public float lifeTime = 5f;

    private float currentTime;

    private void OnEnable()
    {
        currentTime = 0f;
    }

    private void Update()
    {
        if (currentTime > lifeTime)
            gameObject.SetActive(false);
        currentTime += Time.deltaTime;
    }

}
