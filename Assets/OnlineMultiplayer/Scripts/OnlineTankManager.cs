using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineTankManager : Photon.MonoBehaviour
{
    private Color playerColor;

    /// <summary>
    /// 渲染颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void RendererColor(Color color)
    {
        playerColor = color;
        MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer == null)
            return;
        for (int i = 0; i < meshRenderer.Length; i++)
            meshRenderer[i].material.color = color;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playerColor.r);
            stream.SendNext(playerColor.g);
            stream.SendNext(playerColor.b);
        }
        else
        {
            playerColor.r = (float)stream.ReceiveNext();
            playerColor.g = (float)stream.ReceiveNext();
            playerColor.b = (float)stream.ReceiveNext();
            RendererColor(playerColor);
        }
    }
}
