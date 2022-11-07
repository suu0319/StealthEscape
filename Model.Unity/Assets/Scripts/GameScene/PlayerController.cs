using UnityEngine;
using UnityEngine.UI;
using GameOver;
using Manager;

namespace Player 
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _characterController;

        [SerializeField]
        private PlayerStateController _playerStateController;

        [SerializeField]
        private PlayerCameraMovement _cameraMovement;

        [SerializeField]
        private BoxCollider _detectAttackTrigger;

        [SerializeField]
        internal Animator Animator;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _audioClips;

        [SerializeField]
        internal Transform PlayerTransform;

        [SerializeField]
        internal Button AttackBtn, OptionBtn;

        internal bool IsDeath = false;

        private static PlayerController _instance;
        public static PlayerController Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            Init();
            InitBtnOnClick();
        }

        private void Update()
        {
            CheckMoving();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void Init()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
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
                _characterController.enabled = false;
                RemoveBtnOnClick();
                _playerStateController.SwitchDeathState();
                GameOverPanel.Instance.AppearGameOverScreen();
                GameStateController.Instance.SwitchGameOverState();
            }
        }
    }
}