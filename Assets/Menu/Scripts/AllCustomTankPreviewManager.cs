using CameraRig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCustomTankPreviewManager : MonoBehaviour 
{
    static private AllCustomTankPreviewManager instance;
    static public AllCustomTankPreviewManager Instance { get { return instance; } }

    public AllCustomTankManager allCustomTank;          // 所有坦克管理器
    public CatchTextureCam catchTextureCam;             // 捕获纹理相机设备
    public IntervalOffsetCam intervalCam;               // 间隔变化相机
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
