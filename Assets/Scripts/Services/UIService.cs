using UnityEngine;

namespace DunDungeons
{
    public class UIService : MonoBehaviour
    {
        [SerializeField]
        private Transform canvas;

        [SerializeField]
        private EntityHealthBar healthBarPrefab;

        [SerializeField]
        private PlayerDashBar dashBarPrefab;

        public void AddHealthBar(GameObject target, HealthComponent model)
        {
            var healthBar = Instantiate(healthBarPrefab);
            healthBar.transform.SetParent(canvas);
            healthBar.SetTarget(target);
            healthBar.SetModel(model);
        }

        public PlayerDashBar AddDashBar(GameObject target, CharacterMovementController model, HealthComponent hpModel)
        {
            var dashBar = Instantiate(dashBarPrefab);
            dashBar.transform.SetParent(canvas);
            dashBar.SetTarget(target);
            dashBar.SetModels(model, hpModel);

            return dashBar;
        }

    }
}