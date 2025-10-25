using System;
using UnityEngine;

namespace AutoBattler
{
    public class UnitPreviewDragService : MonoBehaviour
    {
        public event Action PreviewDragReleased;

        private UnitPrefabsService unitsPrefabsService;
      
        public UnitBehaviourController ActivePreview { get; private set; }

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
        }

        public void StartPreviewDrag(UnitType unitType)
        {
            var previewInstance = SpawnPreview(unitType);
            ActivePreview = previewInstance;

            previewInstance.SetState(UnitState.Preview);
        }

        private UnitBehaviourController SpawnPreview(UnitType unitType)
        {
            var previewPrefab = unitsPrefabsService.GetUnitPrefabByType(unitType);
            var previewInstance = Instantiate(previewPrefab);
            previewInstance.Initialize(UnitFaction.Player);

            return previewInstance;
        }
    }
}