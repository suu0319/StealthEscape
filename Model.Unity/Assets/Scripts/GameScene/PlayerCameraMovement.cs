using UnityEngine;
using Cinemachine;

namespace Player 
{
    public class PlayerCameraMovement : MonoBehaviour
    {
        [SerializeField]
        private float touchSensitivity_x = 30f;
        [SerializeField]
        private float touchSensitivity_y = 30f;

        private void OnEnable()
        {
            Debug.Log("可以操作Camera");

            CinemachineCore.GetInputAxis = HandleAxisInput;
        }

        private void OnDisable()
        {
            Debug.Log("操作角色，無法操作Camera");

            CinemachineCore.GetInputAxis = HandAxisZero;
        }

        /// <summary>
        /// 取得觸碰輸入值
        /// </summary>
        /// <param name="axisName">水平、垂直座標</param>
        /// <returns></returns>
        private float HandleAxisInput(string axisName)
        {
            switch (axisName)
            {
                case "Mouse X":
                    if (Input.touchCount > 0)
                    {
                        return Input.touches[0].deltaPosition.x / touchSensitivity_x;
                    }
                    else
                    {
                        return Input.GetAxis(axisName);
                    }

                case "Mouse Y":
                    if (Input.touchCount > 0)
                    {
                        return Input.touches[0].deltaPosition.y / touchSensitivity_y;
                    }
                    else
                    {
                        return Input.GetAxis(axisName);
                    }

                default:
                    Debug.LogError("無法識別滑動轉向Camera");
                    break;
            }

            return 0f;
        }

        /// <summary>
        /// 無觸碰輸入值
        /// </summary>
        /// <param name="axisName">None</param>
        /// <returns></returns>
        private float HandAxisZero(string axisName)
        {
            return 0f;
        }
    }
}