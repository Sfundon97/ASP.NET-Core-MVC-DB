// Programmer name : S Nondwatyu
// Student nr : 220036624
// Assignment nr : GA1
// Purpose : The purpose of this LoginDBContext class is to serve as the database context
// for managing user authentication and authorization using ASP.NET Core Identity.
// This context is used to interact with the underlying database for storing user-related data,
// such as user credentials, roles, and associated claims.
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore_DB.Data
{
    public class LoginDBContext: IdentityDbContext
    {
        public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options)
        {

        }
    }
}
