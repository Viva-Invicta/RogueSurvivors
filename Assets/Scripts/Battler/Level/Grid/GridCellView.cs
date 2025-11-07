using UnityEngine;

namespace AutoBattler
{
    public class GridCellView : MonoBehaviour
    {
        [SerializeField]
        private GameObject availableViewRoot;

        [SerializeField]
        private GameObject unavailableViewRoot;

        [SerializeField]
        private Transform visualRoot;

        [SerializeField]
        private Transform entityPlacement;

        private GameObject containedEntity;

        public bool HasEntityInside => containedEntity;

        public void SetSize(Vector3 size)
        {
            visualRoot.localScale = size;
        }

        public void AddEntity(GameObject entity)
        {
            var entityTransform = entity.transform;
            
            entityTransform.SetParent(entityPlacement, false);
            entityTransform.localPosition = Vector3.zero;

            containedEntity = entity;

            if (entity.TryGetComponent<IEntityWithGridPosition>(out var gridEntity))
            {
                gridEntity.SaveTransformData();
            }
        }

        public void ResetEntityPosition()
        {
            if (containedEntity && containedEntity.TryGetComponent<IEntityWithGridPosition>(out var entity))
            {
                entity.Reset();
            }
        }

        public void RemoveEntity()
        {
            Destroy(containedEntity);
            containedEntity = default;
        }

        public void SetState(GridCellState state)
        {
            availableViewRoot.SetActive(state == GridCellState.Available);
            unavailableViewRoot.SetActive(state == GridCellState.Unavailable);
        }

        public bool ContainsWorldPosition(Vector3 worldPosition)
        {
            var bounds = new Bounds(transform.position, visualRoot.localScale);
            
            return bounds.Contains(worldPosition);
        }
    }

    public enum GridCellState
    {
        Available,
        Unavailable,
        Invisible
    }
}