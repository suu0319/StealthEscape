using System.Collections;
using UnityEngine;

namespace Trap 
{
    public abstract class BaseTrap : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource _audioSource;

        [SerializeField]
        protected float interval = 2f;

        protected abstract void OnTriggerEnter(Collider other);

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void Init();

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