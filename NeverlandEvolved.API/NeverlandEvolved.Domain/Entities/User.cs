namespace NeverlandEvolved.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Standardroll

        // En användare kan ha många recensioner (1-till-många)
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}