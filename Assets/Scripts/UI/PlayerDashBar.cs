using UnityEngine;

namespace DunDungeons
{
    public class PlayerDashBar : MonoBehaviour
    {
        [SerializeField]
        private Vector2 offset;

        [SerializeField]
        private RectTransform background;

        [SerializeField]
        private RectTransform foreground;

        private Transform target;
        private Camera mainCamera;
        private CharacterMovementController model;
        private HealthComponent hpModel;

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
            var screenPosition = (Vector2)targetScreenPosition + offset;

            transform.position = screenPosition;
            var dashPassedPercent = (float)model.PassedDashCooldownTime / model.DashCooldownDuration;
            Debug.Log((float)model.PassedDashCooldownTime + " " + model.DashCooldownDuration);
            
            foreground.sizeDelta = new Vector2(background.sizeDelta.x * (1 - dashPassedPercent), background.sizeDelta.y);
        }

        public void SetTarget(GameObject target)
        {
            this.target = target.transform;
        }

        public void SetModels(CharacterMovementController model, HealthComponent hpModel)
        {
            this.model = model;
            this.hpModel = hpModel;

            hpModel.Updated +=  HandleHPModelUpdated;
        }

        private void HandleHPModelUpdated()
        {
            if (hpModel.CurrentHP <= 0)
            {
                hpModel.Updated -= HandleHPModelUpdated;
                Destroy(gameObject);
               
                return;
            }
        }
    }
}