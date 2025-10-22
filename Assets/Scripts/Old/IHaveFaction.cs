namespace DunDungeons
{
    public interface IHaveFaction
    {
        public Faction Faction { get; }
    }

    public enum Faction
    {
        Player,
        Enemy
    }
}