using UnityEngine;

namespace Enemy
{
    public class SoldierAIStateController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// 切換追擊狀態
        /// </summary>
        internal void SwitchTracklState()
        {
            _animator.SetBool("Track", true);
        }

        /// <summary>
        /// 切換攻擊狀態
        /// </summary>
        internal void SwitchAttackState()
        {
            _animator.SetTrigger("Attack");
        }

        /// <summary>
        /// 切換站立狀態(砍死玩家後)
        /// </summary>
        internal void SwitchIdleState()
        {
            _animator.SetTrigger("Idle");
        }

        /// <summary>
        /// 切換死亡狀態
        /// </summary>
        internal void SwitchDeathState()
        {
            _animator.SetTrigger("Death");
        }
    }
}