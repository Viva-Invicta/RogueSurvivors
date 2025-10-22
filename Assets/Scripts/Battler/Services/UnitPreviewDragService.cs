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

            previewInstance.IsPreview = true;
        }

        private UnitBehaviourController SpawnPreview(UnitType unitType)
        {
            var previewPrefab = unitsPrefabsService.GetUnitPrefabByType(unitType);
            var previewInstance = Instantiate(previewPrefab);
            foreach (var renderer in previewInstance.GetComponentsInChildren<Renderer>())
            {
                var previewMaterial = new Material(renderer.material);
                previewMaterial.color = new Color(previewMaterial.color.r, previewMaterial.color.g, previewMaterial.color.b, previewMaterial.color.a);
            }

            return previewInstance;
        }
    }
}