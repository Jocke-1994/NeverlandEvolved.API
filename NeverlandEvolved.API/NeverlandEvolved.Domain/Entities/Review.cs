using System.Text.Json.Serialization; // Viktigt för att [JsonIgnore] ska fungera

namespace NeverlandEvolved.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        // Koppling till Game
        public int GameId { get; set; }

        [JsonIgnore] // Säger till API:et att inte kräva hela Game-objektet i JSON-koden
        public Game? Game { get; set; }

        // Koppling till User
        public int UserId { get; set; }

        [JsonIgnore] // Säger till API:et att inte kräva hela User-objektet i JSON-koden
        public User? User { get; set; }
    }
}