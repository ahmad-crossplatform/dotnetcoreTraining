using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookings.Shared;

public class Venue
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public int TypeId { get; set; }
    [ForeignKey("TypeId")]

    public string Description { get; set; }
    public int Capacity { get; set; }
    public string ImageUrl { get; set; }
    // Other properties and methods
}