using UnityEngine;

namespace DunDungeons
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;

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

            var targetXPosition = target.position.x;
            var targetZPosition = target.position.z;

            var xPosition = targetXPosition + offset.x;
            var zPosition = targetZPosition + offset.z;


            var newCameraPosition = new Vector3(xPosition, position.y, zPosition);
            transform.position = newCameraPosition;

        }

    }
}
