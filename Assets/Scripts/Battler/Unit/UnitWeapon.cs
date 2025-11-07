using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public abstract class UnitWeapon : MonoBehaviour
    {
        protected UnitBehaviourController ActiveTarget;

        public virtual void Activate() { }
        public virtual void Deactivate() { }
        public virtual void SetTarget(UnitBehaviourController target) => ActiveTarget = target;
        public abstract void Initialize(UnitBehaviourController owner, IEnumerable<DamageType> damageType);
    }
}