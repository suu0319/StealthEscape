using UnityEngine;

namespace Player 
{
    public class PlayerStateController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// 切換攻擊狀態
        /// </summary>
        internal void SwitchAttackState() 
        {
            _animator.SetTrigger("Attack");
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