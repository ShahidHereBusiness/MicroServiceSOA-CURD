using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroServicesSOA.SOA;

namespace MicroServicesSOA.DbContext
{
    public class AppDbContext: IdentityDbContext<
            AppUser, AppRole, string,
            ApplUserClaim, AppUserRole, AppUserLogin,
            AppRoleClaim, AppUserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}