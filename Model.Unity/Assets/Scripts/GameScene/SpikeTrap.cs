using System.Collections;
using UnityEngine;
using Player;

namespace Trap
{
    public class SpikeTrap : BaseTrap
    {
        [Header("Animation")]
        [SerializeField]
        private Animator _animator;

        private WaitForSeconds interval;

        private void Start()
        {
            interval = new WaitForSeconds(Interval);
            StartCoroutine(OpenCloseTrap());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            EnterTrapTrigger(other);
        }

        /// <summary>
        /// 進入陷阱(開)範圍
        /// </summary>
        /// <param name="other"></param>
        protected override void EnterTrapTrigger(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerController.Instance.DetectDeath();
            }
        }

        /// <summary>
        /// 開關陷阱
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator OpenCloseTrap()
        {
            AudioSource.Play();
            _animator.SetTrigger("Open");
            yield return interval;
            _animator.SetTrigger("Close");
            yield return interval;

            StartCoroutine(OpenCloseTrap());
        }
    }
}