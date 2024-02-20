using Bookings.Shared;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Server;

public static class GroupBookingEndpointsExt
{
    public static RouteGroupBuilder MapBookingsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/bookings", async (ApplicationDbContext dbContext) =>
        {
            var bookings = await dbContext.Bookings.ToListAsync();
            return bookings;
        })
        .WithName("GetBookings")
        .WithTags("Bookings")
        .WithOpenApi();

        group.MapGet("/bookings/{id}", async (int id, ApplicationDbContext dbContext) =>
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            return booking != null ? Results.Ok(booking) : Results.NotFound();
        })
        .WithName("GetBookingById")
        .WithTags("Bookings")
        .WithOpenApi();

        group.MapPost("/bookings", async (ApplicationDbContext dbContext, Booking booking) =>
        {
            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/bookings/{booking.Id}", booking);
        })
        .WithName("CreateBooking")
        .WithOpenApi()
        .WithTags("Bookings")
        .RequireAuthorization();

        group.MapPut("/bookings/{id}", async (ApplicationDbContext dbContext, int id, Booking updatedBooking) =>
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return Results.NotFound();
            }

            booking.ContactPhone = updatedBooking.ContactPhone;
            booking.ContactEmail = updatedBooking.ContactEmail;
            booking.Date = updatedBooking.Date;
            booking.VenueId = updatedBooking.VenueId;
            booking.Id = updatedBooking.Id;
            booking.NumberOfAttendees = updatedBooking.NumberOfAttendees;
            // Update other properties as needed
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        })
        .WithName("UpdateBooking")
        .WithOpenApi()
        .WithTags("Bookings")
        .RequireAuthorization();

        group.MapDelete("/bookings/{id}", async (ApplicationDbContext dbContext, int id) =>
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return Results.NotFound();
            }

            dbContext.Bookings.Remove(booking);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        })
        .WithName("DeleteBooking")
        .WithOpenApi()
        .WithTags("Bookings")
        .RequireAuthorization();

        return group;
    }
}