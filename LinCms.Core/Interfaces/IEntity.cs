namespace LinCms.Core.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}