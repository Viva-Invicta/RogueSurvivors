using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class PrefabsService : MonoBehaviour
    {
        [field: SerializeField]
        [AssetsOnly]
        public CharacterBehaviourController[] CharacterPrefabs 
        { 
            get; 
            private set; 
        } 
       
        public CharacterBehaviourController GetCharacterPrefabByType(CharacterType characterType)
        {
            for (var i = 0; i < CharacterPrefabs.Length; i++)
            {
                var character = CharacterPrefabs[i];
                if (character.CharacterType == characterType)
                {
                    return character;
                }
            }

            return null;
        }
    }

    public enum CharacterType
    {
        Player,
        Skeleton,
        BigSkeleton
    }
}