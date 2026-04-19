namespace NeverlandEvolved.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } // T.ex. 1-5
        public string Comment { get; set; } = string.Empty;

        // Koppling till Game
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        // Koppling till User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}