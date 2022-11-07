using UnityEngine;

public class SoldierTrackBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("敵人進入追擊狀態");

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("敵人進入攻擊狀態");

        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}