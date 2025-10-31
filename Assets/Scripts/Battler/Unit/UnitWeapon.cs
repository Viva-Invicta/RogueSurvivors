using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public abstract class UnitWeapon : MonoBehaviour
    {
        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Initialize(UnitBehaviourController owner, IEnumerable<DamageType> damageType);
    }
}