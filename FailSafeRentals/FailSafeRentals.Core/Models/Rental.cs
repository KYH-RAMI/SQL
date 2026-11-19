namespace FailSafeRentals.Core.Models;

public class Rental
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalCost { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
