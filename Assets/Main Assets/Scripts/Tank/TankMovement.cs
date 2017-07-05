using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed = 12f;                   // 移动速度
    public float turnSpeed = 180f;              // 旋转速度
    public AudioSource movementAudio;           // 当前音效
    public AudioClip engineIdling;              // 引擎闲置声音
    public AudioClip engineDriving;             // 引擎移动声音
    public float pitchRange = 0.2f;             // 音调浮动范围

    private TankInformation tankInfo;           // 坦克信息
    private string movementAxisName;            // 移动输入轴名
    private string turnAxisName;                // 旋转输入轴名
    private Rigidbody tankRigidbody;            // 刚体
    private float movementInputValue;           // 当前移动值
    private float turnInputValue;               // 当前旋转值
    private float originalPitch;                // 初始音调值
    private ParticleSystem[] particleSystems;   // 坦克的所有粒子系统（尾气）

    private void Awake()
    {
        tankInfo = GetComponent<TankInformation>();
        tankRigidbody = GetComponent<Rigidbody>();
        originalPitch = movementAudio.pitch;
    }

    private void OnEnable()
    {
        tankRigidbody.isKinematic = false;
        movementInputValue = 0f;
        turnInputValue = 0f;

        particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; ++i)
            particleSystems[i].Play();
    }

    private void OnDisable()
    {
        tankRigidbody.isKinematic = true;

        for (int i = 0; i < particleSystems.Length; ++i)
            particleSystems[i].Stop();
    }

    // 获取移动、旋转值
    private void Update()
    {
        movementInputValue = Input.GetAxis(movementAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);

        EngineAudio();
    }

    //设置编号
    public void SetupPlayerInput()
    {
        //输入名要在Player Setting 设置
        movementAxisName = "Vertical" + tankInfo.playerID;
        turnAxisName = "Horizontal" + tankInfo.playerID;
    }

    // 移动、旋转
    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    // 坦克引擎声音
    private void EngineAudio()
    {
        // 如果从移动变化到静止状态（包括旋转），关掉移动音效，开启闲置音效
        if (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f && movementAudio.clip == engineDriving)
            ChangeAudioClipAndPlay(engineIdling);
        else if (movementAudio.clip == engineIdling)
            ChangeAudioClipAndPlay(engineDriving);
    }

    // 改变音效
    private void ChangeAudioClipAndPlay(AudioClip audioClip)
    {
        movementAudio.clip = audioClip;
        movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
        movementAudio.Play();
    }

    //移动
    private void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        tankRigidbody.MovePosition(tankRigidbody.position + movement);
    }

    //旋转
    private void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        tankRigidbody.MoveRotation(tankRigidbody.rotation * turnRotation);
    }
}
