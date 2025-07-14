using UnityEngine;

namespace DunDungeons
{
    public class UIService : MonoBehaviour
    {
        [SerializeField]
        private Transform canvas;

        [SerializeField]
        private EntityHealthBar healthBarPrefab;

        public void AddHealthBar(GameObject target, HealthComponent model)
        {
            var healthBar = Instantiate(healthBarPrefab);
            healthBar.transform.SetParent(canvas);
            healthBar.SetTarget(target);
            healthBar.SetModel(model);
        }

    }
}