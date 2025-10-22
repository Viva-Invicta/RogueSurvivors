using UnityEngine;

namespace AutoBattler
{
    public class ServiceLocator : MonoBehaviour
    {
        [field: SerializeField] public UnitPrefabsService UnitsPrefabsService { get; private set; }
        [field: SerializeField] public UIPrefabsService UIPrefabsService { get; private set; }
        [field: SerializeField] public GridService GridService { get; private set; }
        [field: SerializeField] public UIService UIService { get; private set; }
        [field: SerializeField] public UnitPreviewDragService UnitPreviewDragService { get; private set;}
        [field: SerializeField] public RoomsService RoomsService { get; private set; }
        [field: SerializeField] public UnitShopService UnitShopService { get; private set; }

        public void InitializeServices()
        {
            UIService.Initialize(this);
            UnitShopService.Initialize(this);
            UnitPreviewDragService.Initialize(this);
        }
    }
}