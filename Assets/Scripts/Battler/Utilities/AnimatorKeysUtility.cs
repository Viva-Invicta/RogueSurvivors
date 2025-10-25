using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public static class AnimatorKeysUtility
    {
        private static Dictionary<AnimatorKeyIdentifier, string> AnimatorKeys = new Dictionary<AnimatorKeyIdentifier, string>
        {
            { AnimatorKeyIdentifier.Death, "Death" },
            { AnimatorKeyIdentifier.Cheer, "Cheer" },
            { AnimatorKeyIdentifier.Dash, "Dash" },
            { AnimatorKeyIdentifier.Walking, "Walking" },

            { AnimatorKeyIdentifier.Attack1, "Attack1" },
            { AnimatorKeyIdentifier.Attack2, "Attack2" },
            { AnimatorKeyIdentifier.Attack3, "Attack3" },

            { AnimatorKeyIdentifier.MoveSpeed, "Speed" },
            { AnimatorKeyIdentifier.AttackSpeed, "AttackSpeed" },

            { AnimatorKeyIdentifier.Preview, "Preview" }
        };

        private static Dictionary<AnimatorKeyIdentifier, int> AnimatorHashes = new Dictionary<AnimatorKeyIdentifier, int>();

        public static bool TryGetAnimatorHash(AnimatorKeyIdentifier identifier, out int hash)
        {
            if (AnimatorHashes.TryGetValue(identifier, out hash))
            {
                return true;
            }

            if (AnimatorKeys.TryGetValue(identifier, out var key))
            {
                hash = Animator.StringToHash(key);
                AnimatorHashes.Add(identifier, hash);

                return true;
            }

            Debug.LogError($"{nameof(AnimatorKeysUtility)} : No animation key with id {identifier}");

            return false;
        }
    }

    public enum AnimatorKeyIdentifier
    {
        Death,
        Cheer,
        Dash,
        Walking,
        Attack1,
        Attack2,
        Attack3,
        AttackSpeed,
        MoveSpeed,
        Preview
    }
}
