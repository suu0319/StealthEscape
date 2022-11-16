using UnityEngine;
using Enemy;

namespace Player
{
    public class DetectPlayerAttack : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            DetectAttack(other);
        }

        /// <summary>
        /// 判斷玩家攻擊
        /// </summary>
        /// <param name="other">Collider物件</param>
        private void DetectAttack(Collider other)
        {
            if (other.gameObject.tag == "DetectHurt")
            {
                other.GetComponentInParent<SoldierAIController>().DetectDeath();
            }
        }
    }
}