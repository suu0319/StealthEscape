using UnityEngine;
using Manager;

public class PlayerDeathBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("角色進入Attack State");

        GameStateController.Instance.SwitchGameOverState();

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("角色離開Attack State");

        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
