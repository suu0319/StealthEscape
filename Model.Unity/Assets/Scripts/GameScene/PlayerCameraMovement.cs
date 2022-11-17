using UnityEngine;
using Cinemachine;

namespace Player
{
    public class PlayerCameraMovement : MonoBehaviour
    {
        [Header("Touch Sensitivity")]
        [SerializeField]
        private float touchSensitivity_x = 30f;
        [SerializeField]
        private float touchSensitivity_y = 30f;

        [Header("Other")]
        [SerializeField]
        private bool isPlayerMove = false;

        [SerializeField]
        private RectTransform _joystickHandle;

        private void Start()
        {
            CinemachineCore.GetInputAxis = HandleAxisInput;
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
                    if (Input.touchCount >= 2)
                    {
                        if (isPlayerMove == true)
                        {
                            return Input.touches[Input.touches.Length - 1].deltaPosition.x / touchSensitivity_x;
                        }
                        else
                        {
                            return Input.touches[0].deltaPosition.x / touchSensitivity_x;
                        }
                    }
                    else if (Input.touchCount == 1)
                    {
                        if (_joystickHandle.anchoredPosition != Vector2.zero)
                        {
                            isPlayerMove = true;
                        }

                        return Input.touches[0].deltaPosition.x / touchSensitivity_x;
                    }
                    else
                    {
                        if (!isPlayerMove)
                        {
                            isPlayerMove = false;
                        }
                        
                        return Input.GetAxis(axisName);
                    }

                case "Mouse Y":
                    if (Input.touchCount >= 2)
                    {
                        if (isPlayerMove == true)
                        {
                            return Input.touches[Input.touches.Length - 1].deltaPosition.y / touchSensitivity_y;
                        }
                        else
                        {
                            return Input.touches[0].deltaPosition.y / touchSensitivity_y;
                        }
                    }
                    else if (Input.touchCount == 1)
                    {
                        if (_joystickHandle.anchoredPosition != Vector2.zero)
                        {
                            isPlayerMove = true;
                        }

                        return Input.touches[0].deltaPosition.y / touchSensitivity_y;
                    }
                    else
                    {
                        if (!isPlayerMove)
                        {
                            isPlayerMove = false;
                        }
                        return Input.GetAxis(axisName);
                    }

                default:
                    Debug.LogError("無法識別滑動轉向Camera");
                    break;
            }

            return 0f;
        }
    }
}