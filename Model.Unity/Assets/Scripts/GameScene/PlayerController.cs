using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace Player
{
    public class PlayerController : MonoBehaviour, IModelEvent, IExposureSubject
    {
        public static PlayerController Instance;

        public List<IExposureObserver> EnemyList { get; set; }

        [Header("Script")]
        [SerializeField]
        private PlayerStateController _playerStateController;

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
        private CharacterController _characterController;
        [SerializeField]
        private BoxCollider _detectAttackTrigger;
        
        internal bool IsDeath = false;

        public PlayerController() 
        {
            EnemyList = new List<IExposureObserver>();
        }

        private void Awake()
        {
            InitSingleton();
            InitBtnOnClick();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 增加敵人觀察者
        /// </summary>
        /// <param name="observer">觀察者</param>
        public void AddEnemyObserver(IExposureObserver observer)
        {
            EnemyList.Add(observer);
        }

        /// <summary>
        /// 刪除敵人觀察者
        /// </summary>
        /// <param name="observer">觀察者</param>
        public void RemoveEnemyObserver(IExposureObserver observer)
        {
            EnemyList.Remove(observer);
        }

        /// <summary>
        /// 玩家被發現
        /// </summary>
        public void PlayerExposure()
        {
            foreach (var v in EnemyList)
            {
                v.PlayerExposure();
            }
        }

        /// <summary>
        /// 攻擊Button註冊切換至Attack State
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
        /// 攻擊
        /// </summary>
        private void Attack()
        {
            _playerStateController.SwitchAttackState();
        }

        /// <summary>
        /// 播放攻擊音效
        /// </summary>
        public void PlayAttackSFX()
        {
            _audioSource.clip = _audioClips[0];
            _audioSource.Play();
        }

        /// <summary>
        /// 播放士兵死亡音效
        /// </summary>
        public void PlayDeathSFX()
        {
            _audioSource.clip = _audioClips[1];
            _audioSource.Play();
        }

        /// <summary>
        /// 判定攻擊(Animation Event)
        /// </summary>
        public void DetectAttack()
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
        public void DetectDeath()
        {
            bool isCheat = (GameManager.Instance.GameSceneData.IsCheat || CheatPanelController.IsCheat);

            if ((!isCheat) && (!IsDeath))
            {
                IsDeath = true;
                _characterController.enabled = false;
                _playerStateController.SwitchDeathState();
                RemoveBtnOnClick();
            }
        }
    }
}