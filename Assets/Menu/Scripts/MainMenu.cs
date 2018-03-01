using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class MainMenu : MonoBehaviour 
{
    public PlayableDirector director;
    public CinemachineVirtualCamera cmMainMenuCamera;

    public void MainToArms()
    {
        director.time = 0;
        director.Play();
    }

    public void BackToMain()
    {
        director.time = 0;
        cmMainMenuCamera.enabled = true;
    }
}
