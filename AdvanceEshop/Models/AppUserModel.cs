using Microsoft.AspNetCore.Identity;

namespace AdvanceEshop.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Occupation { get; set; }

    }
}
