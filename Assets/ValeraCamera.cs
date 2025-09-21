using UnityEngine;

namespace DunDungeons
{
    public class ValeraCamera : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private Vector2 boundsSize;
        [SerializeField]
        private float smoothSpeed = 5f;
        [SerializeField]
        private float fastSpeed = 15f;

        [SerializeField]
        private Transform target;

        public void FixedUpdate()
        {
            var targetPosition = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
            var delta = targetPosition - transform.position;

            var insideBounds = Mathf.Abs(delta.x) < boundsSize.x * 0.5f && Mathf.Abs(delta.z) < boundsSize.y * 0.5f;
            var speed = insideBounds ? smoothSpeed : fastSpeed;

            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        }
    }
}
