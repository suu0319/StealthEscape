using UnityEngine;
using GameOver;

public class GameOverBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("遊戲失敗");

        GameOverPanel.Instance.AppearGameOverScreen();

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
}