using UnityEngine;

namespace ModelState
{
    public class ModelStateController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField]
        protected Animator Animator;

        /// <summary>
        /// 切換攻擊狀態
        /// </summary>
        internal void SwitchAttackState()
        {
            Animator.SetTrigger("Attack");
        }

        /// <summary>
        /// 切換死亡狀態
        /// </summary>
        internal void SwitchDeathState()
        {
            Animator.SetTrigger("Death");
        }
    }
}