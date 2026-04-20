namespace NeverlandEvolved.Application.DTOs
{
    // ReviewDto är det objekt som returneras till klienten via API:et.
    // Vi exponerar aldrig entiteten (Review) direkt — det är en del av
    // Clean Architecture-principen. På så sätt styr vi exakt vilken
    // data som skickas ut, utan att läcka intern databasstruktur.
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        // Vi inkluderar ID:n för koppling, men inte hela objekt-grafen
        public int GameId { get; set; }
        public int UserId { get; set; }
    }
}
