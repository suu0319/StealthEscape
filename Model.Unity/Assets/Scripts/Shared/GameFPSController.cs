using UnityEngine;

namespace Manager 
{
    public class GameFPSController : MonoBehaviour
    {
        private float showTime = 1f;
        private float deltaTime = 0f;
        private int count = 0;

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
                count++;
                deltaTime += Time.smoothDeltaTime;

                if (deltaTime >= showTime) 
                {
                    Fps = (int)(count / deltaTime);

                    count = 0;
                    deltaTime = 0f;
                }              
            }
        }
    }
}