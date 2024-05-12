using Microsoft.AspNetCore.Identity;

namespace WebApp4.Entities.Identities
{
    public class AppUser : IdentityUser<string>
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndTime { get; set; }
    }
}
