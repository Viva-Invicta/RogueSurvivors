using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DunDungeons
{
    public class UpgradesWindow : MonoBehaviour
    {
        [SerializeField]
        [AssetsOnly]
        private UpgradeView upgradeViewPrefab;

        [SerializeField]
        private Transform upgradeViewsContainer;

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text description;

        [SerializeField]
        private Text statsChanges;

        [SerializeField]
        private Button okButton;

        private List<UpgradeView> upgradeViews = new List<UpgradeView>();

        private ModifierUpgradeConfiguration activeModifierConfiguration;

        private Queue<UpgradesConfiguration> waitingConfigurations = new Queue<UpgradesConfiguration>();
        private UpgradesConfiguration activeUpgradesConfiguration;

        private ServiceLocator serviceLocator;
        private ModifiersService modifiersService;

        private void OnEnable()
        {
            serviceLocator = FindObjectOfType<ServiceLocator>();
            modifiersService = serviceLocator.ModifiersService;
        }

        public void Show(UpgradesConfiguration upgradesConfiguration)
        {
            if (activeUpgradesConfiguration == default)
            {
                activeUpgradesConfiguration = upgradesConfiguration;
            }
            else
            {
                waitingConfigurations.Enqueue(upgradesConfiguration);
                return;
            }

            Time.timeScale = 0;
            transform.SetAsLastSibling();

            foreach (var upgradeConfiguration in upgradesConfiguration.ModifierUpgrades)
            {
                var upgradeView = Instantiate(upgradeViewPrefab);
                upgradeView.SetConfiguration(upgradeConfiguration);

                upgradeView.Pressed += HandleUpgradeViewSelected;

                upgradeView.transform.SetParent(upgradeViewsContainer.transform);
                upgradeViews.Add(upgradeView);
            }
            HandleUpgradeViewSelected(upgradesConfiguration.ModifierUpgrades.First());
            okButton.onClick.AddListener(HandleOKButtonPressed);
        }

        public void Hide()
        {
            Time.timeScale = 1;
            okButton.onClick.RemoveAllListeners();
            foreach (var upgradeView in upgradeViews)
            {
                upgradeView.Release();
                Destroy(upgradeView.gameObject);
            }
            upgradeViews.Clear();
            activeUpgradesConfiguration = default;

            if (waitingConfigurations.Any())
            {
                Show(waitingConfigurations.Dequeue());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void HandleOKButtonPressed()
        {
            var modifier = serviceLocator.ModifiersService.GetModifier(Faction.Player, activeModifierConfiguration.ModifierType);

            if (activeModifierConfiguration.UpgradeType == ModifierUpgradeType.Multiply)
            {
                modifier.Value = modifier.Value * activeModifierConfiguration.Value;
            }
            else if (activeModifierConfiguration.UpgradeType == ModifierUpgradeType.Sum)
            {
                modifier.Value = modifier.Value + activeModifierConfiguration.Value;
            }

            Hide();
        }

        private void HandleUpgradeViewSelected(ModifierUpgradeConfiguration configuration)
        {
            activeModifierConfiguration = configuration;

            title.text = activeModifierConfiguration.Title;
            description.text = activeModifierConfiguration.Description;

            var modifier = modifiersService.GetModifier(Faction.Player, configuration.ModifierType);
            float statChangeValue;

            if (configuration.UpgradeType == ModifierUpgradeType.Sum)
            {
                statChangeValue = modifier.Value + configuration.Value;
            }
            else
            {
                statChangeValue = modifier.Value * configuration.Value;
            }

            statsChanges.text = $"{Math.Round(modifier.Value, 2)} -> {Math.Round(statChangeValue, 2)}";
        }
    }
}