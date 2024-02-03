using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookings.Shared;

public class VenueType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }

    public int? ParentId { get; set; }
    [ForeignKey("ParentId")]
    public virtual VenueType Parent { get; set; }

}