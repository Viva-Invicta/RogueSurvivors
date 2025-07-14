using System;
using UnityEngine;

namespace DunDungeons
{
    public class InputService : MonoBehaviour
    {
        [SerializeField] private KeyCode upKey;
        [SerializeField] private KeyCode downKey;
        [SerializeField] private KeyCode leftKey;
        [SerializeField] private KeyCode rightKey;
        [SerializeField] private KeyCode attackKey;

        public bool UpPressed => Input.GetKey(upKey);
        public bool DownPressed => Input.GetKey(downKey);
        public bool LeftPressed => Input.GetKey(leftKey);
        public bool RightPressed => Input.GetKey(rightKey);
        public bool AttackPressed => Input.GetKey(attackKey);
    }

   
}