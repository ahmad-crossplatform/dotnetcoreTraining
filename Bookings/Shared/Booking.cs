using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookings.Shared;

public class Booking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    public int VenueId { get; set; }
    [ForeignKey("VenueId")]
    public virtual Venue Venue { get; set; }

    public string UserEmail { get; set; }
    [ForeignKey("UserEmail")]
    public virtual User User { get; set; }
    public DateTime Date { get; set; }
    public int NumberOfAttendees { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
}