using Microsoft.AspNetCore.Identity;

namespace WebApp4.Entities.Identities
{
    public class AppRole:IdentityRole<string>
    {
        public int MyProperty { get; set; }
    }
}
