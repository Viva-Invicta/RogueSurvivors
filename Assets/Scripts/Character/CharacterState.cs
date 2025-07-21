using UnityEngine;

namespace DunDungeons
{
    public class CharacterState : ICharacterStateProvider, ICharacterStateSetter
    {
        public bool IsDead { get; set; }

        public bool IsAttacking { get; set; }
        public bool IsWalking { get; set; }

        public bool IsMovementLocked { get; set; }

        public bool IsWeaponInCooldown  { get; set; }
        public MonoBehaviour RootComponent { get; set; }
    }

    public interface ICharacterStateProvider
    {
        public bool IsDead { get; }
        public bool IsAttacking { get; }
        public bool IsMovementLocked { get; }
        public bool IsWeaponInCooldown { get; }
        public bool IsWalking { get; }
        public MonoBehaviour RootComponent { get; }
    }

    public interface ICharacterStateSetter
    {
        public bool IsDead { set; }
        public bool IsAttacking { set; }
        public bool IsMovementLocked { set; }
        public bool IsWeaponInCooldown { set; }
        public bool IsWalking { get; }
        public MonoBehaviour RootComponent { set; }
    }
}