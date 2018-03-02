using CameraRig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllCustomTankPreviewManager : MonoBehaviour 
{
    static public AllCustomTankPreviewManager Instance { get; private set; }

    public CatchTextureCameraRig catchTextureCam;             // 捕获纹理相机设备
    //public IntervalOffsetCameraRig intervalCam;               // 间隔变化相机
    public RenderTexture selectedTexture;               // 选中的预览纹理
    public SelectedImageManager selectedImage;
    public List<RenderTexture> textureList;             // 纹理列表
    public UnityEvent OnAllTankSetupEvent;              // 所有坦克配置好后响应
    public UnityEvent OnTankPreviewClickedEvent;

    /// <summary>
    /// 获取所有坦克，设置好位置
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AllCustomTankManager.Instance.CreateAllTanks();
        AllCustomTankManager.Instance.SetupAllTanksPosition();
        SetupAllTankTexture();
    }

    /// <summary>
    /// 配置所有坦克预览纹理
    /// </summary>
    public void SetupAllTankTexture()
    {
        for (int i = 0; i < textureList.Count; i++)
        {
            if (AllCustomTankManager.Instance[i] == null)
                textureList[i].Release();
            else
                catchTextureCam.RenderTarget(AllCustomTankManager.Instance[i].transform, textureList[i]);
        }
        OnAllTankSetupEvent.Invoke();
    }

    /// <summary>
    /// 捕获当前坦克的纹理
    /// </summary>
    public void CatchCurrentTankTexture()
    {
        CatchTankTexture(AllCustomTankManager.Instance.CurrentIndex);
    }

    /// <summary>
    /// 捕获新的纹理
    /// </summary>
    /// <param name="index">指定索引值</param>
    public void CatchTankTexture(int index)
    {
        if (AllCustomTankManager.Instance[index] != null)
            catchTextureCam.RenderTarget(AllCustomTankManager.Instance[index].transform, textureList[index]);
    }

    /// <summary>
    /// 选择当前坦克的UI效果
    /// </summary>
    public void SelectedCurrentTankUI()
    {
        if (AllCustomTankManager.Instance.CurrentTank == null)
            return;
        selectedImage.SetTargetImmediately(AllCustomTankManager.Instance.CurrentIndex);
        //intervalCam.SetTargetImmediately(AllCustomTankManager.Instance.CurrentIndex);
    }

    /// <summary>
    /// 当坦克预览点击时响应
    /// </summary>
    public void OnTankPreviewClicked()
    {
        OnTankPreviewClickedEvent.Invoke();
    }
}
