using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankFireButton : MonoBehaviour
{
    public TankShooting tankShooting;
    public Image image;
    public Color normal = Color.white;
    public Color hightlighted = new Color(1f, 0.5f, 0.5f, 1f);
    public Color pressed = new Color(0.8f, 0.2f, 0.2f, 1f);
    public Color disable = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private bool readyToFire = false;

    /// <summary>
    /// 更新蓄力
    /// </summary>
    public void Update()
    {
        if (!tankShootingExistAndEnabled() || !tankShooting.CanFire())
            return;
        if (image.color == disable)
            image.color = normal;
        if (readyToFire)
            tankShooting.Charging();
    }

    /// <summary>
    /// 坦克射击是否存在且可以使用
    /// </summary>
    /// <returns></returns>
    public bool tankShootingExistAndEnabled()
    {
        return tankShooting != null && tankShooting.enabled;
    }

    /// <summary>
    /// 鼠标进入
    /// </summary>
    public void PointerEnter()
    {
        if (!tankShootingExistAndEnabled())
            return;
        image.color = hightlighted;
    }

    /// <summary>
    /// 鼠标离开
    /// </summary>
    public void PointerExit()
    {
        if (!tankShootingExistAndEnabled())
            return;
        image.color = normal;
        if (readyToFire)
            Fire();
    }

    /// <summary>
    /// 鼠标按下，准备同时开始蓄力
    /// </summary>
    public void PointerDown()
    {
        if (!tankShootingExistAndEnabled())
            return;
        image.color = pressed;
        tankShooting.Ready();
        readyToFire = true;
    }

    /// <summary>
    /// 鼠标松开
    /// </summary>
    public void PointerUp()
    {
        if (!tankShootingExistAndEnabled())
            return;
        if (readyToFire)
            Fire();
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Fire()
    {
        if (!tankShootingExistAndEnabled())
            return;
        image.color = disable;
        tankShooting.Fire();
        readyToFire = false;
    }
}
