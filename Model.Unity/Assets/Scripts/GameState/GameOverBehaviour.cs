using UnityEngine;

public class GameOverBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("遊戲失敗");

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
}