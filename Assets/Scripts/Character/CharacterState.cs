using UnityEngine;

namespace DunDungeons
{
    public class CharacterState : ICharacterStateProvider, ICharacterStateSetter
    {
        public bool IsDead { get; set; }

        public bool IsMovementLocked { get; set; }
        public bool IsAttackLocked { get; set; }

        public bool IsDashInCooldown { get; set; }
        public bool IsWeaponInCooldown  { get; set; }

        public float MovementSpeed { get; set; }

        public MonoBehaviour RootComponent { get; set; }
    }

    public interface ICharacterStateProvider
    {
        public bool IsDead { get; }

        public bool IsMovementLocked { get; }
        public bool IsAttackLocked { get; }
        
        public bool IsDashInCooldown { get; }
        public bool IsWeaponInCooldown { get; }

        public float MovementSpeed { get; }

        public MonoBehaviour RootComponent { get; }
    }

    public interface ICharacterStateSetter
    {
        public bool IsDead { set; }

        public bool IsMovementLocked { set; }
        public bool IsAttackLocked { set; }

        public bool IsDashInCooldown { set; }
        public bool IsWeaponInCooldown { set; }

        public float MovementSpeed { set; }

        public MonoBehaviour RootComponent { set; }
    }
}