using CameraRig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCustomTankPreviewManager : MonoBehaviour 
{
    static public AllCustomTankPreviewManager Instance { get; private set; }

    public AllCustomTankManager allCustomTank;          // 所有坦克管理器
    public CatchTextureCameraRig catchTextureCam;             // 捕获纹理相机设备
    public IntervalOffsetCameraRig intervalCam;               // 间隔变化相机
    public RenderTexture selectedTexture;               // 选中的预览纹理
    public List<RenderTexture> textureList;             // 纹理列表

    public event EventHandler allTankSetupHandle;       // 所有坦克配置完后事件

    /// <summary>
    /// 获取所有坦克，设置好位置
    /// </summary>
    private void Awake()
    {
        Instance = this;
        allCustomTank.CreateAllTanks();
        allCustomTank.SetupAllTanksPosition();
        StartCoroutine(SetupAllTankTexture());
    }

    /// <summary>
    /// 当所有坦克配置好之后响应
    /// </summary>
    private void OnAllTankSetup()
    {
        allTankSetupHandle(this, null);
    }

    /// <summary>
    /// 配置所有坦克预览纹理
    /// </summary>
    public IEnumerator SetupAllTankTexture()
    {
        for (int i = 0; i < textureList.Count; i++)
        {
            if (allCustomTank[i] == null)
            {
                textureList[i].Release();
                continue;
            }

            while (catchTextureCam.IsCatching)
                yield return null;
            catchTextureCam.SetCatchTarget(allCustomTank[i].transform, textureList[i]);
        }
        OnAllTankSetup();
    }

    /// <summary>
    /// 捕获当前坦克的纹理
    /// </summary>
    public void CatchCurrentTankTexture()
    {
        CatchTankTexture(allCustomTank.CurrentIndex);
    }

    /// <summary>
    /// 捕获新的纹理
    /// </summary>
    /// <param name="index">指定索引值</param>
    public void CatchTankTexture(int index)
    {
        if (allCustomTank[index] != null)
            catchTextureCam.SetCatchTarget(allCustomTank[index].transform, textureList[index]);
    }

}
