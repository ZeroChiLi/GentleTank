using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class MainMenu : MonoBehaviour 
{
    public CinemachineVirtualCamera cmMainMenuCamera;
    public CinemachineVirtualCamera cmArmsMenuCamera;

    public void MainToArms()
    {
        cmMainMenuCamera.enabled = false;
        cmArmsMenuCamera.enabled = true;
    }

    public void BackToMain()
    {
        cmMainMenuCamera.enabled = true;
        cmArmsMenuCamera.enabled = false;
    }
}
