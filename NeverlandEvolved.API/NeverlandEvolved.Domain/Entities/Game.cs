namespace NeverlandEvolved.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Ett spel kan ha många recensioner (1-till-många)
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}