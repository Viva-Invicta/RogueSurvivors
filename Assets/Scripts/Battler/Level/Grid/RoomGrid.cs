using UnityEngine;

namespace AutoBattler
{
    public class RoomGrid : MonoBehaviour
    {
        [SerializeField] private int worldWidth = 10;
        [SerializeField] private int worldHeight = 10;
        [SerializeField] private GridCellView cellPrefab;
        [SerializeField] private GridConfiguration gridData;

        public int SizeX => Cells.GetLength(0);
        public int SizeY => Cells.GetLength(1);

        private GridCellView[,] cells;

        public GridCellView[,] Cells
        {
            get
            {
                if (cells == null || cells.Length == 0)
                {
                    Debug.LogError($"{nameof(RoomGrid)} : Can't return any cell views because none of them drawn yet!");
                }
                return cells;
            }
        }

        public void DrawGrid()
        {
            if (!IsValidData())
            {
                Debug.LogError($"{nameof(RoomGrid)} : Can't draw any cells because configuration is invalid!");

                return;
            }

            var (gridWidth, gridHeight) = GetGridSize();
            var (cellWorldWidth, cellWorldHeight) = GetCellWorldSize(gridWidth, gridHeight);
            var origin = GetOrigin();

            var grid = new GridCellView[gridWidth, gridHeight];

            for (var x = 0; x < gridWidth; x++)
            {
                for (var y = 0; y < gridHeight; y++)
                {
                    var cellCenter = GetCellCenter(origin, x, y, cellWorldWidth, cellWorldHeight);
                    var cell = Instantiate(cellPrefab, cellCenter, Quaternion.identity, transform);
                    cell.SetSize(new Vector3(cellWorldWidth, 1f, cellWorldHeight));
                    cell.name = $"Cell_{x}_{y}";
                    grid[x, y] = cell;
                }
            }

            cells = grid;
        }

        private void OnDrawGizmos()
        {
            if (!IsValidData())
            {
                return;
            }

            var (gridWidth, gridHeight) = GetGridSize();
            var (cellWorldWidth, cellWorldHeight) = GetCellWorldSize(gridWidth, gridHeight);
            var origin = GetOrigin();

            Gizmos.color = Color.green;

            for (var x = 0; x < gridWidth; x++)
            {
                for (var y = 0; y < gridHeight; y++)
                {
                    var cellCenter = GetCellCenter(origin, x, y, cellWorldWidth, cellWorldHeight);
                    var cellSize = new Vector3(cellWorldWidth, 0.1f, cellWorldHeight);
                    Gizmos.DrawWireCube(cellCenter, cellSize);
                }
            }
        }

        private bool IsValidData()
        {
            return gridData != null && gridData.Width > 0 && gridData.Height > 0;
        }

        private (int width, int height) GetGridSize()
        {
            return (gridData.Width, gridData.Height);
        }

        private (float cellWidth, float cellHeight) GetCellWorldSize(int gridWidth, int gridHeight)
        {
            var cellWidth = (float)worldWidth / gridWidth;
            var cellHeight = (float)worldHeight / gridHeight;
            return (cellWidth, cellHeight);
        }

        private Vector3 GetOrigin()
        {
            return transform.position - new Vector3(worldWidth / 2f, 0f, worldHeight / 2f);
        }

        private static Vector3 GetCellCenter(Vector3 origin, int x, int y, float cellWidth, float cellHeight)
        {
            return origin + new Vector3((x + 0.5f) * cellWidth, 0f, (y + 0.5f) * cellHeight);
        }
    }
}
