using System.Collections;
using UnityEngine;
using Player;
using Pool;

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

        private bool isShoot = false;
        private bool isRecycle = false;

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
            else if ((other.gameObject.tag == "Map") && (!isRecycle))
            {
                isRecycle = true;
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

            isShoot = false;
            isRecycle = false;

            if (!isShoot)
            {
                isShoot = true;
                AudioSource.Play();
                Shoot();
            }
        }

        #region 物件池測試
        /// <summary>
        /// 回收測試
        /// </summary>
        [ContextMenu("RecycleTest")]
        private void RecycleTest()
        {
            ObjectPool.Instance.RecycleToPool("ShootTrap", gameObject.transform.parent.gameObject);
        }
        #endregion
    }
}