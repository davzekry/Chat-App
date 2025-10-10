using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;


namespace EGRideAPI.API.Configuration
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {            
            // Identity configuration
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {               
                options.User.AllowedUserNameCharacters = null; // This allows all characters

                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
