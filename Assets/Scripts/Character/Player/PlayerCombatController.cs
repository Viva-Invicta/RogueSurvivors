using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class PlayerCombatController : CharacterCombatController
    {
        private InputService inputService;

        private bool isInitialized;

        public void Initialize(InputService inputService)
        {
            this.inputService = inputService;
            isInitialized = true;
        }

        private void Update()
        {
            if (inputService.AttackPressed)
            {
                HandleAttackPressed();
            }
        }

        private void HandleAttackPressed()
        {
            if (!isInitialized)
                return;

            Attack();
        }
    }
}
