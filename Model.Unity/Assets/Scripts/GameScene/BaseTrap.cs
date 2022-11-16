using System.Collections;
using UnityEngine;

namespace Trap
{
    public abstract class BaseTrap : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField]
        protected AudioSource AudioSource;

        [Header("Float Value")]
        [SerializeField]
        internal float Interval;

        protected abstract void OnTriggerEnter(Collider other);

        /// <summary>
        /// 進入陷阱(開)範圍
        /// </summary>
        /// <param name="other"></param>
        protected abstract void EnterTrapTrigger(Collider other);

        /// <summary>
        /// 開關陷阱
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator OpenCloseTrap();
    }
}