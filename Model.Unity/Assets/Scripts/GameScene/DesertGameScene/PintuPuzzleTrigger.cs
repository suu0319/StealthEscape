using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class PintuPuzzleTrigger : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField]
        internal AudioSource AudioSource;

        [Header("GameObject")]
        [SerializeField]
        internal GameObject PintuPuzzleObj;

        [Header("Operate Button")]
        [SerializeField]
        internal Button EventBtn;

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            AddOnClickListener();
        }

        private void OnTriggerEnter(Collider other)
        {
            EnterPintuPuzzleTrigger(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ExitPintuPuzzleTrigger(other);
        }

        /// <summary>
        /// 進入拼圖謎題Tigger
        /// </summary>
        /// <param name="other">Collider物件</param>
        private void EnterPintuPuzzleTrigger(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                EventBtn.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 離開拼圖謎題Tigger
        /// </summary>
        /// <param name="other">Collider物件</param>
        private void ExitPintuPuzzleTrigger(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                EventBtn.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 註冊Event Button OnClick事件
        /// </summary>
        private void AddOnClickListener()
        {
            EventBtn.onClick.AddListener(OpenPintuPuzzle);
        }

        /// <summary>
        /// 開啟拼圖謎題
        /// </summary>
        private void OpenPintuPuzzle()
        {
            PintuPuzzleObj.SetActive(true);
        }
    }
}