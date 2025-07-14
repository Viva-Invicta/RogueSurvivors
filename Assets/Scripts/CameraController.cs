using UnityEngine;

namespace DunDungeons
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;

        [SerializeField]
        private Vector3 bounds;

        [SerializeField]
        private float xLerp = 0.6f;

        [SerializeField]
        private float zLerp = 0.95f;

        private Transform target;
        private bool isInitialized;

        public void Initialize(ServiceLocator serviceLocator)
        {
            this.target = serviceLocator.EntitiesService.Player.transform;
            isInitialized = true;
        }

        public void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            var position = transform.position;

            var xPosition = position.x + offset.x;
            var zPosition = position.z + offset.z;

            var targetXPosition = target.position.x;
            var targetZPosition = target.position.z;

            if (targetXPosition + bounds.x / 2 < xPosition)
            {
                xPosition = targetXPosition + bounds.x / 2;
            }
            else if (xPosition < targetXPosition - bounds.x / 2)
            {
                xPosition = targetXPosition - bounds.x / 2;
            }
            else
            {
                xPosition = Mathf.Lerp(xPosition, targetXPosition, 1 - xLerp);
            }

            zPosition = Mathf.Lerp(zPosition, targetZPosition, 1 - zLerp);

            var newCameraPosition = new Vector3(xPosition, position.y, zPosition);
            transform.position = newCameraPosition;

        }

    }
}
