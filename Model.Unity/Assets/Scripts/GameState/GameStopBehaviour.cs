using UnityEngine;

public class GameStopBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("開啟暫停選單");

        Time.timeScale = 0f;

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("退出暫停選單");

        Time.timeScale = 1f;

        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}