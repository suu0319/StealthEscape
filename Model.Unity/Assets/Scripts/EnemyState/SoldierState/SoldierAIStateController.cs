using UnityEngine;
using ModelState;

namespace Enemy
{
    public class SoldierAIStateController : ModelStateController
    {
        /// <summary>
        /// 切換追擊狀態
        /// </summary>
        internal void SwitchTracklState()
        {
            Animator.SetBool("Track", true);
        }

        /// <summary>
        /// 切換站立狀態(砍死玩家後)
        /// </summary>
        internal void SwitchIdleState()
        {
            Animator.SetTrigger("Idle");
        }
    }
}