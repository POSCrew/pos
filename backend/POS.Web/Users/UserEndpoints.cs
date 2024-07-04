using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions.Data;

namespace POS.Web.Users;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var users = routes.MapGroup("users")
            .AddEndpointFilter<RequiresInitializationFilter>();
        users.MapPost("login", Login);
        users.MapPost("registerAdmin", RegisterAdmin);
        users.MapPost("changePassword", ChangePassword).RequireAuthorization();
        users.MapGet("me", GetMe).RequireAuthorization();
        users.MapPost("CreateSeller", CreateSeller).RequireAuthorization(PosIdentity.AdminPolicy);
    }

    private sealed record CreateSellerRequest(string? FirstName, string? LastName, string? UserName, string? PhoneNumber);
    private static async Task<IResult> CreateSeller(
        [FromServices] UserManager<POSUser> userManager,
        [FromServices] IRepository<POSUser> userRepo,
        [FromServices] ITransactionManager transactionManager,
        [FromBody] CreateSellerRequest request
    )
    {
        var username = request.UserName?.Trim() ?? string.Empty;
        var firstName = request.FirstName?.Trim() ?? string.Empty;
        var lastName = request.LastName?.Trim() ?? string.Empty;
        var phoneNumber = request.PhoneNumber?.Trim() ?? string.Empty;

        if(username.Any(c => !PosIdentity.ValidUserNameCharacters.Contains(c)))
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: $"invalid username provided");

        using var transaction = await transactionManager.BeginTransactionAsync();
        
        if (userRepo.Set.Any(u => u.UserName == username))
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "user already exists");

        var randomPassword = GenerateRandomPassword();

        var user = new POSUser
        {
            UserName = username,
            PhoneNumber = phoneNumber,
        };

        var res = await userManager.CreateAsync(user, randomPassword);
        if (!res.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "could not create the user",
                detail: res.Errors.Select(e => $"{e.Code}: {e.Description}").Aggregate((a, b) => a + '\n' + b));

        res = await userManager.AddToRoleAsync(user, PosIdentity.SellerRoleName);
        if (!res.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "could not create the user",
                detail: res.Errors.Select(e => $"{e.Code}: {e.Description}").Aggregate((a, b) => a + '\n' + b));

        await transaction.CommitAsync();
        return Results.Ok(new { Password = randomPassword });
    }

    private sealed record ChangePasswordRequest(string? PreviousPassword, string? NewPassword);
    private static async Task<IResult> ChangePassword(
        [FromServices] UserManager<POSUser> userManager,
        [FromServices] IHttpContextAccessor contextAccessor,
        [FromServices] IRepository<POSUser> userRepo,
        [FromBody] ChangePasswordRequest request
    )
    {
        var previousPassword = request.PreviousPassword?.Trim() ?? string.Empty;
        var newPassword = request.NewPassword?.Trim() ?? string.Empty;

        var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
        var user = userRepo.Set.FirstOrDefault(u => u.UserName == username);

        if(user is null)
            return Results.Problem(statusCode: (int)HttpStatusCode.NotFound, title: "user not found");

        var result = await userManager.ChangePasswordAsync(user, previousPassword, newPassword);
        if (!result.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "changing password failed");
        
        return Results.Ok();
    }

    private static IResult GetMe(ClaimsPrincipal user)
    {
        var username = user?.Identity?.Name ?? string.Empty;
        
        var roles = user?.FindAll(ClaimTypes.Role).Select(r => r.Value);
        return Results.Ok(
            new
            {
                id = user?.FindFirstValue(ClaimTypes.NameIdentifier),
                username,
                roles
            }
        );
    }

    private sealed record LoginRequest(string? UserName, string? Password);
    private static async Task<IResult> Login(
        [FromServices] SignInManager<POSUser> signInManager,
        [FromServices] IRepository<POSUser> userRepo,
        [FromBody] LoginRequest loginRequest
    )
    {
        var username = loginRequest.UserName?.Trim() ?? string.Empty;
        var password = loginRequest.Password?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return Results.Problem(statusCode: (int)HttpStatusCode.Unauthorized, title: "bad login request");

        var user = userRepo.Set.FirstOrDefault(u => u.UserName == username);

        if (user is null)
            return Results.Problem(statusCode: (int)HttpStatusCode.NotFound, title: "invalid id");

        var result = await signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!result.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "login attempt failed");

        await signInManager.SignInAsync(user, true);

        return Results.Ok();
    }

    private static async Task<IResult> RegisterAdmin(
        [FromServices] UserManager<POSUser> userManager,
        [FromServices] IRepository<POSUser> userRepo,
        [FromServices] ITransactionManager transactionManager
    )
    {
        using var transaction = await transactionManager.BeginTransactionAsync();

        if (userRepo.Set.Any(u => u.UserName == "admin"))
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "admin user already exists");

        var randomPassword = GenerateRandomPassword();

        var user = new POSUser
        {
            UserName = "admin",
        };

        var res = await userManager.CreateAsync(user, randomPassword);
        if (!res.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "could not create admin user",
                detail: res.Errors.Select(e => $"{e.Code}: {e.Description}").Aggregate((a, b) => a + '\n' + b));

        res = await userManager.AddToRoleAsync(user, PosIdentity.AdminRoleName);
        if (!res.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "could not create admin user",
                detail: res.Errors.Select(e => $"{e.Code}: {e.Description}").Aggregate((a, b) => a + '\n' + b));

        await transaction.CommitAsync();
        return Results.Ok(new { Password = randomPassword });
    }

    private static string GenerateRandomPassword()
    {
        Span<byte> bytes = stackalloc byte[4];
        RandomNumberGenerator.Fill(bytes);
        return BitConverter.ToInt32(bytes).ToString("x");
    }
}
