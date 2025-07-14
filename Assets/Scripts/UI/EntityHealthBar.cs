using UnityEngine;

namespace DunDungeons
{
    public class EntityHealthBar : MonoBehaviour
    {
        [SerializeField]
        private Vector2 offset;

        [SerializeField]
        private RectTransform background;

        [SerializeField]
        private RectTransform foreground;

        private Transform target;
        private Camera mainCamera;
        private HealthComponent model;

        private void OnEnable()
        {
            mainCamera = Camera.main;
        }

        public void Update()
        {
            if (!target)
            {
                return;
            }

            var targetScreenPosition = mainCamera.WorldToScreenPoint(target.position);
            var screenPosition = (Vector2) targetScreenPosition + offset;

            transform.position = screenPosition;
        }

        public void SetTarget(GameObject target)
        {
            this.target = target.transform;
        }

        public void SetModel(HealthComponent model)
        {
            this.model = model;

            model.Updated += HandleModelUpdated;
        }

        private void HandleModelUpdated()
        {
            if (model.CurrentHP <= 0)
            {
                Destroy(gameObject);
                model.Updated -= HandleModelUpdated;

                return;
            }
            var healthPercent = (float) model.CurrentHP / model.MaxHP;
            foreground.sizeDelta = new Vector2(background.sizeDelta.x * healthPercent, background.sizeDelta.y);
        }
    }
}