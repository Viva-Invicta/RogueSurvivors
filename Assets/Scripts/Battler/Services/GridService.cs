using UnityEngine;

namespace AutoBattler
{
    public class GridService : MonoBehaviour
    {
        private RoomGrid activeRoomGrid;

        public void SetActiveRoomGrid(RoomGrid grid)
        {
            activeRoomGrid = grid;
            activeRoomGrid.DrawGrid();
            DehighlightCells();
        }

        public bool TryPlaceEntityAtGridPosition(GameObject entity, int xPosition, int yPosition)
        {
            if (!TryGetValidCell(xPosition, yPosition, out var cell))
            {
                return false;
            }

            if (cell.HasEntityInside)
            {
                Debug.LogError($"{nameof(GridService)}: Can't place entity on grid because cell already contains an entity.");
                return false;
            }

            cell.AddEntity(entity);
            if (entity.TryGetComponent<IEntityWithGridPosition>(out var entityWithGridPosition))
            {
                entityWithGridPosition.SetGridPosition(xPosition, yPosition);
            }

            return true;
        }

        public bool TryPlaceEntityAtPosition(GameObject entity, Vector3 position)
        {
            if (!IsActiveGridAvailable())
            {
                return false;
            }

            foreach (var cell in activeRoomGrid.Cells)
            {
                if (cell.ContainsWorldPosition(position))
                {
                    if (!cell.HasEntityInside)
                    {
                        cell.AddEntity(entity);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public GridCellView GetCellByPosition(int xPosition, int yPosition)
        {
            if (!TryGetValidCell(xPosition, yPosition, out var cell))
            {
                return default;
            }

            return cell;
        }

        public void ResetActiveGridEntities()
        {
            if (!IsActiveGridAvailable())
            {
                return;
            }

            foreach (var cell in activeRoomGrid.Cells)
            {
                cell.ResetEntityPosition();
            }
        }

        public void HighlightCells()
        {
            if (!IsActiveGridAvailable())
            {
                return;
            }

            foreach (var cell in activeRoomGrid.Cells)
            {
                if (cell.HasEntityInside)
                {
                    cell.SetState(GridCellState.Unavailable);
                }
                else
                {
                    cell.SetState(GridCellState.Available);
                }
            }
        }

        public void DehighlightCells()
        {
            if (!IsActiveGridAvailable())
            {
                return;
            }

            foreach (var cell in activeRoomGrid.Cells)
            {
                cell.SetState(GridCellState.Invisible);
            }
        }

        private bool IsActiveGridAvailable()
        {
            if (activeRoomGrid == default)
            {
                Debug.LogError($"{nameof(GridService)}: No active room grid assigned.");
                return false;
            }
            return true;
        }

        private bool ValidateGridPosition(int xPosition, int yPosition)
        {
            if (xPosition < 0 || xPosition >= activeRoomGrid.SizeX)
            {
                Debug.LogError($"{nameof(GridService)}: X position {xPosition} is out of grid bounds [0, {activeRoomGrid.SizeX - 1}].");
                return false;
            }

            if (yPosition < 0 || yPosition >= activeRoomGrid.SizeY)
            {
                Debug.LogError($"{nameof(GridService)}: Y position {yPosition} is out of grid bounds [0, {activeRoomGrid.SizeY - 1}].");
                return false;
            }

            return true;
        }

        private bool TryGetValidCell(int xPosition, int yPosition, out GridCellView cell)
        {
            cell = default;

            if (!IsActiveGridAvailable())
            {
                return false;
            }

            if (!ValidateGridPosition(xPosition, yPosition))
            {
                return false;
            }

            cell = activeRoomGrid.Cells[xPosition, yPosition];
            return true;
        }
    }
}