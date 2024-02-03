using Bookings.Shared;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Server;

public static class GroupVenuesEndpointsExt
{
    public static RouteGroupBuilder MapVenuesApi(this RouteGroupBuilder group)
    {
        group.MapGet("/venues", async (ApplicationDbContext dbContext) =>
            {
                var venues = await dbContext.Venues.ToListAsync();
                return venues;
            })
            .WithName("GetVenues")
            .WithTags("Venues")
            .WithOpenApi();


        group.MapGet("/venues/{id}", async (int id, ApplicationDbContext dbContext) =>
                await dbContext.Venues.FindAsync(id)
                    is Venue venue
                    ? Results.Ok(venue)
                    : Results.NotFound())
            .WithName("GetVenueById")
            .WithTags("Venues")
            .WithOpenApi();


        group.MapPost("/venues", async (ApplicationDbContext dbContext, Venue venue) =>
            {
                dbContext.Venues.Add(venue);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/venues/{venue.Id}", venue);
            })
            .WithName("CreateVenue")
            .WithOpenApi()
            .WithTags("Venues")
            .RequireAuthorization();

        group.MapPut("/venues/{id}", async (ApplicationDbContext dbContext, int id, Venue updatedVenue) =>
            {
                var venue = await dbContext.Venues.FindAsync(id);
                if (venue == null)
                {
                    return Results.NotFound();
                }

                venue.Name = updatedVenue.Name;
                venue.Street = updatedVenue.Street;
                venue.City = updatedVenue.City;
                venue.State = updatedVenue.State;
                // Update other properties as needed
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            })
            .WithName("UpdateVenue")
            .WithOpenApi()
            .WithTags("Venues")
            .RequireAuthorization();

        group.MapDelete("/venues/{id}", async (ApplicationDbContext dbContext, int id) =>
            {
                var venue = await dbContext.Venues.FindAsync(id);
                if (venue == null)
                {
                    return Results.NotFound();
                }

                dbContext.Venues.Remove(venue);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            })
            .WithName("DeleteVenue")
            .WithOpenApi()
            .WithTags("Venues")
            .RequireAuthorization();


        return group;
    }
}