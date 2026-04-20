namespace NeverlandEvolved.Domain.Entities
{
    // User representerar en registrerad användare i systemet.
    // OBS: Lösenordet sparas här i klartext — i produktion bör det
    // alltid hashas med t.ex. BCrypt innan det sparas i databasen.
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Rollen styr vad användaren får göra i systemet (RBAC).
        // "User" är standardrollen. "Admin" ger tillgång till skyddade endpoints.
        public string Role { get; set; } = "User";

        // Navigeringsegenskap — en användare kan ha skrivit många recensioner.
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
