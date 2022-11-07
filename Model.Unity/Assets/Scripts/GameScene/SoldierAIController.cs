using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using DG.Tweening;
using Player;
using Manager;

namespace Enemy
{
    public class SoldierAIController : MonoBehaviour
    {
        [SerializeField]
        private SoldierAIStateController _soldierAIStateController;

        [SerializeField]
        private NavMeshAgent _agent;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _audioClips;

        [SerializeField]
        private Transform[] _patrolPoint;

        private int destpoints = 0;

        private bool isAttack = false;

        private bool canTrack = false;

        private bool isDeath = false;

        #region 扇形範圍相關variable
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
            _agent.speed = GameManager.Instance.GameSceneData.SoldierEnemySpeed;
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
            else if (_agent.remainingDistance < 0.2f)
            {
                PatrolState();
            }
        }

        #region Trigger偵測玩家是否在範圍內(3秒即切換至追擊狀態)
        private void OnTriggerEnter(Collider other)
        {
            ResetAlertTrigger(other);
        }

        private void OnTriggerStay(Collider other)
        {
            DetectAlertTrigger(other);
        }
        #endregion

        /// <summary>
        /// 計算警戒範圍
        /// </summary>
        private void CalAlertSector()
        {
            _distance = Vector3.Distance(transform.position, PlayerController.Instance.PlayerTransform.position);
            _positionDistance = PlayerController.Instance.PlayerTransform.position - transform.position;
            _angle = Vector3.Angle(_positionDistance, transform.forward);
        }

        /// <summary>
        /// 巡邏狀態
        /// </summary>
        private void PatrolState()
        {
            _agent.SetDestination(_patrolPoint[destpoints].position);
            destpoints = (destpoints + 1) % _patrolPoint.Length;
        }

        /// <summary>
        /// 追擊狀態
        /// </summary>
        private void TrackState()
        {
            if (!_animator.GetBool("Track"))
            {
                _agent.speed = GameManager.Instance.GameSceneData.SoldierEnemySpeed * 1.5f;
                _soldierAIStateController.SwitchTracklState();
            }

            _alertSprite.color = _trackStateColor;
            _agent.SetDestination(PlayerController.Instance.PlayerTransform.position);

            if (PlayerController.Instance.IsDeath) 
            {
                _soldierAIStateController.SwitchIdleState();
            }
            else if ((!isAttack) && (_agent.remainingDistance < 2.5f))
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
                if ((_distance <= 3f) && (!PlayerController.Instance.IsDeath))
                {
                    _agent.speed = 0f;
                    PlayerController.Instance.DetectDeath();
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
                _agent.enabled = false;
                _soldierAIStateController.SwitchDeathState();
                GetComponent<SoldierAIController>().enabled = false;
            }
        }

        /// <summary>
        /// 判斷警戒範圍Trigger(是否有玩家)
        /// </summary>
        /// <param name="other">Collider物件</param>
        private void DetectAlertTrigger(Collider other) 
        {
            if ((!_animator.GetBool("Track")) && (other.gameObject.tag == "Player"))
            {
                if (canTrack)
                {
                    TrackState();
                }
            }
        }

        /// <summary>
        /// 重製警戒範圍Trigger
        /// </summary>
        /// <param name="other">Collider物件</param>
        private void ResetAlertTrigger(Collider other)
        {
            if ((!_animator.GetBool("Track")) && (other.gameObject.tag == "Player"))
            {
                canTrack = false;
                var time = 0;
                DOTween.To(() => time, x => time = x, 1, 3f).onComplete += (() => canTrack = true);
            }
        }

        /// <summary>
        /// 恢復至追擊狀態(攻擊狀態結束)(Animation Event)
        /// </summary>
        private void RevertTrackState() 
        {
            isAttack = false;
        }

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