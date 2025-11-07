namespace AutoBattler
{
    public interface IEntityWithGridPosition
    {
        public (int x, int y) GridPosition { get; }
        public void SetGridPosition(int x, int y);
        public void SaveTransformData();
        public void Reset();
    }
}