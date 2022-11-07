using UnityEngine;
using UnityEngine.UI;
using Manager;

public class GameFPSText : MonoBehaviour
{
    [SerializeField]
    private Text _fpsText;

    private void Update()
    {
        _fpsText.text = "FPS:" + GameFPSController.Fps.ToString();
    }
}