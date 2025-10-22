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