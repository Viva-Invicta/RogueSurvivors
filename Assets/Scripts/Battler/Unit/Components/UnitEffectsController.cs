using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AutoBattler
{
    public class UnitEffectsController : MonoBehaviour, IInitializableWithUnitStatusComponent
    {
        private Dictionary<UnitEffectType, UnitEffect> effectMap = new Dictionary<UnitEffectType, UnitEffect>();

        public void Initialize(IUnitStatusProvider unitStatusProvider)
        {
            var effectsList = unitStatusProvider.Configuration.Effects;

            if (effectsList == default || !effectsList.Any())
            {
                return;
            }
            
            foreach (var effect in effectsList)
            {
                if (!effect.Prefab)
                {
                    Debug.LogError($"{nameof(UnitEffectsController)} : No effect prefab in effect " +
                        $"configuration for unit {unitStatusProvider.Configuration.name}");
                    continue;
                }
                var effectInstance = Instantiate(effect.Prefab, transform, false);
                effectMap.Add(effect.Type, effectInstance);
            }
        }

        public void PlayEffect(UnitEffectType type, float delay = 0f)
        {
            if (effectMap.TryGetValue(type, out var effect))
            {
                effect.Play(delay);
            }
        }
    }
}
