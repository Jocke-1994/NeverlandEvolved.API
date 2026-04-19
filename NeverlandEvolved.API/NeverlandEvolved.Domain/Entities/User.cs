namespace NeverlandEvolved.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // En användare kan ha många recensioner (1-till-många)
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}