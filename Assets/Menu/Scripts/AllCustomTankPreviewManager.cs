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
    public IntervalOffsetCam intervalCam;               // 间隔变化相机
    public PostEffectsBase cameraEffect;                // 屏幕后处理特效
    public RenderTexture selectedTexture;               // 选中的预览纹理
    public List<RenderTexture> textureList;             // 纹理列表

    private WaitForSeconds delayTime = new WaitForSeconds(0.05f);   // 渲染延迟

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
            yield return CameraCapture(textureList[i], i, true);
        yield return CameraCapture(selectedTexture, 0, false);
        intervalCam.enableSmooth = true;
    }

    /// <summary>
    /// 捕获相机的画面到指定纹理中
    /// </summary>
    /// <param name="targetTexture">目标纹理</param>
    /// <param name="index">当前索引</param>
    /// <param name="effectActive">是否开启屏幕特效</param>
    private IEnumerator CameraCapture(RenderTexture targetTexture,int index,bool effectActive = false)
    {
        intervalCam.gameObject.SetActive(false);
        cameraEffect.enabled = effectActive;
        intervalCam.camera.targetTexture = targetTexture;
        intervalCam.FollowImmediately(index);
        intervalCam.gameObject.SetActive(true);
        intervalCam.camera.Render();
        yield return delayTime;
    }

}
