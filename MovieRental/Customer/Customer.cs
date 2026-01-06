using System.ComponentModel.DataAnnotations;

namespace MovieRental.Customer;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}