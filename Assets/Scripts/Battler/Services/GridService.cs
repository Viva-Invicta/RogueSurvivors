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
            if (xPosition >= activeRoomGrid.SizeX)
            {
                Debug.LogError($" {nameof(GridService)} : Can't place entity on grid because grid's Size X too small.");
                return false ;
            }

            if (yPosition >= activeRoomGrid.SizeY)
            {
                Debug.LogError($" {nameof(GridService)} : Can't place entity on grid because grid's Size Y too small.");
                return false;
            }

            var cell = activeRoomGrid.Cells[xPosition, yPosition];
            
            if (cell.HasEntityInside)
            {
                Debug.LogError($" {nameof(GridService)} : Can't place entity on grid because cell already contains an entity.");
                return false;
            }

            cell.AddEntity(entity);

            return true;
        }

        public bool TryPlaceEntityAtPosition(GameObject entity, Vector3 position)
        {
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

        public void HighlightCells()
        {
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
            foreach (var cell in activeRoomGrid.Cells)
            {
                cell.SetState(GridCellState.Invisible);
            }
        }
    }
}