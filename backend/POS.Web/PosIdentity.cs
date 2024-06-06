
using Microsoft.AspNetCore.Identity;
using POS.Infrastructure;

public static class PosIdentity
{
    public const string AdminRoleName = "Admin";
    public const string SellerRoleName = "Seller";
    public const string AdminPolicy = "Admin";
    public const string SellerOrAdminPolicy = "AdminSeller";
    public const string ValidUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";

    internal static void SetupAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(config =>
        {
            config.AddPolicy(AdminPolicy, policy =>
            {
                policy.RequireRole(AdminRoleName);
            });

            config.AddPolicy(SellerOrAdminPolicy, policy =>
            {
                policy.RequireRole(SellerRoleName, AdminRoleName);
            });
        });
    }

    internal static void SetupAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<POSUser, IdentityRole>(options =>
        {
            // TODO: these settings are only for development
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = false;
            options.User.AllowedUserNameCharacters = ValidUserNameCharacters;
            options.Lockout.MaxFailedAccessAttempts = 1000;
            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequiredLength = 1;
        }).AddEntityFrameworkStores<POSDbContext>()
            .AddRoles<IdentityRole>();
    }
}
