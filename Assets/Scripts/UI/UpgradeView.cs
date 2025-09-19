using System;
using UnityEngine;
using UnityEngine.UI;

namespace DunDungeons
{
    public class UpgradeView : MonoBehaviour
    {
        public event Action<ModifierUpgradeConfiguration> Pressed;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Button button;

        private ModifierUpgradeConfiguration configuration;

        public void SetConfiguration(ModifierUpgradeConfiguration configuration)
        {
            this.configuration = configuration;
            image.sprite = configuration.Icon;
            button.onClick.AddListener(() => HandleButtonPressed());
        }

        public void Release()
        {
            button.onClick.RemoveAllListeners();
            Pressed = null;
        }

        private void HandleButtonPressed()
        {
            Pressed?.Invoke(configuration);
        }
    }
}