using UnityEngine;

namespace Player
{
    public class PlayerWalkBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("角色進入Walk State");

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("角色離開Walk State");

            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}