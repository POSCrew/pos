using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using POS.Application.Data;
using POS.Infrastructure;

namespace POS.Web.Users;

public static class UserEndponits
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var users = routes.MapGroup("users");
        users.MapPost("login", Login);
        users.MapPost("registerAdmin", RegisterAdmin);
        users.MapGet("restricted", Restricted).RequireAuthorization();
        users.MapGet("restrictedAdmin", RestrictedAdmin).RequireAuthorization(PosIdentity.Admin);
    }

    private static IResult RestrictedAdmin(HttpContext context)
    {
        return Results.Ok("OK");
    }

    private static IResult Restricted(HttpContext context)
    {
        return Results.Ok("OK");
    }

    private sealed record LoginRequest(string? UserName, string? Password);
    private static async Task<IResult> Login(
        [FromServices] SignInManager<IdentityUser> signInManager,
        [FromServices] IRepository<IdentityUser> userRepo,
        [FromBody] LoginRequest loginRequest
    )
    {
        var username = loginRequest.UserName?.Trim() ?? string.Empty;
        var password = loginRequest.Password?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return Results.Problem(statusCode: (int)HttpStatusCode.Unauthorized, title: "bad login request");

        var user = userRepo.Set.FirstOrDefault(u => u.UserName == username);

        if (user is null)
            return Results.Problem(statusCode: (int)HttpStatusCode.NotFound, title: "user not found");

        var result = await signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!result.Succeeded)
            return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "login attempt failed");

        await signInManager.SignInAsync(user, true);

        return Results.Ok();
    }

    private static async Task<IResult> RegisterAdmin(
        [FromServices] UserManager<IdentityUser> userManager,
        [FromServices] IRepository<IdentityUser> userRepo,
        [FromServices] POSDbContext context
    )
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        if (userRepo.Set.Any(u => u.UserName == "admin"))
            return Results.Problem(statusCode: 400, title: "admin user already exists");

        var randomPassword = "1234"; // TODO: randomly generate a password

        var user = new IdentityUser
        {
            UserName = "admin",
        };

        var res = await userManager.CreateAsync(user, randomPassword);
        if (!res.Succeeded)
            return Results.Problem(statusCode: 400, title: "could not create admin user",
                detail: res.Errors.Select(e => $"{e.Code}: {e.Description}").Aggregate((a, b) => a + '\n' + b));

        res = await userManager.AddToRoleAsync(user, PosIdentity.Admin);
        if (!res.Succeeded)
            return Results.Problem(statusCode: 400, title: "could not create admin user",
                detail: res.Errors.Select(e => $"{e.Code}: {e.Description}").Aggregate((a, b) => a + '\n' + b));

        await transaction.CommitAsync();
        return Results.Ok(new { Password = randomPassword });
    }
}
