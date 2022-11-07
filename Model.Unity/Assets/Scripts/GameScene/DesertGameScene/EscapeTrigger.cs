using UnityEngine;
using GameLoading;
using Manager;

public class EscapeTrigger : MonoBehaviour
{
    [SerializeField]
    private string escapeSceneName;

    private void OnTriggerEnter(Collider other)
    {
        EnterEscapeTrigger(other);
    }

    private void EnterEscapeTrigger(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            GameLoadingPanel.Instance.gameObject.SetActive(true);
            GameLoadingAsync.Instance.LoadGame(escapeSceneName);
            GameStateController.Instance.SwitchGameClearState();
        }
    }
}