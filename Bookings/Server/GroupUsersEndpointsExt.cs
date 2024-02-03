using Bookings.Shared;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Server;

public static class GroupUsersEndpointsExt
{
    public static RouteGroupBuilder MapUsersApi(this RouteGroupBuilder group)
    {
        group.MapGet("/users", async (ApplicationDbContext dbContext) =>
            {
                var Users = await dbContext.Users.ToListAsync();
                return Users;
            })
            .WithName("GetUsers")
            .WithOpenApi()
            .RequireAuthorization()
            .WithTags("Users");


        group.MapGet("/users/{id}", async (int id, ApplicationDbContext dbContext) =>
                await dbContext.Users.FindAsync(id)
                    is User user
                    ? Results.Ok(user)
                    : Results.NotFound())
            .WithName("GetUserById")
            .WithOpenApi()
            .WithTags("Users");


        group.MapPost("/users", async (ApplicationDbContext dbContext, User user) =>
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/Users/{user.Email}", user);
            })
            .WithName("CreateUser")
            .WithOpenApi()
            .WithTags("Users")
            .RequireAuthorization();

        group.MapPut("/users/{id}", async (ApplicationDbContext dbContext, int id, User updatedUser) =>
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    return Results.NotFound();
                }

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                // Update other properties as needed
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            })
            .WithName("UpdateUser")
            .WithOpenApi()
            .WithTags("Users")
            .RequireAuthorization();

        group.MapDelete("/users/{id}", async (ApplicationDbContext dbContext, int id) =>
            {
                var venue = await dbContext.Users.FindAsync(id);
                if (venue == null)
                {
                    return Results.NotFound();
                }

                dbContext.Users.Remove(venue);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            })
            .WithName("DeleteUser")
            .WithOpenApi()
            .WithTags("Users")
            .RequireAuthorization();


        return group;
    }
}