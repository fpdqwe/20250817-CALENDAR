namespace Calendar.Services.Abstractions
{
    public interface IDto<T>
    {
        public T ToEntity();
    }
}
