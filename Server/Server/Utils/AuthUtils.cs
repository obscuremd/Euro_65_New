namespace Server.Utils
{
    public class AuthUtils
    {
        
        public static string GenerateLoginId()
        {
            return $"sec-{Guid.NewGuid().ToString().Substring(0, 8)}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        public static string GeneratePassword()
        {
            return Path.GetRandomFileName().Replace(".", "").Substring(0, 8);
        }
    }
}