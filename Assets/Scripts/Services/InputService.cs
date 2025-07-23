using System;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.XR;
using Input = UnityEngine.Input;

namespace DunDungeons
{
    public class InputService : MonoBehaviour
    {
        [SerializeField] private KeyCode upKey;
        [SerializeField] private KeyCode downKey;
        [SerializeField] private KeyCode leftKey;
        [SerializeField] private KeyCode rightKey;
        [SerializeField] private KeyCode attackKey;
        [SerializeField] private KeyCode specialKey1;

        public bool UpPressed => Input.GetKey(upKey);
        public bool DownPressed => Input.GetKey(downKey);
        public bool LeftPressed => Input.GetKey(leftKey);
        public bool RightPressed => Input.GetKey(rightKey);
        public bool AttackPressed => Input.GetKey(attackKey);
        public bool Special1Pressed => Input.GetKey(specialKey1);

        public Vector3 InputDirection { get; private set; }

        private void Update()
        {
            CalculateInputDirection();
        }

        private void CalculateInputDirection()
        {
            var xInput = 0;
            var zInput = 0;

            if (UpPressed)
            {
                zInput += 1;
            }

            if (DownPressed)
            {
                zInput -= 1;
            }

            if (LeftPressed)
            {
                xInput -= 1;
            }

            if (RightPressed)
            {
                xInput += 1;
            }

            InputDirection = new Vector3(xInput, 0, zInput).normalized;
        }
    }

   
}