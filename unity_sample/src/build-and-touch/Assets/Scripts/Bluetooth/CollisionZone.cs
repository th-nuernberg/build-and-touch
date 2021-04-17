using UnityEngine;

namespace Bluetooth
{
    [RequireComponent(typeof(Collider))]
    public class CollisionZone : MonoBehaviour
    {
        public bool isLeft;
        public FingerType finger;

        private void OnTriggerEnter(Collider other)
        {
            HandCollisionManager.Instance.NotifyCollisionEnter(other, this);
        }

        private void OnTriggerStay(Collider other)
        {
            HandCollisionManager.Instance.NotifyCollisionStay(other, this);
        }

        private void OnTriggerExit(Collider other)
        {
            HandCollisionManager.Instance.NotifyCollisionExit(other, this);
        }
    }
}
