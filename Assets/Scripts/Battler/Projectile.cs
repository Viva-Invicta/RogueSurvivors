using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        public event Action TargetAchieved;

        [SerializeField] private float speed = 10f;
        [SerializeField] private float maxLifetime = 5f;

        private UnitBehaviourController target;
        private float lifetime;
        private bool castStarted;

        public UnitBehaviourController Target => target;

        public void Initialize(UnitBehaviourController target)
        {
            this.target = target;
        }

        public void StartCast()
        {
            castStarted = true;
        }

        private void Update()
        {
            if (!castStarted)
            {
                return;
            }

            lifetime += Time.deltaTime;
            if (lifetime >= maxLifetime)
            {
                Destroy(gameObject);
                return;
            }

            var direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<UnitBehaviourController>(out var hitTarget))
            {
                return;
            }

            if (hitTarget != target)
            {
                return;
            }

            TargetAchieved?.Invoke();
        }

        public void Release()
        {
            TargetAchieved = null;
        }
    }
}
