#if UNITY_EDITOR
using UnityEditor;
#endif
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
                MasterManager master = Instantiate(new GameObject("MasterManager")).AddComponent<MasterManager>();
                master.data = AssetDatabase.LoadAssetAtPath<MasterData>(string.Format("Assets/Main/Master/ScriptableObject/MasterData.asset"));
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
            EditorUtility.SetDirty(data);
        }
    }

    public GameObject StandardPrefab { get { return data.standardPerfab; } set { data.standardPerfab = value; } }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

}
