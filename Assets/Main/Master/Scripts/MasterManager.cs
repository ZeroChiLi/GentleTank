using UnityEditor;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    static public MasterManager Instance { get; private set; }
    public MasterData data;

    public TankAssembleManager SelectedTank
    {
        get { return data.selectedTank; }
        set
        {
            data.selectedTank = value;
            EditorUtility.SetDirty(data);
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

}
