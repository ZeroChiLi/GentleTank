using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // 玩家编号
        public float m_Speed = 12f;                 // 移动速度
        public float m_TurnSpeed = 180f;            // 旋转速度
        public AudioSource m_MovementAudio;         // 当前音效
        public AudioClip m_EngineIdling;            // 引擎闲置声音
        public AudioClip m_EngineDriving;           // 引擎移动声音
        public float m_PitchRange = 0.2f;           // 音调浮动范围

        private string m_MovementAxisName;          // 移动输入轴名
        private string m_TurnAxisName;              // 旋转输入轴名
        private Rigidbody m_Rigidbody;              // 刚体
        private float m_MovementInputValue;         // 当前移动值
        private float m_TurnInputValue;             // 当前旋转值
        private float m_OriginalPitch;              // 初始音调值
        private ParticleSystem[] m_particleSystems; // 坦克的所有粒子系统（尾气）

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            m_Rigidbody.isKinematic = false;
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
                m_particleSystems[i].Play();
        }

        private void OnDisable()
        {
            m_Rigidbody.isKinematic = true;

            for (int i = 0; i < m_particleSystems.Length; ++i)
                m_particleSystems[i].Stop();
        }

        private void Start()
        {
            //输入名要在Player Setting 设置
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update()
        {
            m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

            EngineAudio();
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
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f && m_MovementAudio.clip == m_EngineDriving)
                ChangeAudioClipAndPlay(m_EngineIdling);
            else if (m_MovementAudio.clip == m_EngineIdling)
                ChangeAudioClipAndPlay(m_EngineDriving);
        }

        // 改变音效
        private void ChangeAudioClipAndPlay(AudioClip audioClip)
        {
            m_MovementAudio.clip = audioClip;
            m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            m_MovementAudio.Play();
        }

        //移动
        private void Move()
        {
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        //旋转
        private void Turn()
        {
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }
    }
}