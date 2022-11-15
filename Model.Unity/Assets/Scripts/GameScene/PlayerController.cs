using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace Player 
{
    public class PlayerController : Player
    {
        public static PlayerController Instance;

        [Header("Script")]
        [SerializeField]
        private PlayerStateController _playerStateController;

        [SerializeField]
        private PlayerCameraMovement _cameraMovement;

        [Header("Animation")]
        [SerializeField]
        internal Animator Animator;

        [Header("Audio")]
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _audioClips;

        [Header("Operate Button")]
        [SerializeField]
        internal Button AttackBtn;

        [Header("Position")]
        [SerializeField]
        internal Transform PlayerTransform;

        [Header("Other")]
        [SerializeField]
        private BoxCollider _detectAttackTrigger;
        
        internal bool IsDeath = false;

        private void Awake()
        {
            InitSingleton();
            InitBtnOnClick();
        }

        private void Update()
        {
            CheckMoving();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 攻擊Button註冊切換至alert State
        /// </summary>
        private void InitBtnOnClick()
        {
            AttackBtn.onClick.AddListener(Attack);
        }

        /// <summary>
        /// 刪除攻擊Button註冊(玩家死亡)
        /// </summary>
        internal void RemoveBtnOnClick() 
        {
            AttackBtn.onClick.RemoveListener(Attack);
        }

        /// <summary>
        /// 檢查是否移動中
        /// </summary>
        private void CheckMoving()
        {
            if ((Animator.GetFloat("Speed") == 0f) && (!IsDeath))
            {
                _cameraMovement.enabled = true;
            }
            else
            {
                _cameraMovement.enabled = false;
            }
        }

        /// <summary>
        /// 攻擊
        /// </summary>
        private void Attack() 
        {
            _playerStateController.SwitchAttackState();
        }

        /// <summary>
        /// 播放攻擊音效
        /// </summary>
        private void PlayAttackSFX() 
        {
            _audioSource.clip = _audioClips[0];
            _audioSource.Play();
        }

        /// <summary>
        /// 播放士兵死亡音效
        /// </summary>
        private void PlayDeathSFX()
        {
            _audioSource.clip = _audioClips[1];
            _audioSource.Play();
        }

        /// <summary>
        /// 判定攻擊(Animation Event)
        /// </summary>
        private void DetectAttack()
        {     
            _detectAttackTrigger.enabled = true;    
        }

        /// <summary>
        /// 攻擊結束(Animation Event)
        /// </summary>
        private void AttackEnd()
        {
            _detectAttackTrigger.enabled = false;
        }

        /// <summary>
        /// 切換死亡狀態
        /// </summary>
        internal void DetectDeath()
        {
            bool isCheat = (GameManager.Instance.GameSceneData.IsCheat || CheatPanelController.IsCheat);

            if ((!isCheat) && (!IsDeath))
            {
                IsDeath = true;
                CharacterController.enabled = false;
                _playerStateController.SwitchDeathState();
                RemoveBtnOnClick();
            }
        }
    }
}