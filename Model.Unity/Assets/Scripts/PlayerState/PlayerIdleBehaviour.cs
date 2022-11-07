using UnityEngine;

namespace Player 
{
    public class PlayerIdleBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("角色進入Idle State");

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("角色離開Idle State");

            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}