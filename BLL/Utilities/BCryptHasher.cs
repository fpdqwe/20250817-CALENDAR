using BLL.Abstractions;

namespace BLL.Utilities
{
    public class BCryptHasher : IHasher
    {
        public string Hash(string value)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(value);
        }

        public bool Verify(string value, string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(value, password);
        }
    }
}
