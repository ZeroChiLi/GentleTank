using UnityEngine;

public class MasterManager : MonoBehaviour 
{
    public MasterData data;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
