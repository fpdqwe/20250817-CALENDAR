namespace BLL.Abstractions
{
    public interface IDto<T>
    {
        public T ToEntity();
    }
}
