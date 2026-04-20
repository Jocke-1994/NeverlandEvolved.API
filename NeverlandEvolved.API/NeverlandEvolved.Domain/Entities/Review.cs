using System.Text.Json.Serialization;

namespace NeverlandEvolved.Domain.Entities
{
    // Review representerar en recension som en användare har skrivit om ett spel.
    // Den kopplar samman Game och User i en 1-till-många relation åt båda håll.
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }       // Betyg, t.ex. 1–5
        public string Comment { get; set; } = string.Empty;

        // Främmande nyckel som pekar på vilket spel recensionen gäller
        public int GameId { get; set; }

        // [JsonIgnore] hindrar serialiseraren från att försöka inkludera hela Game-objektet
        // i JSON-svaret, vilket annars kan leda till cirkulära referenser eller onödig data.
        [JsonIgnore]
        public Game? Game { get; set; }

        // Främmande nyckel som pekar på vem som skrivit recensionen
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
