namespace Dzaba.HomeAccounting.DataBase.Contracts.Model
{
    public interface IEntity<T>
        where T : struct
    {
        T Id { get; set; }
    }
}
