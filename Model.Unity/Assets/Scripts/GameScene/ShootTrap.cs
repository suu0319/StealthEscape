using System.Collections;
using UnityEngine;
using Player;

namespace Trap 
{
    public class ShootTrap : BaseTrap
    {
        [SerializeField]
        internal float Speed = 100f;

        [Header("Rigidbody")]
        [SerializeField]
        private Rigidbody _shootTrapRig;

        [Header("Position")]
        [SerializeField]
        private Vector3 originalPos;

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
            else if (other.gameObject.tag == "Map")
            {
                Recycle();
            }
        }

        /// <summary>
        /// 射擊陷阱
        /// </summary>
        private void Shoot()
        {
            Vector3 fwd = transform.TransformDirection(Vector3.up);
            _shootTrapRig.velocity = fwd * Speed;
        }

        /// <summary>
        /// 回收陷阱
        /// </summary>
        private void Recycle()
        {
            _shootTrapRig.velocity = Vector3.zero;
            _shootTrapRig.gameObject.transform.localPosition = originalPos;
            StartCoroutine(OpenCloseTrap());
        }

        /// <summary>
        /// 開關陷阱
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator OpenCloseTrap()
        {
            yield return interval;
            AudioSource.Play();
            Shoot();
        }
    }
}