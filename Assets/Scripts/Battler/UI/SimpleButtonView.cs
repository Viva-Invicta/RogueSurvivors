using System;
using UnityEngine;
using UnityEngine.UI;

namespace AutoBattler
{
    public class SimpleButtonView : UIView
    {
        public event Action Pressed;

        [SerializeField] private Button button;
        [SerializeField] private UIViewType viewType = UIViewType.Undefined;

        public override UIViewType ViewType => viewType;

        public override void Initialize(ServiceLocator serviceLocator)
        {
            base.Initialize(serviceLocator);

            button.onClick.AddListener(() => HandleButtonPressed());
        }

        public override void Release()
        {
            base.Release();

            button.onClick.RemoveAllListeners();
            Pressed = null;
        }

        private void HandleButtonPressed()
        {
            Pressed?.Invoke();
        }
    }
}