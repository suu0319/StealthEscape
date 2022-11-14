using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using DG.Tweening;
using Player;

namespace Enemy
{
    public class SoldierAIController : MonoBehaviour
    {
        [Header("Script")]
        [SerializeField]
        private SoldierAIController _soldierAIController;
        [SerializeField]
        private SoldierAIStateController _soldierAIStateController;
        private PlayerController _playerController;

        [Header("NavMesh")]
        [SerializeField]
        internal NavMeshAgent Agent;

        [Header("Animation")]
        [SerializeField]
        private Animator _animator;

        [Header("Audio")]
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _audioClips;

        [Header("DestPoint")]
        [SerializeField]
        internal List<Vector3> PatrolPoint = new List<Vector3>();

        private int destpoints = 0;

        private bool isAttack = false;

        private bool canTrack = false;

        private bool isDeath = false;

        #region 扇形範圍相關variable
        [Header("AlertRange")]
        [SerializeField]
        private SpriteRenderer _alertSprite;

        [SerializeField]
        private float _distance;

        [SerializeField]
        private float _alertRadius;

        [SerializeField]
        [Range(0, 360)]
        private float _alertAngle;

        private float _forwardAlertAngle;

        private float _angle;

        private Vector3 _positionDistance;

        [SerializeField]
        Color32 _patrolStateColor;

        [SerializeField]
        Color32 _trackStateColor;
        #endregion

        private void Start()
        {
            Init();
            PatrolState();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            _playerController = PlayerController.Instance;
            _forwardAlertAngle = _alertAngle / 2;
            _alertSprite.color = _patrolStateColor;
        }

        private void Update()
        {
            CalAlertSector();

            if (_animator.GetBool("Track"))
            {
                TrackState();
            }
            else if ((_distance <= _alertRadius) && (_angle <= _forwardAlertAngle))
            {
                TrackState();
            }
            else if (Agent.remainingDistance < 0.2f)
            {
                PatrolState();
            }

            if ((!_animator.GetBool("Track")) && (canTrack))
            {
                TrackState();
            }
            else if (_distance <= 8f) 
            {
                EnterAlertDistance();
            }
        }

        /// <summary>
        /// 計算警戒範圍
        /// </summary>
        private void CalAlertSector()
        {
            _distance = Vector3.Distance(transform.position, _playerController.PlayerTransform.position);
            _positionDistance = _playerController.PlayerTransform.position - transform.position;
            _angle = Vector3.Angle(_positionDistance, transform.forward);
        }

        /// <summary>
        /// 巡邏狀態
        /// </summary>
        private void PatrolState()
        {
            Agent.SetDestination(PatrolPoint[destpoints]);
            destpoints = (destpoints + 1) % PatrolPoint.Count;
        }

        /// <summary>
        /// 追擊狀態
        /// </summary>
        private void TrackState()
        {
            if (!_animator.GetBool("Track"))
            {
                Agent.speed *= 1.5f;
                _soldierAIStateController.SwitchTracklState();
            }

            _alertSprite.color = _trackStateColor;
            Agent.SetDestination(_playerController.PlayerTransform.position);

            if (PlayerController.Instance.IsDeath) 
            {
                Agent.speed = 0f;
                _soldierAIStateController.SwitchIdleState();
            }
            else if ((!isAttack) && (Agent.remainingDistance < 2.5f))
            {
                isAttack = true;
                _soldierAIStateController.SwitchAttackState();
            }
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
        /// 播放死亡音效
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
            if (!isDeath) 
            {
                if ((_distance <= 3f) && (!_playerController.IsDeath))
                {
                    Agent.speed = 0f;
                    _playerController.DetectDeath();
                    _soldierAIStateController.SwitchIdleState();
                }
            }
        }

        /// <summary>
        /// 判斷死亡
        /// </summary>
        internal void DetectDeath() 
        {
            if (!isDeath) 
            {
                isDeath = true;
                Agent.enabled = false;
                _soldierAIStateController.SwitchDeathState();
                _soldierAIController.enabled = false;
            }
        }

        /// <summary>
        /// 進入警戒範圍距離內
        /// </summary>
        private void EnterAlertDistance()
        {
            if (!_animator.GetBool("Track") && (!canTrack))
            {
                canTrack = false;
                var time = 0;
                DOTween.To(() => time, x => time = x, 1, 3f).onComplete += (() => VerifyPlayerInAlertDistance());
            }
        }

        /// <summary>
        /// 驗證警戒範圍距離內
        /// </summary>
        private void VerifyPlayerInAlertDistance()
        {
            if (_distance <= 8f) 
            {
                canTrack = true;
            }
        }

        /// <summary>
        /// 恢復至追擊狀態(攻擊狀態結束)(Animation Event)
        /// </summary>
        private void RevertTrackState() 
        {
            isAttack = false;
        }

        #region 物件池測試
        /// <summary>
        /// 回收士兵
        /// </summary>
        [ContextMenu("Recycle Soldier")]
        private void RecycleSoldier()
        {
            ObjectPool.ObjectPool.Instance.RecycleToPool("Soldier", this.gameObject);
        }
        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// 繪製扇形範圍
        /// </summary>
        private void OnDrawGizmos()
        {
            var color = Handles.color;

            Handles.color = Color.black;
            var StartLine = Quaternion.Euler(0, -_alertAngle / 2, 0) * transform.forward;
            Handles.DrawSolidArc(transform.position, transform.up, StartLine, _alertAngle, _alertRadius);
            Handles.color = color;
        }
#endif
    }
}