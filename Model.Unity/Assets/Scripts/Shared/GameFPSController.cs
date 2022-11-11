using UnityEngine;

namespace Manager 
{
    public class GameFPSController : MonoBehaviour
    {
        internal static int Fps;

        private void Awake()
        {
            SetFPSLimit();
        }

        // Update is called once per frame
        private void Update()
        {
            CalFPS();
        }

        /// <summary>
        /// 設定FPS限幀數
        /// </summary>
        private void SetFPSLimit() 
        {
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// 計算FPS
        /// </summary>
        private void CalFPS()
        {
            if (Time.timeScale == 1f) 
            {
                Fps = (int)(1f / Time.smoothDeltaTime);
            }
        }
    }
}