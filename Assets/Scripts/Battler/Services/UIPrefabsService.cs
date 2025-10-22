using Sirenix.OdinInspector;
using UnityEngine;

namespace AutoBattler
{
    public class UIPrefabsService : MonoBehaviour
    {
        [field: SerializeField]
        [AssetsOnly]
        public UIView[] UIPrefabs
        {
            get;
            private set;
        }

        public UIView GetUIPrefabByType(UIViewType prefabType)
        {
            for (var i = 0; i < UIPrefabs.Length; i++)
            {
                var indexedView = UIPrefabs[i];
                if (indexedView.ViewType == prefabType)
                {
                    return indexedView;
                }
            }

            Debug.LogError($"{nameof(UIPrefabsService)} could not find prefab with type {prefabType}");
            return default;
        }
    }

    public enum UIViewType
    {
        UnitShop,
        UnitShopEntry
    }
}