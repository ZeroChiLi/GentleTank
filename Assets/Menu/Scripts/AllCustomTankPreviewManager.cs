using CameraRig;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AllCustomTankPreviewManager : MonoBehaviour 
{
    static private AllCustomTankPreviewManager instance;
    static public AllCustomTankPreviewManager Instance { get { return instance; } }

    public AllCustomTankManager allCustomTank;
    public CatchTextureCam catchTextureCam;
    public IntervalOffsetCam intervalCam;               // 间隔变化相机
    public PostEffectsBase cameraEffect;                // 屏幕后处理特效
    public RenderTexture selectedTexture;               // 选中的预览纹理
    public List<RenderTexture> textureList;             // 纹理列表

    /// <summary>
    /// 获取所有坦克，设置好位置
    /// </summary>
    private void Awake()
    {
        instance = this;
        allCustomTank.CreateAllTanks();
        allCustomTank.SetupAllTanksPosition();
        StartCoroutine(SetupAllTankTexture());
    }

    /// <summary>
    /// 设置当前坦克
    /// </summary>
    private void Start()
    {
        allCustomTank.SelectCurrentTank(0);
    }

    /// <summary>
    /// 配置所有坦克预览纹理
    /// </summary>
    private IEnumerator SetupAllTankTexture()
    {
        for (int i = 0; i < textureList.Count; i++)
        {
            while (catchTextureCam.IsCatching)
                yield return null;
            if (allCustomTank[i] == null)
                continue;
            catchTextureCam.SetCatchTarget(allCustomTank[i].transform, textureList[i]);
        }
    }

    /// <summary>
    /// 捕获新的纹理（当前坦克）
    /// </summary>
    public void CatchNewTexture()
    {
        catchTextureCam.SetCatchTarget(allCustomTank.CurrentTank.transform, textureList[allCustomTank.CurrentIndex]);
    }
}
