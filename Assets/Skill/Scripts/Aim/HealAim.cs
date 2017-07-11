
public class HealAim : Aim
{
    /// <summary>
    /// 指中Player就是友好状态，其他就是普通状态
    /// </summary>
    /// <returns>转换后的状态</returns>
    protected override AimState ConvertState()
    {
        if (!aimEnable)
            return AimState.Disable;
        if (HitGameObject.CompareTag("Player"))
            return AimState.Friendly;
        return AimState.Normal;
    }
}
