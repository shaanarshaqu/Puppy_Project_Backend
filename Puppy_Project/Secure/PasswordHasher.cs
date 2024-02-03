namespace Puppy_Project.Secure
{
    public class PasswordSecure
    {
        public static string HashPassword(string password)
        {
            // Generate a salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password with the generated salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        // Verify a password against a hashed password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Use BCrypt to verify the password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
