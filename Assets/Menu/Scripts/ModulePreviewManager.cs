using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ModulePreviewManager : MonoBehaviour 
{
    public ModuleBase target;
    public Image previewImage;

    /// <summary>
    /// 绑定对应部件
    /// </summary>
    /// <param name="target">目标部件</param>
    virtual public void SetTarget(ModuleBase target)
    {
        this.target = target;
        previewImage.sprite = target.preview;
    }

    abstract public void OnClicked();

    abstract public void OnEnter();

    abstract public void OnExit();

}
