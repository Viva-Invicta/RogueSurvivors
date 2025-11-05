using System;
using UnityEngine;

namespace AutoBattler
{
    public class UnitPreviewDragService : MonoBehaviour
    {
        public event Action PreviewDragReleased;

        private UnitPrefabsService unitsPrefabsService;
      
        public UnitBehaviourController ActivePreview { get; private set; }
        private TargetSelectorFactory targetSelectorFactory;

        private void Update()
        {
            if (ActivePreview)
            {
                if (Input.GetMouseButton(0))
                {
                    var layerMask = ~(1 << LayerMask.NameToLayer(PhysicsLayersUtility.UnitPreviewLayer));
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
                    {
                        ActivePreview.transform.position = hit.point;
                    }
                    else
                    {
                        ActivePreview.transform.position = Vector3.zero;
                    }
                }
                else
                {
                    PreviewDragReleased?.Invoke();
                }
            }
        }

        public void ClearPreview()
        {
            ActivePreview = default;
        }

        public void Initialize(ServiceLocator serviceLocator)
        {
            unitsPrefabsService = serviceLocator.UnitsPrefabsService;
            targetSelectorFactory = new TargetSelectorFactory(serviceLocator.EntitiesService);
        }

        public void StartPreviewDrag(UnitType unitType)
        {
            var previewInstance = SpawnPreview(unitType);
            ActivePreview = previewInstance;

            previewInstance.StateMachine.SetState(UnitStateID.Preview);
        }

        private UnitBehaviourController SpawnPreview(UnitType unitType)
        {
            var previewPrefab = unitsPrefabsService.GetUnitPrefabByType(unitType);
            var previewInstance = Instantiate(previewPrefab);
            previewInstance.Initialize(UnitFaction.Player, targetSelectorFactory);

            return previewInstance;
        }
    }
}