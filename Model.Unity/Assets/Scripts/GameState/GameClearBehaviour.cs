using UnityEngine;

public class GameClearBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("遊戲過關");

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
}