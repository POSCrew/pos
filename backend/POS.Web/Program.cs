using Microsoft.AspNetCore.Identity;
using POS.Infrastructure;
using POS.Web.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<POSDbContext>(builder.Configuration.GetConnectionString("POSConnectionString"));

builder.Services.AddAuthorization();
// builder.Services.AddAuthentication();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
// builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => {
    // TODO: these settings are only for development
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    options.Lockout.MaxFailedAccessAttempts = 1000;
    options.Lockout.AllowedForNewUsers = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 1;
})
    .AddEntityFrameworkStores<POSDbContext>();

builder.Services.RegisterInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapIdentityApi<IdentityUser>();

app.AddUserEndpoints();

app.Run();
