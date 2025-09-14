using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DunDungeons
{
    public class ModifiersService : MonoBehaviour
    {
        private List<Modifier> modifiers;

        public Modifier GetModifier(Faction faction, ModifierType modifierType)
        {
            if (modifiers == null)
            {
                modifiers = new List<Modifier>();
            }

            var result = modifiers.FirstOrDefault((modifier) => modifier.Faction == faction &&
                                                                modifier.Type == modifierType);

            if (result == default)
            {
                result = new Modifier(modifierType, faction);
                modifiers.Add(result);

            }

            return result;
        }
    }
}
