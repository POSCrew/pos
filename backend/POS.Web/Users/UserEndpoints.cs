
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Data;

namespace POS.Web.Users;

public static class UserEndponits
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var users = routes.MapGroup("users");
        users.MapPost("registerAdmin", RegisterAdmin);
    }

    private static async Task<IResult> RegisterAdmin(
        [FromServices] UserManager<IdentityUser> userManager,
        [FromServices] IRepository<IdentityUser> userRepo
    )
    {
        if (userRepo.Set.Any(u => u.UserName == "admin"))
            return Results.Problem(detail: null, instance: null, statusCode: 400, title: "admin user already exists", type: null, extensions: null);

        var randomPassword = "1234"; // TODO: randomly generate a password

        var res = await userManager.CreateAsync(new IdentityUser
        {
            UserName = "admin",
        }, randomPassword);

        if (res.Succeeded)
            return Results.Ok(new { Password = randomPassword });

        return Results.Problem(statusCode: 400, title: "could not create admin user", detail: res.Errors.Select(e => $"{e.Code}: {e.Description}\n").Aggregate((a, b) => a + b));
    }
}
