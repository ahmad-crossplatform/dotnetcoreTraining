using System.ComponentModel.DataAnnotations;

namespace Bookings.Shared;

public class User
{
    [Key]
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }

    public virtual ICollection<Booking> Bookings { get; set; }

}