using UnityEditor;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    static private MasterManager instance;
    static public MasterManager Instance
    {
        get
        { 
            if (instance == null)
            {
                MasterManager master = new GameObject("MasterManager").AddComponent<MasterManager>();
                master.data = Resources.Load<MasterData>("MasterData");
                //master.data = AssetDatabase.LoadAssetAtPath<MasterData>(string.Format("Assets/Main/Master/ScriptableObject/MasterData.asset"));
                instance = master;
            }
            return instance;
        }
        private set { instance = value; }
    }
    public MasterData data;

    public TankAssembleManager SelectedTank
    {
        get { return data.selectedTank; }
        set
        {
            data.selectedTank = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(data);
#endif
        }
    }

    public GameObject StandardPrefab { get { return data.standardPerfab; } set { data.standardPerfab = value; } }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

}
