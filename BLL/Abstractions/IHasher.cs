namespace BLL.Abstractions
{
    public interface IHasher
    {
        public string Hash(string value);
        public bool Verify(string value, string password);
    }
}
