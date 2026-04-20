namespace NeverlandEvolved.Domain.Entities
{
    // Game är en domänentitet — den representerar ett spel i systemet
    // och är mappat till tabellen "Games" i databasen via EF Core.
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Navigeringsegenskap — ett spel kan ha många recensioner (1-till-många relation).
        // EF Core använder denna för att bygga JOIN-frågor och ladda relaterad data.
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
